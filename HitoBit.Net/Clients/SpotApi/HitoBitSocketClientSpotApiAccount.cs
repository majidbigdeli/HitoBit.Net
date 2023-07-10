using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using HitoBit.Net.Objects.Models.Spot;
using CryptoExchange.Net.Converters;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class HitoBitSocketClientSpotApiAccount : IHitoBitSocketClientSpotApiAccount
    {
        private const string executionUpdateEvent = "executionReport";
        private const string ocoOrderUpdateEvent = "listStatus";
        private const string accountPositionUpdateEvent = "outboundAccountPosition";
        private const string balanceUpdateEvent = "balanceUpdate";

        private readonly HitoBitSocketClientSpotApi _client;

        private readonly ILogger _logger;

        #region constructor/destructor

        internal HitoBitSocketClientSpotApiAccount(ILogger logger, HitoBitSocketClientSpotApi client)
        {
            _client = client;
            _logger = logger;
        }

        #endregion

        #region Queries

        #region Get Account Info

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitAccountInfo>>> GetAccountInfoAsync(IEnumerable<string>? symbols = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbols", symbols);
            return await _client.QueryAsync<HitoBitAccountInfo>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"account.status", parameters, true, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Rate Limits

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitCurrentRateLimit>>>> GetOrderRateLimitsAsync(IEnumerable<string>? symbols = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbols", symbols);
            return await _client.QueryAsync<IEnumerable<HitoBitCurrentRateLimit>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"account.rateLimits.orders", parameters, true, true).ConfigureAwait(false);
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<string>>> StartUserStreamAsync()
        {
            var result = await _client.QueryAsync<HitoBitListenKey>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"userDataStream.start", new Dictionary<string, object>(), true).ConfigureAwait(false);
            if (!result)
                return result.AsError<HitoBitResponse<string>>(result.Error!);

            return result.As(new HitoBitResponse<string>
            {
                Ratelimits = result.Data!.Ratelimits!,
                Result = result.Data!.Result?.ListenKey!
            });
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<object>>> KeepAliveUserStreamAsync(string listenKey)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("listenKey", listenKey);
            return await _client.QueryAsync<object>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"userDataStream.ping", parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<object>>> StopUserStreamAsync(string listenKey)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("listenKey", listenKey);
            return await _client.QueryAsync<object>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"userDataStream.stop", parameters, true).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Streams

        #region User Data Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitStreamOrderUpdate>>? onOrderUpdateMessage,
            Action<DataEvent<HitoBitStreamOrderList>>? onOcoOrderUpdateMessage,
            Action<DataEvent<HitoBitStreamPositionsUpdate>>? onAccountPositionMessage,
            Action<DataEvent<HitoBitStreamBalanceUpdate>>? onAccountBalanceUpdate,
            CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var handler = new Action<DataEvent<string>>(data =>
            {
                var combinedToken = JToken.Parse(data.Data);
                var token = combinedToken["data"];
                if (token == null)
                    return;

                var evnt = token["e"]?.ToString();
                if (evnt == null)
                    return;

                switch (evnt)
                {
                    case executionUpdateEvent:
                        {
                            var result = _client.DeserializeInternal<HitoBitStreamOrderUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onOrderUpdateMessage?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                            }
                            else
                            {
                                _logger.Log(LogLevel.Warning,
                                    "Couldn't deserialize data received from order stream: " + result.Error);
                            }

                            break;
                        }
                    case ocoOrderUpdateEvent:
                        {
                            var result = _client.DeserializeInternal<HitoBitStreamOrderList>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onOcoOrderUpdateMessage?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                            }
                            else
                            {
                                _logger.Log(LogLevel.Warning,
                                    "Couldn't deserialize data received from oco order stream: " + result.Error);
                            }

                            break;
                        }
                    case accountPositionUpdateEvent:
                        {
                            var result = _client.DeserializeInternal<HitoBitStreamPositionsUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onAccountPositionMessage?.Invoke(data.As(result.Data));
                            }
                            else
                            {
                                _logger.Log(LogLevel.Warning,
                                    "Couldn't deserialize data received from account position stream: " + result.Error);
                            }

                            break;
                        }
                    case balanceUpdateEvent:
                        {
                            var result = _client.DeserializeInternal<HitoBitStreamBalanceUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onAccountBalanceUpdate?.Invoke(data.As(result.Data, result.Data.Asset));
                            }
                            else
                            {
                                _logger.Log(LogLevel.Warning,
                                    "Couldn't deserialize data received from account position stream: " + result.Error);
                            }

                            break;
                        }
                    default:
                        _logger.Log(LogLevel.Warning, $"Received unknown user data event {evnt}: " + data);
                        break;
                }
            });

            return await _client.SubscribeAsync(_client.BaseAddress, new[] { listenKey }, handler, ct).ConfigureAwait(false);
        }
        #endregion

        #endregion
    }
}
