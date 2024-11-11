using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Objects.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Clients.UsdFuturesApi
{
    internal class HitoBitSocketClientUsdFuturesApiExchangeData : IHitoBitSocketClientUsdFuturesApiExchangeData
    {
        private readonly HitoBitSocketClientUsdFuturesApi _client;
        private readonly ILogger _logger;

        private const string _klineStreamEndpoint = "@kline";
        private const string _continuousContractKlineStreamEndpoint = "@continuousKline";
        private const string _markPriceStreamEndpoint = "@markPrice";
        private const string _allMarkPriceStreamEndpoint = "!markPrice@arr";
        private const string _symbolMiniTickerStreamEndpoint = "@miniTicker";
        private const string _allMiniTickerStreamEndpoint = "!miniTicker@arr";
        private const string _symbolTickerStreamEndpoint = "@ticker";
        private const string _allTickerStreamEndpoint = "!ticker@arr";
        private const string _compositeIndexEndpoint = "@compositeIndex";

        private const string _aggregatedTradesStreamEndpoint = "@aggTrade";
        private const string _tradesStreamEndpoint = "@trade";
        private const string _bookTickerStreamEndpoint = "@bookTicker";
        private const string _allBookTickerStreamEndpoint = "!bookTicker";
        private const string _liquidationStreamEndpoint = "@forceOrder";
        private const string _allLiquidationStreamEndpoint = "!forceOrder@arr";
        private const string _partialBookDepthStreamEndpoint = "@depth";
        private const string _depthStreamEndpoint = "@depth";

        internal HitoBitSocketClientUsdFuturesApiExchangeData(ILogger logger, HitoBitSocketClientUsdFuturesApi client)
        {
            _client = client;
            _logger = logger;
        }

        #region Queries

        #region Get Orderbook

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitFuturesOrderBook>>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            parameters.AddOptionalParameter("limit", limit);
            int weight = limit <= 50 ? 2 : limit <= 100 ? 5 : limit <= 500 ? 10 : 20;
            return await _client.QueryAsync<HitoBitFuturesOrderBook>(_client.ClientOptions.Environment.UsdFuturesSocketApiAddress!.AppendPath("ws-fapi/v1"), $"depth", parameters, weight: weight, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Price

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitPrice>>> GetPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            return await _client.QueryAsync<HitoBitPrice>(_client.ClientOptions.Environment.UsdFuturesSocketApiAddress!.AppendPath("ws-fapi/v1"), $"ticker.price", parameters, weight: 1, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Price

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitPrice>>>> GetPricesAsync(CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            return await _client.QueryAsync<IEnumerable<HitoBitPrice>>(_client.ClientOptions.Environment.UsdFuturesSocketApiAddress!.AppendPath("ws-fapi/v1"), $"ticker.price", parameters, weight: 2, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Price

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitBookPrice>>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            return await _client.QueryAsync<HitoBitBookPrice>(_client.ClientOptions.Environment.UsdFuturesSocketApiAddress!.AppendPath("ws-fapi/v1"), $"ticker.book", parameters, weight: 2, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Price

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitBookPrice>>>> GetBookPricesAsync(CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            return await _client.QueryAsync<IEnumerable<HitoBitBookPrice>>(_client.ClientOptions.Environment.UsdFuturesSocketApiAddress!.AppendPath("ws-fapi/v1"), $"ticker.book", parameters, weight: 5, ct: ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Streams

        #region Mark Price Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<HitoBitFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<HitoBitFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesUsdtStreamMarkPrice>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _markPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Mark Price Stream for All market

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(int? updateInterval, Action<DataEvent<IEnumerable<HitoBitFuturesUsdtStreamMarkPrice>>> onMessage, CancellationToken ct = default)
        {
            updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitFuturesUsdtStreamMarkPrice>>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { _allMarkPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "") }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) =>
            await SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamKlineData>>>(data => onMessage(data.As<IHitoBitStreamKlineData>(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.SelectMany(a => intervals.Select(i => a.ToLower(CultureInfo.InvariantCulture) + _klineStreamEndpoint + "_" + EnumConverter.GetString(i))).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Continuous contract kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamContinuousKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToContinuousContractKlineUpdatesAsync(new[] { pair }, contractType, interval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamContinuousKlineData>> onMessage, CancellationToken ct = default)
        {
            pairs.ValidateNotNull(nameof(pairs));
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamContinuousKlineData>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                      "_" +
                                      EnumConverter.GetString(contractType).ToLower() +
                                      _continuousContractKlineStreamEndpoint +
                                      "_" +
                                      EnumConverter.GetString(interval)).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, pairs, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Mini Ticker Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default) => await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamMiniTick>>>(data => onMessage(data.As<IHitoBitMiniTick>(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _symbolMiniTickerStreamEndpoint).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Mini Tickers Stream
        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBitMiniTick>>(data.Data.Data).WithStreamId(data.Data.Stream)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { _allMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }
        #endregion

        #region Individual Symbol Ticker Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamTick>>>(data => onMessage(data.As<IHitoBit24HPrice>(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _symbolTickerStreamEndpoint).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Composite index Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToCompositeIndexUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
        {
            var action = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamCompositeIndex>>>(data =>
            {
                onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol));
            });
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { symbol.ToLower(CultureInfo.InvariantCulture) + _compositeIndexEndpoint }, action, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Tickers Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBit24HPrice>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBit24HPrice>>(data.Data.Data).WithStreamId(data.Data.Stream)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { _allTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
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
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }
        #endregion

        #region Trade Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, bool filterOutNonTradeUpdates = true, CancellationToken ct = default) =>
            await SubscribeToTradeUpdatesAsync(new[] { symbol }, onMessage, filterOutNonTradeUpdates, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols,
            Action<DataEvent<HitoBitStreamTrade>> onMessage, bool filterOutNonTradeUpdates = true, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamTrade>>>(data =>
            {
                if (filterOutNonTradeUpdates && data.Data.Data.Type != "MARKET")
                    return;

                onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol));
            });
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + _tradesStreamEndpoint).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Book Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { _allBookTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
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
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Liquidation Order Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Data.Symbol)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { _allLiquidationStreamEndpoint }, handler, ct).ConfigureAwait(false);
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
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Contract Info Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamSymbolUpdate>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { "!contractInfo" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Asset Index Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAssetIndexUpdatesAsync(Action<DataEvent<IEnumerable<HitoBitFuturesStreamAssetIndexUpdate>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitFuturesStreamAssetIndexUpdate>>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { "!assetIndex@arr" }, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAssetIndexUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamAssetIndexUpdate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamAssetIndexUpdate>>>(data => onMessage(data.As(data.Data.Data).WithStreamId(data.Data.Stream).WithSymbol(data.Data.Data.Symbol)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { symbol.ToLowerInvariant() + "@assetIndex" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

    }
}
