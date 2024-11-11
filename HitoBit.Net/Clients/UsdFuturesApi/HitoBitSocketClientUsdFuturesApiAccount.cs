using HitoBit.Net.Clients.SpotApi;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Sockets;
using CryptoExchange.Net.Objects.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Clients.UsdFuturesApi
{
    internal class HitoBitSocketClientUsdFuturesApiAccount : IHitoBitSocketClientUsdFuturesApiAccount
    {
        private readonly HitoBitSocketClientUsdFuturesApi _client;
        private readonly ILogger _logger;

        internal HitoBitSocketClientUsdFuturesApiAccount(ILogger logger, HitoBitSocketClientUsdFuturesApi client)
        {
            _client = client;
            _logger = logger;
        }

        #region Queries


        #region Future Account Balance

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitUsdFuturesAccountBalance>>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture));

            return await _client.QueryAsync<IEnumerable<HitoBitUsdFuturesAccountBalance>>(_client.ClientOptions.Environment.UsdFuturesSocketApiAddress!.AppendPath("ws-fapi/v1"), $"v2/account.balance", parameters, true, true, weight: 5, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Info

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitFuturesAccountInfoV3>>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture));
            return await _client.QueryAsync<HitoBitFuturesAccountInfoV3>(_client.ClientOptions.Environment.UsdFuturesSocketApiAddress!.AppendPath("ws-fapi/v1"), $"v2/account.status", parameters, true, true, weight: 5, ct: ct).ConfigureAwait(false);            
        }

        #endregion

        #endregion

        #region Streams

        #region User Data Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitFuturesStreamConfigUpdate>>? onConfigUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamMarginUpdate>>? onMarginUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamTradeUpdate>>? onTradeUpdate = null,
            Action<DataEvent<HitoBitStreamEvent>>? onListenKeyExpired = null,
            Action<DataEvent<HitoBitStrategyUpdate>>? onStrategyUpdate = null,
            Action<DataEvent<HitoBitGridUpdate>>? onGridUpdate = null,
            Action<DataEvent<HitoBitConditionOrderTriggerRejectUpdate>>? onConditionalOrderTriggerRejectUpdate = null,
            CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var subscription = new HitoBitUsdFuturesUserDataSubscription(_logger, new List<string> { listenKey }, onOrderUpdate, onTradeUpdate, onConfigUpdate, onMarginUpdate, onAccountUpdate, onListenKeyExpired, onStrategyUpdate, onGridUpdate, onConditionalOrderTriggerRejectUpdate);
            return await _client.SubscribeInternalAsync(_client.BaseAddress, subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion
    }
}
