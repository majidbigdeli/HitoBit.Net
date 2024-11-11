using HitoBit.Net.Enums;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Objects.Sockets;

namespace HitoBit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// HitoBit Spot Exchange Data socket requests and subscriptions
    /// </summary>
    public interface IHitoBitSocketClientSpotApiExchangeData
    {
        /// <summary>
        /// Ping to test connection
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#test-connectivity" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<object>>> PingAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the server time
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#check-server-time" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<DateTime>>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets information about the exchange including rate limits and symbol list
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#exchange-information" /></para>
        /// </summary>
        /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<HitoBitExchangeInfo>>> GetExchangeInfoAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default);

        /// <summary>
        /// Gets compressed, aggregate trades. Trades that fill at the same time, from the same order, with the same price will have the quantity aggregated.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#aggregate-trades" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="fromId">Filter by from trade id</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitStreamAggregatedTrade>>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);
        /// <summary>
        /// Gets the best price/quantity on the order book for a symbol.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#symbol-order-book-ticker" /></para>
        /// </summary>
        /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitBookPrice>>>> GetBookTickersAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default);
        /// <summary>
        /// Gets current average price for a symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#current-average-price" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<HitoBitAveragePrice>>> GetCurrentAvgPriceAsync(string symbol, CancellationToken ct = default);
        /// <summary>
        /// Get candlestick data for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#klines" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitSpotKline>>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);
        /// <summary>
        /// Gets the order book for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#order-book" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="limit">Number of entries</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<HitoBitOrderBook>>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);
        /// <summary>
        /// Gets the recent trades for a symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#recent-trades" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitRecentTradeQuote>>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);
        /// <summary>
        /// Get data based on the last x time, specified as windowSize
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#rolling-window-price-change-statistics" /></para>
        /// </summary>
        /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitRollingWindowTick>>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);
        /// <summary>
        /// Get data regarding the last 24 hours
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#symbol-price-ticker" /></para>
        /// </summary>
        /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBit24HPrice>>>> GetTickersAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default);
        /// <summary>
        /// Gets the historical trades for a symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#historical-trades" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="fromId">Filter by from trade id</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitRecentTradeQuote>>>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default);
        /// <summary>
        /// Get candlestick data for the provided symbol. Returns modified kline data, optimized for the presentation of candlestick charts
        /// <para><a href="https://hitobit-docs.github.io/apidocs/websocket_api/en/#ui-klines" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<HitoBitResponse<IEnumerable<HitoBitSpotKline>>>> GetUIKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the aggregated trades update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#aggregate-trade-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the aggregated trades update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#aggregate-trade-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to mini ticker updates stream for all symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#all-market-mini-tickers-stream" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default);
        
        /// <summary>
        /// Subscribe to rolling window ticker updates stream for all symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#all-market-rolling-window-statistics-streams" /></para>
        /// </summary>
        /// <param name="windowSize">Window size, either 1 hour or 4 hours</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllRollingWindowTickerUpdatesAsync(TimeSpan windowSize, Action<DataEvent<IEnumerable<HitoBitStreamRollingWindowTick>>> onMessage, CancellationToken ct = default);
        
        /// <summary>
        /// Subscribes to ticker updates stream for all symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#all-market-tickers-stream" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitTick>>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to leveraged token info updates
        /// <para><a href="https://hitobit-docs.github.io/apidocs/futures/en/#blvt-info-streams" /></para>
        /// </summary>
        /// <param name="tokens">The tokens to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(IEnumerable<string> tokens, Action<DataEvent<HitoBitBlvtInfoUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to leveraged token info updates
        /// <para><a href="https://hitobit-docs.github.io/apidocs/futures/en/#blvt-info-streams" /></para>
        /// </summary>
        /// <param name="token">The token to subscribe to</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(string token, Action<DataEvent<HitoBitBlvtInfoUpdate>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to leveraged token kline updates
        /// <para><a href="https://hitobit-docs.github.io/apidocs/futures/en/#blvt-nav-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="tokens">The tokens to subscribe to</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(IEnumerable<string> tokens, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to leveraged token kline updates
        /// <para><a href="https://hitobit-docs.github.io/apidocs/futures/en/#blvt-nav-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="token">The token to subscribe to</param>
        /// <param name="interval">The kline interval</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(string token, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to the book ticker update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#individual-symbol-book-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamBookPrice>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to the book ticker update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#individual-symbol-book-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamBookPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbols and intervals
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
        /// <param name="intervals">The intervals of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbol and intervals
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="intervals">The intervals of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to mini ticker updates stream for a list of symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#individual-symbol-mini-ticker-stream" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to mini ticker updates stream for a specific symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#individual-symbol-mini-ticker-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to the depth update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#diff-depth-stream" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
        /// <param name="updateInterval">Update interval in milliseconds</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<IHitoBitEventOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the order book updates for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#diff-depth-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="updateInterval">Update interval in milliseconds</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<IHitoBitEventOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth updates for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#partial-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe on, for example `ETHUSDT`</param>
        /// <param name="levels">The amount of entries to be returned in the update of each symbol, 5, 10 or 20</param>
        /// <param name="updateInterval">Update interval in milliseconds, 1000ms or 100ms</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<DataEvent<IHitoBitOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth updates for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#partial-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe on, for example `ETHUSDT`</param>
        /// <param name="levels">The amount of entries to be returned in the update, 5, 10 or 20</param>
        /// <param name="updateInterval">Update interval in milliseconds, 1000ms or 100ms</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<DataEvent<IHitoBitOrderBook>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribe to rolling window ticker updates stream for a symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#individual-symbol-rolling-window-statistics-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `ETHUSDT`</param>
        /// <param name="windowSize">Window size, either 1 hour or 4 hours</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToRollingWindowTickerUpdatesAsync(string symbol, TimeSpan windowSize, Action<DataEvent<HitoBitStreamRollingWindowTick>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to ticker updates stream for a specific symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#individual-symbol-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to ticker updates stream for a specific symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#individual-symbol-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the trades update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#trade-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default);
        /// <summary>
        /// Subscribes to the trades update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#trade-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default);
    }
}