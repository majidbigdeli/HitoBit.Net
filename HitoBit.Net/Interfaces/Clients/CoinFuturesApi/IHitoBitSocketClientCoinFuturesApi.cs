﻿using HitoBit.Net.Enums;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Objects.Sockets;

namespace HitoBit.Net.Interfaces.Clients.CoinFuturesApi
{
    /// <summary>
    /// HitoBit Coin futures streams
    /// </summary>
    public interface IHitoBitSocketClientCoinFuturesApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        IHitoBitSocketClientCoinFuturesApiShared SharedClient { get; }

        /// <summary>
        /// Subscribes to the aggregated trades update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#aggregate-trade-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the aggregated trades update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#aggregate-trade-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to individual trade update. NOTE: This endpoint stream isn't document and therefor might be changed or removed without prior notice
        /// </summary>
        /// <param name="symbol">Symbol to subscribe, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to individual trade update. NOTE: This endpoint stream isn't document and therefor might be changed or removed without prior notice
        /// </summary>
        /// <param name="symbols">Symbols to subscribe, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbol and intervals
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="intervals">The intervals of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the candlestick update stream for the provided symbols and intervals
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="intervals">The intervals of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to mini ticker updates stream for a specific symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#individual-symbol-mini-ticker-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to mini ticker updates stream for a list of symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#individual-symbol-mini-ticker-stream" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to mini ticker updates stream for all symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#all-market-mini-tickers-stream" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to ticker updates stream for a specific symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#individual-symbol-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to ticker updates stream for a specific symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#individual-symbol-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe to, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to ticker updates stream for all symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#all-market-tickers-streams" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBit24HPrice>>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to all book ticker update streams
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#all-book-tickers-stream" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the book ticker update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#individual-symbol-book-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the book ticker update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#individual-symbol-book-ticker-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to specific symbol forced liquidations stream
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#liquidation-order-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to list of symbol forced liquidations stream
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#liquidation-order-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to all forced liquidations stream
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#all-market-liquidation-order-streams" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth updates for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#partial-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe on, for example `BTCUSD_PERP`</param>
        /// <param name="levels">The amount of entries to be returned in the update</param>
        /// <param name="updateInterval">Update interval in milliseconds</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth updates for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#partial-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe on, for example `BTCUSD_PERP`</param>
        /// <param name="levels">The amount of entries to be returned in the update of each symbol</param>
        /// <param name="updateInterval">Update interval in milliseconds, either 100 or 500. Defaults to 250</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the order book updates for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#diff-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="updateInterval">Update interval in milliseconds, either 0 or 100, 500 or 1000, depending on endpoint</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the depth update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#diff-book-depth-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="updateInterval">Update interval in milliseconds, either 0 or 100, 500 or 1000, depending on endpoint</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the account update stream. Prior to using this, the <see cref="IHitoBitRestClientCoinFuturesApiAccount.StartUserStreamAsync(CancellationToken)">restClient.CoinFuturesApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#user-data-streams" /></para>
        /// </summary>
        /// <param name="listenKey">Listen key retrieved by the <see cref="IHitoBitRestClientCoinFuturesApiAccount.StartUserStreamAsync(CancellationToken)">restClient.CoinFuturesApi.Account.StartUserStreamAsync</see> method</param>
        /// <param name="onLeverageUpdate">The event handler for leverage changed update</param>
        /// <param name="onMarginUpdate">The event handler for whenever a margin has changed</param>
        /// <param name="onAccountUpdate">The event handler for whenever an account update is received</param>
        /// <param name="onOrderUpdate">The event handler for whenever an order status update is received</param>
        /// <param name="onListenKeyExpired">Responds when the listen key for the stream has expired. Initiate a new instance of the stream here</param>
        /// <param name="onStrategyUpdate">The event handler for whenever a strategy update is received</param>
        /// <param name="onGridUpdate">The event handler for whenever a grid update is received</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitFuturesStreamConfigUpdate>>? onLeverageUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamMarginUpdate>>? onMarginUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<HitoBitStreamEvent>>? onListenKeyExpired = null,
            Action<DataEvent<HitoBitStrategyUpdate>>? onStrategyUpdate = null,
            Action<DataEvent<HitoBitGridUpdate>>? onGridUpdate = null,
            CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the Index price update stream for a single pair
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#index-price-stream" /></para>
        /// </summary>
        /// <param name="pair">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(string pair, int? updateInterval, Action<DataEvent<HitoBitFuturesStreamIndexPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the Index price update stream for a list of pairs
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#index-price-stream" /></para>
        /// </summary>
        /// <param name="pairs">The pairs, for example `BTCUSD`</param>
        /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(IEnumerable<string> pairs, int? updateInterval, Action<DataEvent<HitoBitFuturesStreamIndexPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the Mark price update stream for a single symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#mark-price-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<HitoBitFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        ///Subscribe to the Mark price update stream for all symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#mark-price-of-all-symbols-of-a-pair" /></para>
        /// </summary>
        /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(Action<DataEvent<IEnumerable<HitoBitFuturesCoinStreamMarkPrice>>> onMessage, int? updateInterval = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the Mark price update stream for a list of symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#mark-price-stream" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="updateInterval">Update interval in milliseconds, either 1000 or 3000. Defaults to 3000</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<HitoBitFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the continuous contract candlestick update stream for the provided pair
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#continuous-contract-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="pair">The pair, for example `BTCUSD`</param>
        /// <param name="contractType">The contract type</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the continuous contract candlestick update stream for the provided pairs
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#continuous-contract-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="pairs">The pairs, for example `BTCUSD`</param>
        /// <param name="contractType">The contract type</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the index candlestick update stream for the provided pair
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#index-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="pair">The pair, for example `BTCUSD`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(string pair, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the index candlestick update stream for the provided pairs
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#index-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="pairs">The pairs, for example `BTCUSD`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(IEnumerable<string> pairs, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the mark price candlestick update stream for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#mark-price-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribes to the mark price candlestick update stream for the provided symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#mark-price-kline-candlestick-streams" /></para>
        /// </summary>
        /// <param name="symbols">The symbols, for example `BTCUSD_PERP`</param>
        /// <param name="interval">The interval of the candlesticks</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to contract/symbol updates
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default);
    }
}
