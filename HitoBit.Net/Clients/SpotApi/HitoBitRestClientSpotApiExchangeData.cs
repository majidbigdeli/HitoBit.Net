using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using HitoBit.Net.Objects.Models.Spot.Convert;
using HitoBit.Net.Objects.Models.Spot.IsolatedMargin;
using HitoBit.Net.Objects.Models.Spot.Margin;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class HitoBitRestClientSpotApiExchangeData : IHitoBitRestClientSpotApiExchangeData
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly ILogger _logger;

        private readonly HitoBitRestClientSpotApi _baseClient;

        internal HitoBitRestClientSpotApiExchangeData(ILogger logger, HitoBitRestClientSpotApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        #region Test Connectivity

        /// <inheritdoc />
        public async Task<WebCallResult<long>> PingAsync(CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ping", HitoBitExchange.RateLimiter.SpotRestIp);
            var result = await _baseClient.SendAsync<object>(request, null, ct).ConfigureAwait(false);
            sw.Stop();
            return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
        }   

        #endregion

        #region Check Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/time", HitoBitExchange.RateLimiter.SpotRestIp);
            var result = await _baseClient.SendAsync<HitoBitCheckTime>(request, null, ct).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);            
        }

        #endregion

        #region Exchange Information

        /// <inheritdoc />
        public Task<WebCallResult<HitoBitExchangeInfo>> GetExchangeInfoAsync(bool? returnPermissionSets = null, SymbolStatus? symbolStatus = null, CancellationToken ct = default)
             => GetExchangeInfoAsync(Array.Empty<string>(), returnPermissionSets, symbolStatus, ct);

        /// <inheritdoc />
        public Task<WebCallResult<HitoBitExchangeInfo>> GetExchangeInfoAsync(string symbol, bool? returnPermissionSets = null, CancellationToken ct = default)
             => GetExchangeInfoAsync(new string[] { symbol }, returnPermissionSets, null, ct);

        /// <inheritdoc />
        public Task<WebCallResult<HitoBitExchangeInfo>> GetExchangeInfoAsync(AccountType permission, bool? returnPermissionSets = null, SymbolStatus? symbolStatus = null, CancellationToken ct = default)
             => GetExchangeInfoAsync(new AccountType[] { permission }, returnPermissionSets, symbolStatus, ct);

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitExchangeInfo>> GetExchangeInfoAsync(AccountType[] permissions, bool? returnPermissionSets = null, SymbolStatus? symbolStatus = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("showPermissionSets", returnPermissionSets?.ToString().ToLowerInvariant());
            parameters.AddOptionalEnum("symbolStatus", symbolStatus);

            if (permissions.Length > 1)
            {
                List<string> list = new List<string>();
                foreach (var permission in permissions)
                {
                    list.Add(permission.ToString().ToUpper());
                }

                parameters.Add("permissions", JsonSerializer.Serialize(list));
            }
            else if (permissions.Any())
            {
                parameters.Add("permissions", permissions.First().ToString().ToUpper());
            }

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/exchangeInfo", HitoBitExchange.RateLimiter.SpotRestIp, 20, false, arraySerialization: ArrayParametersSerialization.Array);
            var exchangeInfoResult = await _baseClient.SendAsync<HitoBitExchangeInfo>(request, parameters, ct).ConfigureAwait(false);
            if (!exchangeInfoResult)
                return exchangeInfoResult;

            _baseClient._exchangeInfo = exchangeInfoResult.Data;
            _baseClient._lastExchangeInfoUpdate = DateTime.UtcNow;
            _logger.Log(LogLevel.Information, "Trade rules updated");
            return exchangeInfoResult;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitExchangeInfo>> GetExchangeInfoAsync(IEnumerable<string> symbols, bool? returnPermissionSets = null, SymbolStatus? symbolStatus = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("showPermissionSets", returnPermissionSets?.ToString().ToLowerInvariant());
            parameters.AddOptionalEnum("symbolStatus", symbolStatus);

            if (symbols.Count() > 1)
            {
                parameters.Add("symbols", JsonSerializer.Serialize(symbols));
            }
            else if (symbols.Any())
            {
                parameters.Add("symbol", symbols.First());
            }

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/exchangeInfo", HitoBitExchange.RateLimiter.SpotRestIp, 20, false, arraySerialization: ArrayParametersSerialization.Array);
            var exchangeInfoResult = await _baseClient.SendAsync<HitoBitExchangeInfo>(request, parameters, ct).ConfigureAwait(false);
            if (!exchangeInfoResult)
                return exchangeInfoResult;

            _baseClient._exchangeInfo = exchangeInfoResult.Data;
            _baseClient._lastExchangeInfoUpdate = DateTime.UtcNow;
            _logger.Log(LogLevel.Information, "Trade rules updated");
            return exchangeInfoResult;
        }

        #endregion

        #region System status
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/system/status", HitoBitExchange.RateLimiter.SpotRestIp);
            return await _baseClient.SendAsync<HitoBitSystemStatus>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Asset Details
        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, HitoBitAssetDetails>>> GetAssetDetailsAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/asset/assetDetail", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<Dictionary<string, HitoBitAssetDetails>>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Get products

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitProduct>>> GetProductsAsync(CancellationToken ct = default)
        {
            var url = _baseClient.ClientOptions.Environment.SpotRestAddress.Replace("api.", "www.");
            var request = _definitions.GetOrCreate(HttpMethod.Get, "bapi/asset/v2/public/asset-service/product/get-products");
            var data = await _baseClient.SendToAddressAsync<HitoBitExchangeApiWrapper<IEnumerable<HitoBitProduct>>>(url, request, null, ct).ConfigureAwait(false);
            if (!data)
                return data.As<IEnumerable<HitoBitProduct>>(null);

            if (!data.Data.Success)
                return data.AsError<IEnumerable<HitoBitProduct>>(new ServerError(data.Data.Code, data.Data.Message + " - " + data.Data.MessageDetail));

            return data.As(data.Data.Data);
        }
        #endregion

        #region Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 5000);
            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            var requestWeight = limit == null ? 5 : limit <= 100 ? 5 : limit <= 500 ? 25 : limit <= 1000 ? 50 : 250;

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/depth", HitoBitExchange.RateLimiter.SpotRestIp, requestWeight);
            var result = await _baseClient.SendAsync<HitoBitOrderBook>(request, parameters, ct, requestWeight).ConfigureAwait(false);
            if (result)
                result.Data.Symbol = symbol;
            return result;
        }

        #endregion

        #region Recent Trades List

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/trades", HitoBitExchange.RateLimiter.SpotRestIp, 25);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitRecentTradeQuote>>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitRecentTrade>>(result.Data);
        }

        #endregion

        #region Old Trade Lookup

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);
            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/historicalTrades", HitoBitExchange.RateLimiter.SpotRestIp, 25);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitRecentTradeQuote>>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitRecentTrade>>(result.Data);
        }

        #endregion

        #region Compressed/Aggregate Trades List

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/aggTrades", HitoBitExchange.RateLimiter.SpotRestIp, 2);
            return await _baseClient.SendAsync<IEnumerable<HitoBitAggregatedTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Kline/Candlestick Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new ParameterCollection {
                { "symbol", symbol },
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/klines", HitoBitExchange.RateLimiter.SpotRestIp, 2);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitSpotKline>>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region UI Kline Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetUiKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new ParameterCollection {
                { "symbol", symbol },
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/uiKlines", HitoBitExchange.RateLimiter.SpotRestIp, 2);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitSpotKline>>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region Current Average Price

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAveragePrice>> GetCurrentAvgPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbol", symbol } };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/avgPrice", HitoBitExchange.RateLimiter.SpotRestIp, 2);
            return await _baseClient.SendAsync<HitoBitAveragePrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region 24hr Ticker Price Change Statistics

        /// <inheritdoc />
        public async Task<WebCallResult<IHitoBitTick>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbol", symbol } };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/24hr", HitoBitExchange.RateLimiter.SpotRestIp, 1);
            var result = await _baseClient.SendAsync<HitoBit24HPrice>(request, parameters, ct, 1).ConfigureAwait(false);
            return result.As<IHitoBitTick>(result.Data);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitTick>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbols", $"[{string.Join("," ,symbols.Select(s => $"\"{s}\""))}]" } };
            var symbolCount = symbols.Count();
            var weight = symbolCount <= 20 ? 2 : symbolCount <= 100 ? 40 : 80;

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/24hr", HitoBitExchange.RateLimiter.SpotRestIp, weight);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBit24HPrice>>(request, parameters, ct, weight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitTick>>(result.Data);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitTick>>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/24hr", HitoBitExchange.RateLimiter.SpotRestIp, 80);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBit24HPrice>>(request, null, ct, 80).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitTick>>(result.Data);
        }

        #endregion

        #region Trading Day Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTradingDayTicker>> GetTradingDayTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection 
            { 
                { "symbol", symbol }
            };
            parameters.AddOptional("timeZone", timeZone);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/tradingDay", HitoBitExchange.RateLimiter.SpotRestIp, 4);
            return await _baseClient.SendAsync<HitoBitTradingDayTicker>(request, parameters, ct, 1).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitTradingDayTicker>>> GetTradingDayTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
            parameters.AddOptional("timeZone", timeZone);
            var symbolCount = symbols.Count();
            var weight = Math.Min(symbolCount * 4, 50);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/tradingDay", HitoBitExchange.RateLimiter.SpotRestIp, weight);
            return await _baseClient.SendAsync<IEnumerable<HitoBitTradingDayTicker>>(request, parameters, ct, weight).ConfigureAwait(false);
        }
        #endregion

        #region Rolling window price change ticker

        /// <inheritdoc />
        public async Task<WebCallResult<IHitoBit24HPrice>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker", HitoBitExchange.RateLimiter.SpotRestIp, 2);
            var result = await _baseClient.SendAsync<HitoBit24HPrice>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IHitoBit24HPrice>(result.Data);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBit24HPrice>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
            parameters.AddOptionalParameter("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
            var symbolCount = symbols.Count();
            var weight = Math.Min(symbolCount * 4, 100);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker", HitoBitExchange.RateLimiter.SpotRestIp, weight);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBit24HPrice>>(request, parameters, ct, weight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBit24HPrice>>(result.Data);
        }

        private string GetWindowSize(TimeSpan timeSpan)
        {
            if (timeSpan.TotalHours < 1)
                return timeSpan.TotalMinutes + "m";
            else if (timeSpan.TotalHours < 24)
                return timeSpan.TotalHours + "h";
            return timeSpan.TotalDays + "d";
        }
        #endregion

        #region Symbol Price Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitPrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/price", HitoBitExchange.RateLimiter.SpotRestIp, 1);
            return await _baseClient.SendAsync<HitoBitPrice>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitPrice>>> GetPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/price", HitoBitExchange.RateLimiter.SpotRestIp, 4);
            return await _baseClient.SendAsync<IEnumerable<HitoBitPrice>>(request, parameters, ct, 4).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitPrice>>> GetPricesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/price", HitoBitExchange.RateLimiter.SpotRestIp, 4);
            return await _baseClient.SendAsync<IEnumerable<HitoBitPrice>>(request, null, ct, 4).ConfigureAwait(false);
        }

        #endregion

        #region Symbol Order Book Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBookPrice>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbol", symbol } };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/bookTicker", HitoBitExchange.RateLimiter.SpotRestIp, 2);
            return await _baseClient.SendAsync<HitoBitBookPrice>(request, parameters, ct, 2).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBookPrice>>> GetBookPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/bookTicker", HitoBitExchange.RateLimiter.SpotRestIp, 4);
            return await _baseClient.SendAsync<IEnumerable<HitoBitBookPrice>>(request, parameters, ct, 4).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBookPrice>>> GetBookPricesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/v3/ticker/bookTicker", HitoBitExchange.RateLimiter.SpotRestIp, 4);
            return await _baseClient.SendAsync<IEnumerable<HitoBitBookPrice>>(request, null, ct, 4).ConfigureAwait(false);
        }

        #endregion

        #region Get All Margin Assets

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMarginAsset>>> GetMarginAssetsAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("asset", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/margin/allAssets", HitoBitExchange.RateLimiter.SpotRestIp, 1);
            return await _baseClient.SendAsync<IEnumerable<HitoBitMarginAsset>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get All Margin Pairs

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMarginPair>>> GetMarginSymbolsAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/margin/allPairs", HitoBitExchange.RateLimiter.SpotRestIp, 1);
            return await _baseClient.SendAsync<IEnumerable<HitoBitMarginPair>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Query Margin PriceIndex
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitMarginPriceIndex>> GetMarginPriceIndexAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateNotNull(nameof(symbol));

            var parameters = new ParameterCollection
            {
                {"symbol", symbol}
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/margin/priceIndex", HitoBitExchange.RateLimiter.SpotRestIp, 10);
            return await _baseClient.SendAsync<HitoBitMarginPriceIndex>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Query isolated margin symbol

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(string? symbol = null, int? receiveWindow =
            null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow
                                                              ?.ToString(CultureInfo.InvariantCulture) ??
                                                          _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(
                                                              CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/margin/isolated/allPairs", HitoBitExchange.RateLimiter.SpotRestIp, 10, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitIsolatedMarginSymbol>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Leveraged tokens

        #region Get Leveraged Token info

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBlvtInfo>>> GetLeveragedTokenInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/blvt/tokenInfo", HitoBitExchange.RateLimiter.SpotRestIp, 1);
            return await _baseClient.SendAsync<IEnumerable<HitoBitBlvtInfo>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get historical klines
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBlvtKline>>> GetLeveragedTokensHistoricalKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var url = _baseClient.ClientOptions.Environment.UsdFuturesRestAddress;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/lvtKlines", HitoBitExchange.RateLimiter.FuturesRest, 1);
            return await _baseClient.SendToAddressAsync<IEnumerable<HitoBitBlvtKline>>(url!, request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Get Cross Margin Colleteral Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitCrossMarginCollateralRatio>>> GetCrossMarginCollateralRatioAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/margin/crossMarginCollateralRatio", HitoBitExchange.RateLimiter.SpotRestIp, 100, false);
            return await _baseClient.SendAsync<IEnumerable<HitoBitCrossMarginCollateralRatio>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Future Hourly Interest Rate

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesInterestRate>>> GetFutureHourlyInterestRateAsync(IEnumerable<string> assets, bool isolated, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "assets", string.Join(",", assets) },
                { "isIsolated", isolated.ToString().ToUpper() }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/margin/next-hourly-interest-rate", HitoBitExchange.RateLimiter.SpotRestIp, 100, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesInterestRate>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Margin Delist Schedule

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMarginDelistSchedule>>> GetMarginDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/margin/delist-schedule", HitoBitExchange.RateLimiter.SpotRestIp, 100);
            return await _baseClient.SendAsync<IEnumerable<HitoBitMarginDelistSchedule>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Convert

        #region Get Convert List All Pairs

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitConvertAssetPair>>> GetConvertListAllPairsAsync(string? quoteAsset = null, string? baseAsset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("fromAsset", quoteAsset);
            parameters.AddOptionalParameter("toAsset", baseAsset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/convert/exchangeInfo", HitoBitExchange.RateLimiter.SpotRestIp, 20);
            return await _baseClient.SendAsync<IEnumerable<HitoBitConvertAssetPair>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Convert Quantity Precision Per Asset

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitConvertQuantityPrecisionAsset>>> GetConvertQuantityPrecisionPerAssetAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/convert/assetInfo", HitoBitExchange.RateLimiter.SpotRestIp, 100);
            return await _baseClient.SendAsync<IEnumerable<HitoBitConvertQuantityPrecisionAsset>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Get Delist Schedule

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitDelistSchedule>>> GetDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/spot/delist-schedule", HitoBitExchange.RateLimiter.SpotRestIp, 100);
            return await _baseClient.SendAsync<IEnumerable<HitoBitDelistSchedule>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
