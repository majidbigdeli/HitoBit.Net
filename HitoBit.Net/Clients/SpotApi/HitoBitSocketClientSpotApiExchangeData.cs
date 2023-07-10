
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class HitoBitSocketClientSpotApiExchangeData : IHitoBitSocketClientSpotApiExchangeData
    {
        private readonly ILogger _logger;
        private readonly HitoBitSocketClientSpotApi _client;

        #region constructor/destructor

        internal HitoBitSocketClientSpotApiExchangeData(ILogger logger, HitoBitSocketClientSpotApi client)
        {
            _client = client;
            _logger = logger;
        }

        #endregion

        #region Queries

        #region Ping

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<object>>> PingAsync()
        {
            return await _client.QueryAsync<object>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"ping", new Dictionary<string, object>()).ConfigureAwait(false);
        }

        #endregion

        #region Get Server Time

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<DateTime>>> GetServerTimeAsync()
        {
            var result = await _client.QueryAsync<HitoBitCheckTime>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"time", new Dictionary<string, object>()).ConfigureAwait(false);
            if (!result)
                return result.AsError<HitoBitResponse<DateTime>>(result.Error!);

            return result.As(new HitoBitResponse<DateTime>
            {
                Ratelimits = result.Data!.Ratelimits!,
                Result = result.Data!.Result!.ServerTime!
            });
        }

        #endregion

        #region Get Exchange Info

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitExchangeInfo>>> GetExchangeInfoAsync(IEnumerable<string>? symbols = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbols", symbols);
            var result = await _client.QueryAsync<HitoBitExchangeInfo>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"exchangeInfo", parameters).ConfigureAwait(false);
            if (!result)
                return result;

            _client._exchangeInfo = result.Data.Result;
            _client._lastExchangeInfoUpdate = DateTime.UtcNow;
            _logger.Log(LogLevel.Information, "Trade rules updated");
            return result;
        }

        #endregion

        #region Get Orderbook

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitOrderBook>>> GetOrderBookAsync(string symbol, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            parameters.AddOptionalParameter("limit", limit);
            return await _client.QueryAsync<HitoBitOrderBook>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"depth", parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitRecentTradeQuote>>>> GetRecentTradesAsync(string symbol, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            parameters.AddOptionalParameter("limit", limit);
            return await _client.QueryAsync<IEnumerable<HitoBitRecentTradeQuote>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"trades.recent", parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitRecentTradeQuote>>>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("fromId", fromId);
            return await _client.QueryAsync<IEnumerable<HitoBitRecentTradeQuote>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"trades.historical", parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Aggregated Trades

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitStreamAggregatedTrade>>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("fromId", fromId);
            return await _client.QueryAsync<IEnumerable<HitoBitStreamAggregatedTrade>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"trades.aggregate", parameters, false).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitSpotKline>>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            parameters.AddParameter("interval", EnumConverter.GetString(interval));
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            return await _client.QueryAsync<IEnumerable<HitoBitSpotKline>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"klines", parameters, false).ConfigureAwait(false);
        }

        #endregion

        #region Get UI Klines

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitSpotKline>>>> GetUIKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            parameters.AddParameter("interval", EnumConverter.GetString(interval));
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            return await _client.QueryAsync<IEnumerable<HitoBitSpotKline>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"uiKlines", parameters, false).ConfigureAwait(false);
        }

        #endregion

        #region Get Average Price

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<HitoBitAveragePrice>>> GetCurrentAvgPriceAsync(string symbol)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddParameter("symbol", symbol);
            return await _client.QueryAsync<HitoBitAveragePrice>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"avgPrice", parameters, false).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBit24HPrice>>>> GetTickersAsync(IEnumerable<string>? symbols = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbols", symbols);
            return await _client.QueryAsync<IEnumerable<HitoBit24HPrice>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"ticker.24hr", parameters, false).ConfigureAwait(false);
        }

        #endregion

        #region Get Rolling Window Tickers

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitRollingWindowTick>>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbols", symbols);
            return await _client.QueryAsync<IEnumerable<HitoBitRollingWindowTick>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"ticker", parameters, false).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Tickers

        /// <inheritdoc />
        public async Task<CallResult<HitoBitResponse<IEnumerable<HitoBitBookPrice>>>> GetBookTickersAsync(IEnumerable<string>? symbols = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbols", symbols);
            return await _client.QueryAsync<IEnumerable<HitoBitBookPrice>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"ticker.book", parameters, false).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Streams

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
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@trade").ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

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
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@aggTrade")
                .ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
                    a.ToLower(CultureInfo.InvariantCulture) + "@kline" + "_" +
                    JsonConvert.SerializeObject(i, new KlineIntervalConverter(false)))).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@miniTicker")
                .ToArray();

            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Mini Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(
            Action<DataEvent<IEnumerable<IHitoBitMiniTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBitMiniTick>>(data.Data.Data, data.Data.Stream)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { "!miniTicker@arr" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region Individual Market Rolling Window Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToRollingWindowTickerUpdatesAsync(string symbol, TimeSpan windowSize,
            Action<DataEvent<HitoBitStreamRollingWindowTick>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamRollingWindowTick>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
            var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { $"{symbol.ToLowerInvariant()}@ticker_{windowString}" }, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Rolling Window Tickers Stream

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllRollingWindowTickerUpdatesAsync(TimeSpan windowSize,
            Action<DataEvent<IEnumerable<HitoBitStreamRollingWindowTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamRollingWindowTick>>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
            var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { $"!ticker_{windowString}@arr" }, handler, ct).ConfigureAwait(false);
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
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@bookTicker").ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
                a.ToLower(CultureInfo.InvariantCulture) + "@depth" + levels +
                (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
                a.ToLower(CultureInfo.InvariantCulture) + "@depth" +
                (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
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
            symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@ticker").ToArray();
            return await _client.SubscribeAsync(_client.BaseAddress, symbols, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #region All Market Tickers Streams

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<DataEvent<IEnumerable<IHitoBitTick>>> onMessage, CancellationToken ct = default)
        {
            var handler = new Action<DataEvent<HitoBitCombinedStream<IEnumerable<HitoBitStreamTick>>>>(data => onMessage(data.As<IEnumerable<IHitoBitTick>>(data.Data.Data, data.Data.Stream)));
            return await _client.SubscribeAsync(_client.BaseAddress, new[] { "!ticker@arr" }, handler, ct).ConfigureAwait(false);
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
            var address = ((HitoBitEnvironment)_client.ClientOptions.Environment).BlvtSocketAddress;
            if (address == null)
                throw new Exception("No url found for Blvt stream, check the `BlvtSocketAddress` in the client environment");

            tokens = tokens.Select(a => a.ToUpper(CultureInfo.InvariantCulture) + "@tokenNav").ToArray();
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitBlvtInfoUpdate>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.TokenName)));
            return await _client.SubscribeAsync(address.AppendPath("lvt-p"), tokens, handler, ct).ConfigureAwait(false);
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
            var address = ((HitoBitEnvironment)_client.ClientOptions.Environment).BlvtSocketAddress;
            if (address == null)
                throw new Exception("No url found for Blvt stream, check the `BlvtSocketAddress` in the client environment");

            tokens = tokens.Select(a => a.ToUpper(CultureInfo.InvariantCulture) + "@nav_kline" + "_" + JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))).ToArray();
            var handler = new Action<DataEvent<HitoBitCombinedStream<HitoBitStreamKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
            return await _client.SubscribeAsync(address.AppendPath("lvt-p"), tokens, handler, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

    }
}
