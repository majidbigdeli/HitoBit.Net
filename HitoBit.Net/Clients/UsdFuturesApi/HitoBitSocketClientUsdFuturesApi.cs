using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using HitoBit.Net.Objects.Models.Spot.Socket;
using HitoBit.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HitoBit.Net.Clients.UsdFuturesApi
{
    /// <summary>
    /// Client providing access to the HitoBit Usd futures websocket Api
    /// </summary>
    public class HitoBitSocketClientUsdFuturesApi : SocketApiClient, IHitoBitSocketClientUsdFuturesApi
    {
        #region fields
        private const string klineStreamEndpoint = "@kline";
        private const string continuousContractKlineStreamEndpoint = "@continuousKline";
        private const string markPriceStreamEndpoint = "@markPrice";
        private const string allMarkPriceStreamEndpoint = "!markPrice@arr";
        private const string symbolMiniTickerStreamEndpoint = "@miniTicker";    
        private const string allMiniTickerStreamEndpoint = "!miniTicker@arr";
        private const string symbolTickerStreamEndpoint = "@ticker";
        private const string allTickerStreamEndpoint = "!ticker@arr";
        private const string compositeIndexEndpoint = "@compositeIndex";

        private const string aggregatedTradesStreamEndpoint = "@aggTrade";
        private const string tradesStreamEndpoint = "@trade";
        private const string bookTickerStreamEndpoint = "@bookTicker";
        private const string allBookTickerStreamEndpoint = "!bookTicker";
        private const string liquidationStreamEndpoint = "@forceOrder";
        private const string allLiquidationStreamEndpoint = "!forceOrder@arr";
        private const string partialBookDepthStreamEndpoint = "@depth";
        private const string depthStreamEndpoint = "@depth";

        private const string configUpdateEvent = "ACCOUNT_CONFIG_UPDATE";
        private const string marginUpdateEvent = "MARGIN_CALL";
        private const string accountUpdateEvent = "ACCOUNT_UPDATE";
        private const string orderUpdateEvent = "ORDER_TRADE_UPDATE";
        private const string listenKeyExpiredEvent = "listenKeyExpired";
        private const string strategyUpdateEvent = "STRATEGY_UPDATE";
        private const string gridUpdateEvent = "GRID_UPDATE";
        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of HitoBitSocketClientUsdFuturesStreams
        /// </summary>
        internal HitoBitSocketClientUsdFuturesApi(ILogger logger, HitoBitSocketOptions options) :
            base(logger, options.Environment.UsdFuturesSocketAddress!, options, options.UsdFuturesOptions)
        {
            SetDataInterpreter((data) => string.Empty, null);
            RateLimitPerSocketPerSecond = 4;
        }
        #endregion 

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider(credentials);


        #region methods

        #region Mark Price Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<DataEvent<HitoBitFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<DataEvent<HitoBitFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesUsdtStreamMarkPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + markPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Mark Price Stream for All market

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(int? updateInterval, Action<DataEvent<IEnumerable<HitoBitFuturesUsdtStreamMarkPrice>>> onMessage, CancellationToken ct = default)
        {
            updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitFuturesUsdtStreamMarkPrice>>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
            return await SubscribeAsync(BaseAddress, new[] { allMarkPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "") }, handler, ct).ConfigureAwait(false);
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
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamKlineData>>>(data => onMessage(data.As<IHitoBitStreamKlineData>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.SelectMany(a => intervals.Select(i => a.ToLower(CultureInfo.InvariantCulture) + klineStreamEndpoint + "_" + JsonConvert.SerializeObject(i, new KlineIntervalConverter(false)))).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Continuous contract kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamContinuousKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToContinuousContractKlineUpdatesAsync(new[] { pair }, contractType, interval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, ContractType contractType, KlineInterval interval, Action<DataEvent<HitoBitStreamContinuousKlineData>> onMessage, CancellationToken ct = default)
        {
            pairs.ValidateNotNull(nameof(pairs));
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamContinuousKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                      "_" +
                                      JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)).ToLower() +
                                      continuousContractKlineStreamEndpoint +
                                      "_" +
                                      JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))).ToArray();
            return await SubscribeAsync(BaseAddress, pairs, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Mini Ticker Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default) => await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamMiniTick>>>(data => onMessage(data.As<IHitoBitMiniTick>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolMiniTickerStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Mini Tickers Stream
        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBitMiniTick>>(data.Data.Data, data.Data.Stream)));
            return await SubscribeAsync(BaseAddress, new[] { allMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }
        #endregion

        #region Individual Symbol Ticker Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBit24HPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamTick>>>(data => onMessage(data.As<IHitoBit24HPrice>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolTickerStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Composite index Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToCompositeIndexUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
        {
            var action = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamCompositeIndex>>>(data =>
            {
                onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
            });
            return await SubscribeAsync(BaseAddress, new[] { symbol + compositeIndexEndpoint }, action, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Tickers Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBit24HPrice>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBit24HPrice>>(data.Data.Data, data.Data.Stream)));
            return await SubscribeAsync(BaseAddress, new[] { allTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Aggregate Trade Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default) => await SubscribeToAggregatedTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamAggregatedTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + aggregatedTradesStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
            foreach (var symbol in symbols)
                symbol.ValidateHitoBitSymbol();

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + tradesStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Book Ticker Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default) => await SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + bookTickerStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Book Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            return await SubscribeAsync(BaseAddress, new[] { allBookTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Liquidation Order Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default) => await SubscribeToLiquidationUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data, data.Data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + liquidationStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Liquidation Order Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data, data.Data.Data.Data.Symbol)));
            return await SubscribeAsync(BaseAddress, new[] { allLiquidationStreamEndpoint }, handler, ct).ConfigureAwait(false);
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
                onMessage(data.As<IHitoBitFuturesEventOrderBook>(data.Data.Data, data.Data.Data.Symbol));
            });

            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + partialBookDepthStreamEndpoint + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamOrderBookDepth>>>(data => onMessage(data.As<IHitoBitFuturesEventOrderBook>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + depthStreamEndpoint + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Contract Info Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSymbolUpdatesAsync(Action<DataEvent<HitoBitFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitFuturesStreamSymbolUpdate>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            return await SubscribeAsync(BaseAddress, new[] { "!contractInfo" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region User Data Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitFuturesStreamConfigUpdate>>? onConfigUpdate,
            Action<DataEvent<HitoBitFuturesStreamMarginUpdate>>? onMarginUpdate,
            Action<DataEvent<HitoBitFuturesStreamAccountUpdate>>? onAccountUpdate,
            Action<DataEvent<HitoBitFuturesStreamOrderUpdate>>? onOrderUpdate,
            Action<DataEvent<HitoBitStreamEvent>>? onListenKeyExpired,
            Action<DataEvent<HitoBitStrategyUpdate>>? onStrategyUpdate,
            Action<DataEvent<HitoBitGridUpdate>>? onGridUpdate,
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
                    case configUpdateEvent:
                        {
                            var result = Deserialize<HitoBitFuturesStreamConfigUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onConfigUpdate?.Invoke(data.As(result.Data, result.Data.LeverageUpdateData?.Symbol));
                            }
                            else
                                _logger.Log(LogLevel.Warning, "Couldn't deserialize data received from config stream: " + result.Error);

                            break;
                        }
                    case marginUpdateEvent:
                        {
                            var result = Deserialize<HitoBitFuturesStreamMarginUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onMarginUpdate?.Invoke(data.As(result.Data));
                            }
                            else
                                _logger.Log(LogLevel.Warning, "Couldn't deserialize data received from order stream: " + result.Error);
                            break;
                        }
                    case accountUpdateEvent:
                        {
                            var result = Deserialize<HitoBitFuturesStreamAccountUpdate>(token);
                            if (result.Success)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onAccountUpdate?.Invoke(data.As(result.Data));
                            }
                            else
                                _logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account stream: " + result.Error);

                            break;
                        }
                    case orderUpdateEvent:
                        {
                            var result = Deserialize<HitoBitFuturesStreamOrderUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onOrderUpdate?.Invoke(data.As(result.Data, result.Data.UpdateData.Symbol));
                            }
                            else
                                _logger.Log(LogLevel.Warning, "Couldn't deserialize data received from order stream: " + result.Error);
                            break;
                        }
                    case listenKeyExpiredEvent:
                        {
                            var result = Deserialize<HitoBitStreamEvent>(token);
                            if (result)
                                onListenKeyExpired?.Invoke(data.As(result.Data, combinedToken["stream"]!.Value<string>()));                            
                            else
                                _logger.Log(LogLevel.Warning, "Couldn't deserialize data received from the expired listen key event: " + result.Error);
                            break;
                        }
                    case strategyUpdateEvent:
                        {
                            var result = Deserialize<HitoBitStrategyUpdate>(token);
                            if (result)
                                onStrategyUpdate?.Invoke(data.As(result.Data, combinedToken["stream"]!.Value<string>()));
                            else
                                _logger.Log(LogLevel.Warning, "Couldn't deserialize data received from the StrategyUpdate event: " + result.Error);
                            break;
                        }
                    case gridUpdateEvent:
                        {
                            var result = Deserialize<HitoBitGridUpdate>(token);
                            if (result)
                                onGridUpdate?.Invoke(data.As(result.Data, combinedToken["stream"]!.Value<string>()));
                            else
                                _logger.Log(LogLevel.Warning, "Couldn't deserialize data received from the GridUpdate event: " + result.Error);
                            break;
                        }
                    default:
                        _logger.Log(LogLevel.Warning, $"Received unknown user data event {evnt}: " + data.Data);
                        break;
                }
            });

            return await SubscribeAsync(BaseAddress, new[] { listenKey }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

        internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<DataEvent<T>> onData, CancellationToken ct)
        {
            var request = new HitoBitSocketRequest
            {
                Method = "SUBSCRIBE",
                Params = topics.ToArray(),
                Id = NextId()
            };

            return SubscribeAsync(url.AppendPath("stream"), request, null, false, onData, ct);
        }

        /// <inheritdoc />
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
        {
            callResult = null;
            if (message.Type != JTokenType.Object)
                return false;

            var id = message["id"];
            if (id == null)
                return false;

            var bRequest = (HitoBitSocketRequest)request;
            if ((int)id != bRequest.Id)
                return false;

            var result = message["result"];
            if (result != null && result.Type == JTokenType.Null)
            {
                _logger.Log(LogLevel.Trace, $"Socket {s.SocketId} Subscription completed");
                callResult = new CallResult<object>(new object());
                return true;
            }

            var error = message["error"];
            if (error == null)
            {
                callResult = new CallResult<object>(new ServerError("Unknown error: " + message));
                return true;
            }

            callResult = new CallResult<object>(new ServerError(error["code"]!.Value<int>(), error["msg"]!.ToString()));
            return true;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            if (message.Type != JTokenType.Object)
                return false;

            var bRequest = (HitoBitSocketRequest)request;
            var stream = message["stream"];
            if (stream == null)
                return false;

            return bRequest.Params.Contains(stream.ToString());
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            return true;
        }

        /// <inheritdoc />
        protected override Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscription)
        {
            var topics = ((HitoBitSocketRequest)subscription.Request!).Params;
            var unsub = new HitoBitSocketRequest { Method = "UNSUBSCRIBE", Params = topics, Id = NextId() };
            var result = false;

            if (!connection.Connected)
                return true;

            await connection.SendAndWaitAsync(unsub, ClientOptions.RequestTimeout, null, data =>
            {
                if (data.Type != JTokenType.Object)
                    return false;

                var id = data["id"];
                if (id == null)
                    return false;

                if ((int)id != unsub.Id)
                    return false;

                var result = data["result"];
                if (result?.Type == JTokenType.Null)
                {
                    result = true;
                    return true;
                }

                return true;
            }).ConfigureAwait(false);
            return result;
        }
    }
}
