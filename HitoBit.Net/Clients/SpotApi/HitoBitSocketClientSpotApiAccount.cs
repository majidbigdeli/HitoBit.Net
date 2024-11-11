using HitoBit.Net.Objects.Models.Spot.Socket;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using HitoBit.Net.Objects.Sockets.Subscriptions;
using HitoBit.Net.Objects.Models;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class HitoBitSocketClientSpotApiAccount : IHitoBitSocketClientSpotApiAccount
    {
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
        public async Task<CallResult<HitoBitResponse<HitoBitAccountInfo>>> GetAccountInfoAsync(bool? omitZeroBalances = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("omitZeroBalances", omitZeroBalances?.ToString().ToLowerInvariant());
            return await _client.QueryAsync<HitoBitAccountInfo>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"account.status", parameters, true, true, weight: 20, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Rate Limits

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitCurrentRateLimit>>>> GetOrderRateLimitsAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbols", symbols);
            return await _client.QueryAsync<IEnumerable<HitoBitCurrentRateLimit>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"account.rateLimits.orders", parameters, true, true, weight: 40, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Start User Stream

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<string>>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var result = await _client.QueryAsync<HitoBitListenKey>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"userDataStream.start", new Dictionary<string, object>(), true, weight: 2, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsError<HitoBitResponse<string>>(result.Error!);

            return result.As(new HitoBitResponse<string>
            {
                Ratelimits = result.Data!.Ratelimits!,
                Result = result.Data!.Result?.ListenKey!
            });
        }

        #endregion

        #region Keep Alive User Stream

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<object>>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("listenKey", listenKey);
            return await _client.QueryAsync<object>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"userDataStream.ping", parameters, true, weight: 2, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Stop User Stream

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<object>>> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("listenKey", listenKey);
            return await _client.QueryAsync<object>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"userDataStream.stop", parameters, true, weight: 2, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Streams

        #region User Data Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitStreamOrderUpdate>>? onOrderUpdateMessage = null,
            Action<DataEvent<HitoBitStreamOrderList>>? onOcoOrderUpdateMessage = null,
            Action<DataEvent<HitoBitStreamPositionsUpdate>>? onAccountPositionMessage = null,
            Action<DataEvent<HitoBitStreamBalanceUpdate>>? onAccountBalanceUpdate = null,
            Action<DataEvent<HitoBitStreamEvent>>? onListenKeyExpired = null,
            CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));
            var subscription = new HitoBitSpotUserDataSubscription(_logger, new List<string> { listenKey }, onOrderUpdateMessage, onOcoOrderUpdateMessage, onAccountPositionMessage, onAccountBalanceUpdate, onListenKeyExpired, false);
            return await _client.SubscribeInternalAsync(_client.BaseAddress, subscription, ct).ConfigureAwait(false);
        }
        #endregion

        #endregion
    }
}
 