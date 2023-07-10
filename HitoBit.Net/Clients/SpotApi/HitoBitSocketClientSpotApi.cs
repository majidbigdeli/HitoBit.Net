using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class HitoBitSocketClientSpotApi : SocketApiClient, IHitoBitSocketClientSpotApi
    {
        #region fields
        /// <inheritdoc />
        public new HitoBitSocketOptions ClientOptions => (HitoBitSocketOptions)base.ClientOptions;
        /// <inheritdoc />
        public new HitoBitSocketApiOptions ApiOptions => (HitoBitSocketApiOptions)base.ApiOptions;

        internal HitoBitExchangeInfo? _exchangeInfo;
        internal DateTime? _lastExchangeInfoUpdate;
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
            SetDataInterpreter((data) => string.Empty, null);
            RateLimitPerSocketPerSecond = 4;

            Account = new HitoBitSocketClientSpotApiAccount(logger, this);
            ExchangeData = new HitoBitSocketClientSpotApiExchangeData(logger, this);
            Trading = new HitoBitSocketClientSpotApiTrading(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider(credentials);

        internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<DataEvent<T>> onData, CancellationToken ct)
        {
            var request = new HitoBitSocketRequest
            {
                Method = "SUBSCRIBE",
                Params = topics.ToArray(),
                Id = NextId()
            };

            return SubscribeAsync(url.AppendPath("stream"), request, null, false, onData, ct);
        }

        internal Task<CallResult<HitoBitResponse<T>>> QueryAsync<T>(string url, string method, Dictionary<string, object> parameters, bool authenticated = false, bool sign = false)
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
                    parameters.Add("apiKey", authProvider.GetApiKey());
                }
            }

            var request = new HitoBitSocketQuery
            {
                Method = method,
                Params = parameters,
                Id = NextId()
            };

            return QueryAsync<HitoBitResponse<T>>(url, request, false);
        }

        internal CallResult<T> DeserializeInternal<T>(JToken obj, JsonSerializer? serializer = null, int? requestId = null)
        {
            return base.Deserialize<T>(obj, serializer, requestId);
        }

        /// <inheritdoc />
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            callResult = null!;
            var bRequest = (HitoBitSocketQuery)request;
            if (bRequest.Id != data["id"]?.Value<int>())
                return false;

            var status = data["status"]?.Value<int>();
            if (status != 200)
            {
                var error = data["error"]!;
                callResult = new CallResult<T>(new ServerError(error["code"]!.Value<int>(), error["msg"]!.Value<string>()!));
                return true;
            }
            callResult = Deserialize<T>(data!);
            return true;
        }

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
        {
            var result = message;

            if (result != null && message.Count() > 0)
            {
                if (message.Value<bool>("hasError"))
                {

                    callResult = new CallResult<object>(new ServerError("Unknown error: " + message.Value<string>("data")));
                    return true;
                }

                _logger.Log(LogLevel.Trace, $"Socket {s.SocketId} Subscription completed");



                callResult = new CallResult<object>(new object());
                return true;
            }

            callResult = new CallResult<object>(new ServerError("Unknown error: " + message));
            return false;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            if (message.Type != JTokenType.Object)
                return false;

            var bRequest = (HitoBitSocketRequest)request;
            var stream = message["stream"];
            if (stream == null)
                return false;

            return bRequest.Params.Contains(stream.ToString());
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            return true;
        }

        /// <inheritdoc />
        protected override Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscription)
        {
            var topics = ((HitoBitSocketRequest)subscription.Request!).Params;
            var unsub = new HitoBitSocketRequest { Method = "UNSUBSCRIBE", Params = topics, Id = NextId() };
            var result = false;

            if (!connection.Connected)
                return true;

            await connection.SendAndWaitAsync(unsub, ClientOptions.RequestTimeout, null, data =>
            {
                if (data.Type != JTokenType.Object)
                    return false;

                var id = data["id"];
                if (id == null)
                    return false;

                if ((int)id != unsub.Id)
                    return false;

                var result = data["result"];
                if (result?.Type == JTokenType.Null)
                {
                    result = true;
                    return true;
                }

                return true;
            }).ConfigureAwait(false);
            return result;
        }

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

        protected override IWebsocket CreateSocket(string address)
        {
            return new SignalRCryptoExchangeWebSocketClient(_logger, GetWebSocketParameters(address), null);
        }
    }
}
