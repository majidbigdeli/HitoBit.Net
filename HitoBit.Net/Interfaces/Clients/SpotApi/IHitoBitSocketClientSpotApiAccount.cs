using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HitoBit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// HitoBit Spot Account socket requests and subscriptions
    /// </summary>
    public interface IHitoBitSocketClientSpotApiAccount
    {
        /// <summary>
        /// Gets account information, including balances
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/websocket_api/en/#account-information-user_data" /></para>
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<HitoBitAccountInfo>>> GetAccountInfoAsync(IEnumerable<string>? symbols = null);

        /// <summary>
        /// Get order rate limit status
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/websocket_api/en/#account-order-rate-limits-user_data" /></para>
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitCurrentRateLimit>>>> GetOrderRateLimitsAsync(IEnumerable<string>? symbols = null);

        /// <summary>
        /// Sends a keep alive for the current user stream listen key to keep the stream from closing. Stream auto closes after 60 minutes if no keep alive is send. 30 minute interval for keep alive is recommended.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/websocket_api/en/#ping-user-data-stream-user_stream" /></para>
        /// </summary>
        /// <param name="listenKey"></param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<object>>> KeepAliveUserStreamAsync(string listenKey);
        /// <summary>
        /// Starts a user stream by requesting a listen key. This listen key can be used in a subsequent request to SubscribeToUserDataUpdates. The stream will close after 60 minutes unless a keep alive is send.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/websocket_api/en/#start-user-data-stream-user_stream" /></para>
        /// </summary>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<string>>> StartUserStreamAsync();
        /// <summary>
        /// Stops the current user stream
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/websocket_api/en/#stop-user-data-stream-user_stream" /></para>
        /// </summary>
        /// <param name="listenKey"></param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<object>>> StopUserStreamAsync(string listenKey);

        /// <summary>
        /// Subscribes to the account update stream. Prior to using this, the HitoBitClient.Spot.UserStreams.StartUserStream method should be called.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#user-data-streams" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the StartUserStream method</param>
        /// <param name="onOrderUpdateMessage">The event handler for whenever an order status update is received</param>
        /// <param name="onOcoOrderUpdateMessage">The event handler for whenever an oco order status update is received</param>
        /// <param name="onAccountPositionMessage">The event handler for whenever an account position update is received. Account position updates are a list of changed funds</param>
        /// <param name="onAccountBalanceUpdate">The event handler for whenever a deposit or withdrawal has been processed and the account balance has changed</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(string listenKey, Action<DataEvent<HitoBitStreamOrderUpdate>>? onOrderUpdateMessage, Action<DataEvent<HitoBitStreamOrderList>>? onOcoOrderUpdateMessage, Action<DataEvent<HitoBitStreamPositionsUpdate>>? onAccountPositionMessage, Action<DataEvent<HitoBitStreamBalanceUpdate>>? onAccountBalanceUpdate, CancellationToken ct = default);
    }
}