using HitoBit.Net.Clients.SpotApi;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Models.Spot.Socket;
using HitoBit.Net.Objects.Options;
using HitoBit.Net.Objects.Sockets;
using HitoBit.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.Clients.UsdFuturesApi
{
    /// <summary>
    /// Client providing access to the HitoBit Usd futures websocket Api
    /// </summary>
    internal partial class HitoBitSocketClientUsdFuturesApi : SocketApiClient, IHitoBitSocketClientUsdFuturesApi
    {
        /// <inheritdoc />
        public new HitoBitSocketOptions ClientOptions => (HitoBitSocketOptions)base.ClientOptions;
        /// <inheritdoc />
        public new HitoBitSocketApiOptions ApiOptions => (HitoBitSocketApiOptions)base.ApiOptions;

        #region fields
        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        private static readonly MessagePath _streamPath = MessagePath.Get().Property("stream");

        internal HitoBitFuturesUsdtExchangeInfo? _exchangeInfo;
        internal DateTime? _lastExchangeInfoUpdate;
        internal readonly string _brokerId;
        #endregion

        /// <inheritdoc />
        public IHitoBitSocketClientUsdFuturesApiAccount Account { get; }
        /// <inheritdoc />
        public IHitoBitSocketClientUsdFuturesApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IHitoBitSocketClientUsdFuturesApiTrading Trading { get; }

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of HitoBitSocketClientUsdFuturesStreams
        /// </summary>
        internal HitoBitSocketClientUsdFuturesApi(ILogger logger, HitoBitSocketOptions options) :
            base(logger, options.Environment.UsdFuturesSocketAddress!, options, options.UsdFuturesOptions)
        {
            Account = new HitoBitSocketClientUsdFuturesApiAccount(logger, this);
            ExchangeData = new HitoBitSocketClientUsdFuturesApiExchangeData(logger, this);
            Trading = new HitoBitSocketClientUsdFuturesApiTrading(logger, this);

            _brokerId = !string.IsNullOrEmpty(options.UsdFuturesOptions.BrokerId) ? options.UsdFuturesOptions.BrokerId! : "x-d63tKbx3";

            // When sending more than 4000 bytes the server responds very delayed (somehow connected to the websocket keep alive interval on framework level)
            // See https://dev.hitobit.vision/t/socket-live-subscribing-server-delay/9645/2
            // To prevent issues we keep below this
            MessageSendSizeLimit = 4000;

            RateLimiter = HitoBitExchange.RateLimiter.FuturesSocket;
        }
        #endregion 

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider(credentials);

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => HitoBitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();
        protected override IByteMessageAccessor CreateAccessor() => new SystemTextJsonByteMessageAccessor();
        public IHitoBitSocketClientUsdFuturesApiShared SharedClient => this;


        internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<DataEvent<T>> onData, CancellationToken ct)
        {
            var subscription = new HitoBitSubscription<T>(_logger, topics.ToList(), onData, false);
            return SubscribeAsync(url.AppendPath("stream"), subscription, ct);
        }

        internal Task<CallResult<UpdateSubscription>> SubscribeInternalAsync(string url, Subscription subscription, CancellationToken ct)
        {
            return base.SubscribeAsync(url.AppendPath("stream"), subscription, ct);
        }

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var id = message.GetValue<int?>(_idPath);
            if (id != null)
                return id.ToString();

            return message.GetValue<string>(_streamPath);
        }

        internal async Task<CallResult<HitoBitResponse<T>>> QueryAsync<T>(string url, string method, Dictionary<string, object> parameters, bool authenticated = false, bool sign = false, int weight = 1, CancellationToken ct = default)
        {
            if (authenticated)
            {
                if (AuthenticationProvider == null)
                    throw new InvalidOperationException("No credentials provided for authenticated endpoint");

                var authProvider = (HitoBitAuthenticationProvider)AuthenticationProvider;
                if (sign)
                {
                    parameters = authProvider.AuthenticateSocketParameters(parameters);
                }
                else
                {
                    parameters.Add("apiKey", authProvider.ApiKey);
                }
            }

            var request = new HitoBitSocketQuery
            {
                Method = method,
                Params = parameters,
                Id = ExchangeHelpers.NextId()
            };

            var query = new HitoBitSpotQuery<HitoBitResponse<T>>(request, false, weight);
            var result = await QueryAsync(url, query, ct).ConfigureAwait(false);
            if (!result.Success && result.Error is HitoBitRateLimitError rle)
            {
                if (rle.RetryAfter != null && RateLimiter != null && ClientOptions.RateLimiterEnabled)
                {
                    _logger.LogWarning("Ratelimit error from server, pausing requests until {Until}", rle.RetryAfter.Value);
                    await RateLimiter.SetRetryAfterGuardAsync(rle.RetryAfter.Value).ConfigureAwait(false);
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(null);
    }
}
