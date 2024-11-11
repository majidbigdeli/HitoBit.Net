using HitoBit.Net.Enums;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Objects.Sockets;

namespace HitoBit.Net.Interfaces.Clients.UsdFuturesApi
{
    /// <summary>
    /// HitoBit USD futures account websocket API
    /// </summary>
    public interface IHitoBitSocketClientUsdFuturesApiAccount
    {
        /// <summary>
        /// Gets account balances
        /// <para><a href="https://hitobit-docs.github.io/apidocs/futures/en/#futures-account-balance-v2-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The account information</returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitUsdFuturesAccountBalance>>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get account information, including position and balances
        /// <para><a href="https://developers.hitobit.com/docs/derivatives/usds-margined-futures/account/rest-api/Account-Information-V3" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        Task<CallResult<HitoBitResponse<HitoBitFuturesAccountInfoV3>>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the account update stream. Prior to using this, the <see cref="IHitoBitRestClientUsdFuturesApiAccount.StartUserStreamAsync(CancellationToken)">restClient.UsdFuturesApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/futures/en/#user-data-streams" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the <see cref="IHitoBitRestClientUsdFuturesApiAccount.StartUserStreamAsync(CancellationToken)">restClient.UsdFuturesApi.Account.StartUserStreamAsync</see> method</param>
        /// <param name="onLeverageUpdate">The event handler for leverage changed update</param>
        /// <param name="onMarginUpdate">The event handler for whenever a margin has changed</param>
        /// <param name="onAccountUpdate">The event handler for whenever an account update is received</param>
        /// <param name="onOrderUpdate">The event handler for whenever an order status update is received</param>
        /// <param name="onTradeUpdate">The event handler for whenever an trade status update is received</param>
        /// <param name="onListenKeyExpired">Responds when the listen key for the stream has expired. Initiate a new instance of the stream here</param>
        /// <param name="onStrategyUpdate">The event handler for whenever a strategy update is received</param>
        /// <param name="onGridUpdate">The event handler for whenever a grid update is received</param>
        /// <param name="onConditionalOrderTriggerRejectUpdate">The event handler for whenever a trigger order failed to place an order</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitFuturesStreamConfigUpdate>>? onLeverageUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamMarginUpdate>>? onMarginUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamTradeUpdate>>? onTradeUpdate = null,
            Action<DataEvent<HitoBitStreamEvent>>? onListenKeyExpired = null,
            Action<DataEvent<HitoBitStrategyUpdate>>? onStrategyUpdate = null,
            Action<DataEvent<HitoBitGridUpdate>>? onGridUpdate = null,
            Action<DataEvent<HitoBitConditionOrderTriggerRejectUpdate>>? onConditionalOrderTriggerRejectUpdate = null,
            CancellationToken ct = default);
    }
}
