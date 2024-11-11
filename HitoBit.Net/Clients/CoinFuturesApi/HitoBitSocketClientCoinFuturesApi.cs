using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.CoinFuturesApi;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Models.Spot.Socket;
using HitoBit.Net.Objects.Options;
using HitoBit.Net.Objects.Sockets;
using HitoBit.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.Clients.CoinFuturesApi
{
    /// <inheritdoc cref="IHitoBitSocketClientCoinFuturesApi" />
    internal partial class HitoBitSocketClientCoinFuturesApi : SocketApiClient, IHitoBitSocketClientCoinFuturesApi
    {
        #region fields
        private const string _klineStreamEndpoint = "@kline";
        private const string _markPriceStreamEndpoint = "@markPrice";
        private const string _allMarkPriceStreamEndpoint = "!markPrice@arr";
        private const string _indexPriceStreamEndpoint = "@indexPrice";
        private const string _continuousKlineStreamEndpoint = "@continuousKline";
        private const string _indexKlineStreamEndpoint = "@indexPriceKline";
        private const string _markKlineStreamEndpoint = "@markPriceKline";
        private const string _symbolMiniTickerStreamEndpoint = "@miniTicker";
        private const string _allMiniTickerStreamEndpoint = "!miniTicker@arr";
        private const string _symbolTickerStreamEndpoint = "@ticker";
        private const string _allTickerStreamEndpoint = "!ticker@arr";

        private const string _aggregatedTradesStreamEndpoint = "@aggTrade";
        private const string _tradesStreamEndpoint = "@trade";
        private const string _bookTickerStreamEndpoint = "@bookTicker";
        private const string _allBookTickerStreamEndpoint = "!bookTicker";
        private const string _liquidationStreamEndpoint = "@forceOrder";
        private const string _allLiquidationStreamEndpoint = "!forceOrder@arr";
        private const string _partialBookDepthStreamEndpoint = "@depth";
        private const string _depthStreamEndpoint = "@depth";

        private static readonly MessagePath _idPath = MessagePath.Get().Property("id");
        private static readonly MessagePath _streamPath = MessagePath.Get().Property("stream");
        #endregion

        #region constructor/destructor

        internal HitoBitSocketClientCoinFuturesApi(ILogger logger, HitoBitSocketOptions options) :
            base(logger, options.Environment.CoinFuturesSocketAddress!, options, options.CoinFuturesOptions)
        {
            // When sending more than 4000 bytes the server responds very delayed (somehow connected to the websocket keep alive interval)
            // See https://dev.hitobit.vision/t/socket-live-subscribing-server-delay/9645/2
            // To prevent issues we keep below this
            MessageSendSizeLimit = 4000;

            RateLimiter = HitoBitExchange.RateLimiter.FuturesSocket;
        }
        #endregion 

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider(credentials);

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        protected override IByteMessageAccessor CreateAccessor() => new SystemTextJsonByteMessageAccessor();
        public IHitoBitSocketClientCoinFuturesApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => HitoBitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        #region methods

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var id = message.GetValue<int?>(_idPath);
            if (id != null)
                return id.ToString();

            return message.GetValue<string>(_streamPath);
        }

        #region Kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, new[] { interval }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default)
            => await SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamCoinKlineData>>>(data =>
            {
                var result = data.Data.Data;
                onMessage(data.As<IHitoBitStreamKlineData>(result).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol));
        });
            symbols = symbols.SelectMany(a => intervals.Select(i => a.ToLower(CultureInfo.InvariantCulture) + _klineStreamEndpoint + "_" + EnumConverter.GetString(i))).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Index Price Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(string pair, int? updateInterval, Action<DataEvent<HitoBitFuturesStreamIndexPrice>> onMessage, CancellationToken ct = default) => await SubscribeToIndexPriceUpdatesAsync(new[] { pair }, updateInterval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(IEnumerable<string> pairs, int? updateInterval, Action<DataEvent<HitoBitFuturesStreamIndexPrice>> onMessage, CancellationToken ct = default)
        {
            pairs.ValidateNotNull(nameof(pairs));
            updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

            var internalHandler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamIndexPrice>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Pair)));
            pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _indexPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
            return await SubscribeAsync( BaseAddress, pairs, internalHandler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Mark Price Stream
        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<HitoBitFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<HitoBitFuturesCoinStreamMarkPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

            var internalHandler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesCoinStreamMarkPrice>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _markPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, internalHandler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Continuous contract kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToContinuousContractKlineUpdatesAsync(new[] { pair }, contractType, interval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default)
        {
            pairs.ValidateNotNull(nameof(pairs));
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamKlineData>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                      "_" +
                                      EnumConverter.GetString(contractType).ToLower() +
                                      _continuousKlineStreamEndpoint +
                                      "_" +
                                      EnumConverter.GetString(interval)).ToArray();
            return await SubscribeAsync( BaseAddress, pairs, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Index kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(string pair, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToIndexKlineUpdatesAsync(new[] { pair }, interval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(IEnumerable<string> pairs, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default)
        {
            pairs.ValidateNotNull(nameof(pairs));
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamIndexKlineData>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                      _indexKlineStreamEndpoint +
                                      "_" +
                                      EnumConverter.GetString(interval)).ToArray();
            return await SubscribeAsync( BaseAddress, pairs, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Mark price kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceKlineUpdatesAsync(new[] { symbol }, interval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<HitoBitStreamIndexKlineData>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamIndexKlineData>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                          _markKlineStreamEndpoint +
                                         "_" +
                                         EnumConverter.GetString(interval)).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Mini Ticker Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default) => await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamCoinMiniTick>>>(data => onMessage(data.As<IHitoBitMiniTick>(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _symbolMiniTickerStreamEndpoint).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Mini Tickers Stream
        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamCoinMiniTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBitMiniTick>>(data.Data.Data).WithStreamId(data.Data.Stream)));
            return await SubscribeAsync( BaseAddress, new[] { _allMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }
        #endregion

        #region Individual Symbol Ticker Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamCoinTick>>>(data => onMessage(data.As<IHitoBit24HPrice>(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _symbolTickerStreamEndpoint).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Tickers Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBit24HPrice>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamCoinTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBit24HPrice>>(data.Data.Data).WithStreamId(data.Data.Stream)));
            return await SubscribeAsync( BaseAddress, new[] { _allTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Aggregate Trade Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default) => await SubscribeToAggregatedTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamAggregatedTrade>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _aggregatedTradesStreamEndpoint).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }
        #endregion

        #region Trade Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default) =>
            await SubscribeToTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamTrade>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _tradesStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Mark Price Stream for All market

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(Action<DataEvent<IEnumerable<HitoBitFuturesCoinStreamMarkPrice>>> onMessage, int? updateInterval = null, CancellationToken ct = default)
        {
            updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitFuturesCoinStreamMarkPrice>>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream)));
            return await SubscribeAsync(BaseAddress, new[] { _allMarkPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "") }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Book Ticker Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default) => await SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _bookTickerStreamEndpoint).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Book Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            return await SubscribeAsync( BaseAddress, new[] { _allBookTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Liquidation Order Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default) => await SubscribeToLiquidationUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _liquidationStreamEndpoint).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Liquidation Order Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Data.Symbol)));
            return await SubscribeAsync( BaseAddress, new[] { _allLiquidationStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Partial Book Depth Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => await SubscribeToPartialOrderBookUpdatesAsync(new[] { symbol }, levels, updateInterval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            levels.ValidateIntValues(nameof(levels), 5, 10, 20);
            updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamOrderBookDepth>>>(data =>
            {
                data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
                onMessage(data.As<IHitoBitFuturesEventOrderBook>(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol));
        });

            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _partialBookDepthStreamEndpoint + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Diff. Book Depth Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => await SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<IHitoBitFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamOrderBookDepth>>>(data => onMessage(data.As<IHitoBitFuturesEventOrderBook>(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _depthStreamEndpoint + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await SubscribeAsync( BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Contract Info Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamSymbolUpdate>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            return await SubscribeAsync(BaseAddress, new[] { "!contractInfo" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region User Data Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitFuturesStreamConfigUpdate>>? onConfigUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamMarginUpdate>>? onMarginUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamAccountUpdate>>? onAccountUpdate = null,
            Action<DataEvent<HitoBitFuturesStreamOrderUpdate>>? onOrderUpdate = null,
            Action<DataEvent<HitoBitStreamEvent>>? onListenKeyExpired = null,
            Action<DataEvent<HitoBitStrategyUpdate>>? onStrategyUpdate = null,
            Action<DataEvent<HitoBitGridUpdate>>? onGridUpdate = null,
            CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var subscription = new HitoBitCoinFuturesUserDataSubscription(_logger, new List<string> { listenKey }, onOrderUpdate, onConfigUpdate, onMarginUpdate, onAccountUpdate, onListenKeyExpired, onStrategyUpdate, onGridUpdate);
            return await SubscribeAsync(BaseAddress.AppendPath("stream"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion
        internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<DataEvent<T>> onData, CancellationToken ct)
        {
            var request = new HitoBitSocketRequest
            {
                Method = "SUBSCRIBE",
                Params = topics.ToArray(),
                Id = ExchangeHelpers.NextId()
            };

            var subscription = new HitoBitSubscription<T>(_logger, topics.ToList(), onData, false);
            return SubscribeAsync(url.AppendPath("stream"), subscription, ct);
        }

        /// <inheritdoc />
        protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection) => Task.FromResult<Query?>(null);
    }
}
