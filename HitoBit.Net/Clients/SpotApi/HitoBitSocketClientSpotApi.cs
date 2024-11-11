using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Options;
using HitoBit.Net.Objects.Sockets;
using HitoBit.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal partial class HitoBitSocketClientSpotApi : SocketApiClient, IHitoBitSocketClientSpotApi
    {
        #region fields
        /// <inheritdoc />
        public new HitoBitSocketOptions ClientOptions => (HitoBitSocketOptions)base.ClientOptions;
        /// <inheritdoc />
        public new HitoBitSocketApiOptions ApiOptions => (HitoBitSocketApiOptions)base.ApiOptions;

        internal HitoBitExchangeInfo? _exchangeInfo;
        internal DateTime? _lastExchangeInfoUpdate;
        internal readonly string _brokerId;

        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        private static readonly MessagePath _streamPath = MessagePath.Get().Property("stream");
        #endregion

        /// <inheritdoc />
        public IHitoBitSocketClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IHitoBitSocketClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IHitoBitSocketClientSpotApiTrading Trading { get; }

        #region constructor/destructor

        internal HitoBitSocketClientSpotApi(ILogger logger, HitoBitSocketOptions options) :
            base(logger, options.Environment.SpotSocketStreamAddress, options, options.SpotOptions)
        {
            Account = new HitoBitSocketClientSpotApiAccount(logger, this);
            ExchangeData = new HitoBitSocketClientSpotApiExchangeData(logger, this);
            Trading = new HitoBitSocketClientSpotApiTrading(logger, this);

            _brokerId = !string.IsNullOrEmpty(options.SpotOptions.BrokerId) ? options.SpotOptions.BrokerId! : "x-VICEW9VV";

            // When sending more than 4000 bytes the server responds very delayed (somehow connected to the websocket keep alive interval)
            // See https://dev.hitobit.vision/t/socket-live-subscribing-server-delay/9645/2
            // To prevent issues we keep below this
            MessageSendSizeLimit = 4000;

            RateLimiter = HitoBitExchange.RateLimiter.SpotSocket;

            SetDedicatedConnection(ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), true);
        }
        #endregion

        public IHitoBitSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => HitoBitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider(credentials);

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        protected override IByteMessageAccessor CreateAccessor() => new SystemTextJsonByteMessageAccessor();

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var id = message.GetValue<int?>(_idPath);
            if (id != null)
                return id.ToString();

            return message.GetValue<string>(_streamPath);
        }

        internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<DataEvent<T>> onData, CancellationToken ct)
        {
            var subscription = new HitoBitSubscription<T>(_logger, topics.ToList(), onData, false);
            return base.SubscribeAsync(url.AppendPath("stream"), subscription, ct);
        }

        internal Task<CallResult<UpdateSubscription>> SubscribeInternalAsync(string url, Subscription subscription, CancellationToken ct)
        {
            return base.SubscribeAsync(url.AppendPath("stream"), subscription, ct);
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

        internal async Task<HitoBitTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, SpotOrderType? type)
        {
            if (ApiOptions.TradeRulesBehaviour == TradeRulesBehaviour.None)
                return HitoBitTradeRuleResult.CreatePassed(quantity, quoteQuantity, price, stopPrice);

            if (_exchangeInfo == null || _lastExchangeInfoUpdate == null || (DateTime.UtcNow - _lastExchangeInfoUpdate.Value).TotalMinutes > ApiOptions.TradeRulesUpdateInterval.TotalMinutes)
                await ExchangeData.GetExchangeInfoAsync().ConfigureAwait(false);

            if (_exchangeInfo == null)
                return HitoBitTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

            return HitoBitHelpers.ValidateTradeRules(_logger, ApiOptions.TradeRulesBehaviour, _exchangeInfo, symbol, quantity, quoteQuantity, price, stopPrice, type);
        }
    }
}
