using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects.Models.Futures;
using HitoBit.Net.Objects.Models.Spot;
using CryptoExchange.Net.RateLimiting.Guards;
using System.Diagnostics;

namespace HitoBit.Net.Clients.UsdFuturesApi
{
    /// <inheritdoc />
    internal class HitoBitRestClientUsdFuturesApiExchangeData : IHitoBitRestClientUsdFuturesApiExchangeData
    {
        private readonly ILogger _logger;
        private static readonly RequestDefinitionCache _definitions = new();

        private readonly HitoBitRestClientUsdFuturesApi _baseClient;

        internal HitoBitRestClientUsdFuturesApiExchangeData(ILogger logger, HitoBitRestClientUsdFuturesApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        #region Test Connectivity

        /// <inheritdoc />
        public async Task<WebCallResult<long>> PingAsync(CancellationToken ct = default)
        {
            var sw = Stopwatch.StartNew();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ping", HitoBitExchange.RateLimiter.FuturesRest, 1);
            var result = await _baseClient.SendAsync<object>(request, null, ct).ConfigureAwait(false);
            sw.Stop();
            return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
        }

        #endregion

        #region Check Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(bool resetAutoTimestamp = false, CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/time", HitoBitExchange.RateLimiter.FuturesRest, 1);
            var result = await _baseClient.SendAsync<HitoBitCheckTime>(request, null, ct).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        #endregion

        #region Exchange Information

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesUsdtExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/exchangeInfo", HitoBitExchange.RateLimiter.FuturesRest, 1);
            var exchangeInfoResult = await _baseClient.SendAsync<HitoBitFuturesUsdtExchangeInfo>(request, null, ct).ConfigureAwait(false);
            if (!exchangeInfoResult)
                return exchangeInfoResult;

            _baseClient._exchangeInfo = exchangeInfoResult.Data;
            _baseClient._lastExchangeInfoUpdate = DateTime.UtcNow;
            _logger.Log(LogLevel.Information, "Trade rules updated");
            return exchangeInfoResult;
        }

        #endregion

        #region Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000);
            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/depth", HitoBitExchange.RateLimiter.FuturesRest, requestWeight);
            var result = await _baseClient.SendAsync<HitoBitFuturesOrderBook>(request, parameters, ct, requestWeight).ConfigureAwait(false);
            if (result && string.IsNullOrEmpty(result.Data.Symbol))
                result.Data.Symbol = symbol;
            return result.As(result.Data);
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/aggTrades", HitoBitExchange.RateLimiter.FuturesRest, 20);
            return await _baseClient.SendAsync<IEnumerable<HitoBitAggregatedTrade>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Info

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesFundingInfo>>> GetFundingInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/fundingInfo", HitoBitExchange.RateLimiter.FuturesRest, 0);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesFundingInfo>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesFundingRateHistory>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);
            var parameters = new ParameterCollection {
                { "symbol", symbol }
            };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/fundingRate", HitoBitExchange.RateLimiter.EndpointLimit, 1, false,
                limitGuard: new SingleLimitGuard(500, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesFundingRateHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Top Trader Long/Short Ratio (Accounts)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new ParameterCollection {
                { "symbol", symbolPair },
            };

            parameters.AddEnum("period", period);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/topLongShortAccountRatio", HitoBitExchange.RateLimiter.EndpointLimit, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Top Trader Long/Short Ratio (Positions)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new ParameterCollection {
                { "symbol", symbolPair },
            };

            parameters.AddEnum("period", period);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/topLongShortPositionRatio", HitoBitExchange.RateLimiter.EndpointLimit, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Global Long/Short Ratio (Accounts)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new ParameterCollection {
                { "symbol", symbolPair },
            };

            parameters.AddEnum("period", period);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/globalLongShortAccountRatio", HitoBitExchange.RateLimiter.EndpointLimit, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Mark Price Kline/Candlestick Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMarkIndexKline>>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);

            var parameters = new ParameterCollection {
                { "symbol", symbol },
            };

            parameters.AddEnum("interval", interval);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/markPriceKlines", HitoBitExchange.RateLimiter.FuturesRest, requestWeight);
            return await _baseClient.SendAsync<IEnumerable<HitoBitMarkIndexKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        }

        #endregion

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/trades", HitoBitExchange.RateLimiter.FuturesRest, 5);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitRecentTradeQuote>>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitRecentTrade>>(result.Data);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null,
            CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);
            var parameters = new ParameterCollection { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/historicalTrades", HitoBitExchange.RateLimiter.FuturesRest, 20);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitRecentTradeQuote>>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitRecentTrade>>(result.Data);
        }

        #region Mark Price

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesMarkPrice>> GetMarkPriceAsync(string symbol,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/premiumIndex", HitoBitExchange.RateLimiter.FuturesRest, 1);
            return await _baseClient.SendAsync<HitoBitFuturesMarkPrice>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesMarkPrice>>> GetMarkPricesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/premiumIndex", HitoBitExchange.RateLimiter.FuturesRest, 10);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesMarkPrice>>(request, null, ct).ConfigureAwait(false);
        }
        #endregion

        #region 24h statistics
        /// <inheritdoc />
        public async Task<WebCallResult<IHitoBit24HPrice>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/24hr", HitoBitExchange.RateLimiter.FuturesRest, 1);
            var result = await _baseClient.SendAsync<HitoBit24HPrice>(request, parameters, ct).ConfigureAwait(false);
            return result.As<IHitoBit24HPrice>(result.Data);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBit24HPrice>>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/24hr", HitoBitExchange.RateLimiter.FuturesRest, 40);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBit24HPrice>>(request, null, ct).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBit24HPrice>>(result.Data);
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

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/klines", HitoBitExchange.RateLimiter.FuturesRest, requestWeight);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitFuturesUsdtKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region Kline/Premium Index

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMarkIndexKline>>> GetPremiumIndexKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new ParameterCollection {
                { "symbol", symbol },
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/premiumIndexKlines", HitoBitExchange.RateLimiter.FuturesRest, requestWeight);
            return await _baseClient.SendAsync<IEnumerable<HitoBitMarkIndexKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        }

        #endregion

        #region Symbol Order Book Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBookPrice>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/bookTicker", HitoBitExchange.RateLimiter.FuturesRest, 2);
            return await _baseClient.SendAsync<HitoBitBookPrice>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBookPrice>>> GetBookPricesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/bookTicker", HitoBitExchange.RateLimiter.FuturesRest, 5);
            return await _baseClient.SendAsync<IEnumerable<HitoBitBookPrice>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Open Interest

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/openInterest", HitoBitExchange.RateLimiter.FuturesRest, 1);
            return await _baseClient.SendAsync<HitoBitFuturesOpenInterest>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Open Interest History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesOpenInterestHistory>>> GetOpenInterestHistoryAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new ParameterCollection {
                { "symbol", symbol },
            };

            parameters.AddEnum("period", period);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/openInterestHist", HitoBitExchange.RateLimiter.EndpointLimit, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesOpenInterestHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Taker Buy/Sell Volume Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new ParameterCollection {
                { "symbol", symbol },
            };

            parameters.AddEnum("period", period);
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/takerlongshortRatio", HitoBitExchange.RateLimiter.EndpointLimit, 1, false,
                limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesBuySellVolumeRatio>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Composite index symbol information

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesCompositeIndexInfo>>> GetCompositeIndexInfoAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);

            var weight = symbol == null ? 10 : 1;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/indexInfo", HitoBitExchange.RateLimiter.FuturesRest, weight);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesCompositeIndexInfo>>(request, parameters, ct, weight).ConfigureAwait(false);
        }

        #endregion

        #region Get price

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitPrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v2/ticker/price", HitoBitExchange.RateLimiter.FuturesRest, 1);
            return await _baseClient.SendAsync<HitoBitPrice>(request, parameters, ct, 1).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitPrice>>> GetPricesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v2/ticker/price", HitoBitExchange.RateLimiter.FuturesRest, 2);
            return await _baseClient.SendAsync<IEnumerable<HitoBitPrice>>(request, null, ct, 2).ConfigureAwait(false);
        }
        #endregion

        #region Continuous contract Kline Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetContinuousContractKlinesAsync(string pair, ContractType contractType, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new ParameterCollection {
                { "pair", pair },
            };
            parameters.AddEnum("interval", interval);
            parameters.AddEnum("contractType", contractType);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/continuousKlines", HitoBitExchange.RateLimiter.FuturesRest, requestWeight);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitFuturesUsdtKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region Index Price Kline Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetIndexPriceKlinesAsync(string pair, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new ParameterCollection {
                { "pair", pair },
            };
            parameters.AddEnum("interval", interval);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/indexPriceKlines", HitoBitExchange.RateLimiter.FuturesRest, requestWeight);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitFuturesUsdtKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region Asset index

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesAssetIndex>>> GetAssetIndexesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/assetIndex", HitoBitExchange.RateLimiter.FuturesRest, 10);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesAssetIndex>>(request, null, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesAssetIndex>> GetAssetIndexAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/assetIndex", HitoBitExchange.RateLimiter.FuturesRest, 1);
            return await _baseClient.SendAsync<HitoBitFuturesAssetIndex>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Basis

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesBasis>>> GetBasisAsync(string symbol, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "pair", symbol }
            };
            parameters.AddEnum("contractType", contractType);
            parameters.AddEnum("period", period);
            parameters.AddOptional("limit", limit ?? 30);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/basis", HitoBitExchange.RateLimiter.FuturesRest, 0);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesBasis>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Convert Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesConvertSymbol>>> GetConvertSymbolsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("fromAsset", fromAsset);
            parameters.AddOptional("toAsset", toAsset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v1/convert/exchangeInfo", HitoBitExchange.RateLimiter.FuturesRest, 20, false);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitFuturesConvertSymbol>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
