using HitoBit.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Objects.Models.Spot.Socket;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using CryptoExchange.Net.Interfaces;

namespace HitoBit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// HitoBit Spot streams
    /// </summary>
    public interface IHitoBitSocketClientSpotStreams : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribes to the aggregated trades update stream for the provided symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#aggregate-trade-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the aggregated trades update stream for the provided symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#aggregate-trade-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbol and intervals
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="intervals">The intervals of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbols and intervals
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="intervals">The intervals of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to mini ticker updates stream for a specific symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#individual-symbol-mini-ticker-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to mini ticker updates stream for a list of symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#individual-symbol-mini-ticker-stream" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to mini ticker updates stream for all symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#all-market-mini-tickers-stream" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to rolling window ticker updates stream for a symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#individual-symbol-rolling-window-statistics-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe</param>
        /// <param name="windowSize">Window size, either 1 hour or 4 hours</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToRollingWindowTickerUpdatesAsync(string symbol, TimeSpan windowSize,
            Action<DataEvent<HitoBitStreamRollingWindowTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to rolling window ticker updates stream for all symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#all-market-rolling-window-statistics-streams" /></para>
        /// </summary>
        /// <param name="windowSize">Window size, either 1 hour or 4 hours</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllRollingWindowTickerUpdatesAsync(TimeSpan windowSize,
            Action<DataEvent<IEnumerable<HitoBitStreamRollingWindowTick>>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to ticker updates stream for a specific symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#individual-symbol-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to ticker updates stream for a specific symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#individual-symbol-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to ticker updates stream for all symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#all-market-tickers-stream" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitTick>>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the book ticker update stream for the provided symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#individual-symbol-book-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamBookPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the book ticker update stream for the provided symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#individual-symbol-book-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamBookPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the trades update stream for the provided symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#trade-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default);


        /// <summary>
        /// Subscribes to the trades update stream for the provided symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#trade-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth updates for the provided symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#partial-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe on</param>
        /// <param name="levels">The amount of entries to be returned in the update</param>
        /// <param name="updateInterval">Update interval in milliseconds</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<DataEvent<IHitoBitOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth updates for the provided symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#partial-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe on</param>
        /// <param name="levels">The amount of entries to be returned in the update of each symbol</param>
        /// <param name="updateInterval">Update interval in milliseconds</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<DataEvent<IHitoBitOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the order book updates for the provided symbol
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#diff-depth-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="updateInterval">Update interval in milliseconds</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<IHitoBitEventOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth update stream for the provided symbols
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#diff-depth-stream" /></para>
        /// </summary>
        /// <param name="symbols">The symbols</param>
        /// <param name="updateInterval">Update interval in milliseconds</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<IHitoBitEventOrderBook>> onMessage, CancellationToken ct = default);

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
        Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitStreamOrderUpdate>>? onOrderUpdateMessage,
            Action<DataEvent<HitoBitStreamOrderList>>? onOcoOrderUpdateMessage,
            Action<DataEvent<HitoBitStreamPositionsUpdate>>? onAccountPositionMessage,
            Action<DataEvent<HitoBitStreamBalanceUpdate>>? onAccountBalanceUpdate,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribes to leveraged token info updates
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/futures/en/#blvt-info-streams" /></para>
        /// </summary>
        /// <param name="token">The token to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(string token,
            Action<DataEvent<HitoBitBlvtInfoUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to leveraged token info updates
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/futures/en/#blvt-info-streams" /></para>
        /// </summary>
        /// <param name="tokens">The tokens to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(IEnumerable<string> tokens, Action<DataEvent<HitoBitBlvtInfoUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to leveraged token kline updates
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/futures/en/#blvt-nav-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="token">The token to subscribe to</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(string token,
            KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to leveraged token kline updates
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/futures/en/#blvt-nav-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="tokens">The tokens to subscribe to</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(IEnumerable<string> tokens, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default);
    }
}
