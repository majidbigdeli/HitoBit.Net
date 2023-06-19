using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoExchange.Net.Interfaces;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class HitoBitSocketClientSpotStreams : SocketApiClient, IHitoBitSocketClientSpotStreams
    {
        #region fields
        private readonly HitoBitSocketClientOptions _options;

        private const string depthStreamEndpoint = "@depth";
        private const string bookTickerStreamEndpoint = "@bookTicker";
        private const string allBookTickerStreamEndpoint = "!bookTicker";
        private const string klineStreamEndpoint = "@kline";
        private const string tradesStreamEndpoint = "@trade";
        private const string aggregatedTradesStreamEndpoint = "@aggTrade";
        private const string symbolTickerStreamEndpoint = "@ticker";
        private const string allSymbolTickerStreamEndpoint = "!ticker@arr";
        private const string partialBookDepthStreamEndpoint = "@depth";
        private const string symbolMiniTickerStreamEndpoint = "@miniTicker";
        private const string allSymbolMiniTickerStreamEndpoint = "!miniTicker@arr";

        private const string bltvInfoEndpoint = "@tokenNav";
        private const string bltvKlineEndpoint = "@nav_kline";

        private const string executionUpdateEvent = "executionReport";
        private const string ocoOrderUpdateEvent = "listStatus";
        private const string accountPositionUpdateEvent = "outboundAccountPosition";
        private const string balanceUpdateEvent = "balanceUpdate";
        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of HitoBitSocketClientSpot with default options
        /// </summary>
        public HitoBitSocketClientSpotStreams(Log log, HitoBitSocketClientOptions options) :
            base(log, options, options.SpotStreamsOptions)
        {
            _options = options;

            SetDataInterpreter((data) => string.Empty, null);
            RateLimitPerSocketPerSecond = 4;
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider((HitoBitApiCredentials)credentials);

        #region methods

        #region Aggregate Trade Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol,
            Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default) =>
            await SubscribeToAggregatedTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
            IEnumerable<string> symbols, Action<DataEvent<HitoBitStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            foreach (var symbol in symbols)
                symbol.ValidateHitoBitSymbol();

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamAggregatedTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + aggregatedTradesStreamEndpoint)
                .ToArray();
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

        #region Kline/Candlestick Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) =>
            await SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
            IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) =>
            await SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols,
            KlineInterval interval, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default) =>
            await SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols,
            IEnumerable<KlineInterval> intervals, Action<DataEvent<IHitoBitStreamKlineData>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            foreach (var symbol in symbols)
                symbol.ValidateHitoBitSymbol();

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamKlineData>>>(data => onMessage(data.As<IHitoBitStreamKlineData>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.SelectMany(a =>
                intervals.Select(i =>
                    a.ToLower(CultureInfo.InvariantCulture) + klineStreamEndpoint + "_" +
                    JsonConvert.SerializeObject(i, new KlineIntervalConverter(false)))).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Mini Ticker Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol,
            Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default) =>
            await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(
            IEnumerable<string> symbols, Action<DataEvent<IHitoBitMiniTick>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            foreach (var symbol in symbols)
                symbol.ValidateHitoBitSymbol();

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamMiniTick>>>(data => onMessage(data.As<IHitoBitMiniTick>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolMiniTickerStreamEndpoint)
                .ToArray();

            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Mini Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(
            Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBitMiniTick>>(data.Data.Data, data.Data.Stream)));
            return await SubscribeAsync(BaseAddress, new[] { allSymbolMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Market Rolling Window Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToRollingWindowTickerUpdatesAsync(string symbol, TimeSpan windowSize,
            Action<DataEvent<HitoBitStreamRollingWindowTick>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamRollingWindowTick>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
            var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
            return await SubscribeAsync(BaseAddress, new[] { $"{symbol.ToLowerInvariant()}@ticker_{windowString}" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Rolling Window Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllRollingWindowTickerUpdatesAsync(TimeSpan windowSize,
            Action<DataEvent<IEnumerable<HitoBitStreamRollingWindowTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamRollingWindowTick>>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
            var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
            return await SubscribeAsync(BaseAddress, new[] { $"!ticker_{windowString}@arr" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Book Ticker Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol,
            Action<DataEvent<HitoBitStreamBookPrice>> onMessage, CancellationToken ct = default) =>
            await SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols,
            Action<DataEvent<HitoBitStreamBookPrice>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            foreach (var symbol in symbols)
                symbol.ValidateHitoBitSymbol();

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + bookTickerStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Partial Book Depth Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol,
            int levels, int? updateInterval, Action<DataEvent<IHitoBitOrderBook>> onMessage, CancellationToken ct = default) =>
            await SubscribeToPartialOrderBookUpdatesAsync(new[] { symbol }, levels, updateInterval, onMessage, ct)
                .ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
            IEnumerable<string> symbols, int levels, int? updateInterval, Action<DataEvent<IHitoBitOrderBook>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            foreach (var symbol in symbols)
                symbol.ValidateHitoBitSymbol();

            levels.ValidateIntValues(nameof(levels), 5, 10, 20);
            updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitOrderBook>>>(data =>
            {
                data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
                onMessage(data.As<IHitoBitOrderBook>(data.Data.Data, data.Data.Data.Symbol));
            });

            symbols = symbols.Select(a =>
                a.ToLower(CultureInfo.InvariantCulture) + partialBookDepthStreamEndpoint + levels +
                (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Diff. Depth Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol,
            int? updateInterval, Action<DataEvent<IHitoBitEventOrderBook>> onMessage, CancellationToken ct = default) =>
            await SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols,
            int? updateInterval, Action<DataEvent<IHitoBitEventOrderBook>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));
            foreach (var symbol in symbols)
                symbol.ValidateHitoBitSymbol();

            updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitEventOrderBook>>>(data => onMessage(data.As<IHitoBitEventOrderBook>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a =>
                a.ToLower(CultureInfo.InvariantCulture) + depthStreamEndpoint +
                (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Symbol Ticker Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<IHitoBitTick>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IHitoBitTick>> onMessage, CancellationToken ct = default)
        {
            symbols.ValidateNotNull(nameof(symbols));

            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamTick>>>(data => onMessage(data.As<IHitoBitTick>(data.Data.Data, data.Data.Data.Symbol)));
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolTickerStreamEndpoint).ToArray();
            return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Tickers Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBitTick>>(data.Data.Data, data.Data.Stream)));
            return await SubscribeAsync(BaseAddress, new[] { allSymbolTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region User Data Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
            string listenKey,
            Action<DataEvent<HitoBitStreamOrderUpdate>>? onOrderUpdateMessage,
            Action<DataEvent<HitoBitStreamOrderList>>? onOcoOrderUpdateMessage,
            Action<DataEvent<HitoBitStreamPositionsUpdate>>? onAccountPositionMessage,
            Action<DataEvent<HitoBitStreamBalanceUpdate>>? onAccountBalanceUpdate,
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
                    case executionUpdateEvent:
                        {
                            var result = Deserialize<HitoBitStreamOrderUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onOrderUpdateMessage?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                            }
                            else
                                _log.Write(LogLevel.Warning,
                                    "Couldn't deserialize data received from order stream: " + result.Error);
                            break;
                        }
                    case ocoOrderUpdateEvent:
                        {
                            var result = Deserialize<HitoBitStreamOrderList>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onOcoOrderUpdateMessage?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                            }
                            else
                                _log.Write(LogLevel.Warning,
                                    "Couldn't deserialize data received from oco order stream: " + result.Error);
                            break;
                        }
                    case accountPositionUpdateEvent:
                        {
                            var result = Deserialize<HitoBitStreamPositionsUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onAccountPositionMessage?.Invoke(data.As(result.Data));
                            }
                            else
                                _log.Write(LogLevel.Warning,
                                    "Couldn't deserialize data received from account position stream: " + result.Error);
                            break;
                        }
                    case balanceUpdateEvent:
                        {
                            var result = Deserialize<HitoBitStreamBalanceUpdate>(token);
                            if (result)
                            {
                                result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                                onAccountBalanceUpdate?.Invoke(data.As(result.Data, result.Data.Asset));
                            }
                            else
                                _log.Write(LogLevel.Warning,
                                    "Couldn't deserialize data received from account position stream: " + result.Error);
                            break;
                        }
                    default:
                        _log.Write(LogLevel.Warning, $"Received unknown user data event {evnt}: " + data);
                        break;
                }
            });

            return await SubscribeAsync(BaseAddress, new[] { listenKey }, handler, ct).ConfigureAwait(false);
        }
        #endregion

        #region Blvt info update
        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(string token,
            Action<DataEvent<HitoBitBlvtInfoUpdate>> onMessage, CancellationToken ct = default)
            => SubscribeToBlvtInfoUpdatesAsync(new List<string> { token }, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(IEnumerable<string> tokens, Action<DataEvent<HitoBitBlvtInfoUpdate>> onMessage, CancellationToken ct = default)
        {
            if (_options.BlvtStreamAddress == null)
                throw new Exception("No url found for Blvt stream, check the `BlvtStreamAddress` client option");

            tokens = tokens.Select(a => a.ToUpper(CultureInfo.InvariantCulture) + bltvInfoEndpoint).ToArray();
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitBlvtInfoUpdate>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.TokenName)));
            return await SubscribeAsync(_options.BlvtStreamAddress, tokens, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Blvt kline update
        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(string token,
            KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default) =>
            SubscribeToBlvtKlineUpdatesAsync(new List<string> { token }, interval, onMessage, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(IEnumerable<string> tokens, KlineInterval interval, Action<DataEvent<HitoBitStreamKlineData>> onMessage, CancellationToken ct = default)
        {
            if (_options.BlvtStreamAddress == null)
                throw new Exception("No url found for Blvt stream, check the `BlvtStreamAddress` client option");

            tokens = tokens.Select(a => a.ToUpper(CultureInfo.InvariantCulture) + bltvKlineEndpoint + "_" + JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))).ToArray();
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            return await SubscribeAsync(_options.BlvtStreamAddress, tokens, handler, ct).ConfigureAwait(false);
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
            var result = message;

            if (result != null && message.Count() > 0)
            {
                if (message.Value<bool>("hasError"))
                {

                    callResult = new CallResult<object>(new ServerError("Unknown error: " + message.Value<string>("data")));
                    return true;
                }

                _log.Write(LogLevel.Trace, $"Socket {s.SocketId} Subscription completed");



                callResult = new CallResult<object>(new object());
                return true;
            }

            callResult = new CallResult<object>(new ServerError("Unknown error: " + message));
            return false;
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

            await connection.SendAndWaitAsync(unsub, Options.SocketResponseTimeout, null, data =>
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

        protected override IWebsocket CreateSocket(string address)
        {
            return new SignalRCryptoExchangeWebSocketClient(_log, GetWebSocketParameters(address), null);
        }
    }
}
