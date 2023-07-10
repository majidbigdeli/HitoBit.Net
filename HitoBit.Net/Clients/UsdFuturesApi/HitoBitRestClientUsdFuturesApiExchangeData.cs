using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects.Models.Futures;
using HitoBit.Net.Objects.Models.Spot;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HitoBit.Net.Clients.UsdFuturesApi
{
    /// <inheritdoc />
    public class HitoBitRestClientUsdFuturesApiExchangeData : IHitoBitRestClientUsdFuturesApiExchangeData
    {
        private const string orderBookEndpoint = "depth";
        private const string aggregatedTradesEndpoint = "aggTrades";
        private const string markPriceKlinesEndpoint = "markPriceKlines";

        private const string fundingRateHistoryEndpoint = "fundingRate";

        private const string topLongShortAccountRatioEndpoint = "topLongShortAccountRatio";
        private const string topLongShortPositionRatioEndpoint = "topLongShortPositionRatio";
        private const string globalLongShortAccountRatioEndpoint = "globalLongShortAccountRatio";

        private const string recentTradesEndpoint = "trades";
        private const string historicalTradesEndpoint = "historicalTrades";
        private const string markPriceEndpoint = "premiumIndex";
        private const string price24HEndpoint = "ticker/24hr";
        private const string allPricesEndpoint = "ticker/price";
        private const string bookPricesEndpoint = "ticker/bookTicker";
        private const string openInterestEndpoint = "openInterest";
        private const string openInterestHistoryEndpoint = "openInterestHist";
        private const string takerBuySellVolumeRatioEndpoint = "takerlongshortRatio";
        private const string compositeIndexapi = "indexInfo";
        private const string klinesEndpoint = "klines";
        private const string continuousContractKlineEndpoint = "continuousKlines";
        private const string indexPriceKlinesKlineEndpoint = "indexPriceKlines";
        private const string assetIndexEndpoint = "assetIndex";

        private const string pingEndpoint = "ping";
        private const string checkTimeEndpoint = "time";
        private const string exchangeInfoEndpoint = "exchangeInfo";

        private const string api = "fapi";
        private const string publicVersion = "1";
        private const string tradingDataapi = "futures/data";

        private readonly ILogger _logger;

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
            var result = await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(pingEndpoint, api, "1"), HttpMethod.Get, ct).ConfigureAwait(false);
            sw.Stop();
            return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
        }

        #endregion

        #region Check Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(bool resetAutoTimestamp = false, CancellationToken ct = default)
        {
            var url = _baseClient.GetUrl(checkTimeEndpoint, api, "1");
            var result = await _baseClient.SendRequestInternal<HitoBitCheckTime>(url, HttpMethod.Get, ct, ignoreRateLimit: true).ConfigureAwait(false);
            return result.As(result.Data?.ServerTime ?? default);
        }

        #endregion

        #region Exchange Information

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesUsdtExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
        {
            var exchangeInfoResult = await _baseClient.SendRequestInternal<HitoBitFuturesUsdtExchangeInfo>(_baseClient.GetUrl(exchangeInfoEndpoint, api, "1"), HttpMethod.Get, ct).ConfigureAwait(false);
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
            var parameters = new Dictionary<string, object> { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
            var result = await _baseClient.SendRequestInternal<HitoBitFuturesOrderBook>(_baseClient.GetUrl(orderBookEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object> { { "symbol", symbol } };
            parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitAggregatedTrade>>(_baseClient.GetUrl(aggregatedTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 20).ConfigureAwait(false);
        }

        #endregion

        #region Get Funding Rate History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesFundingRateHistory>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);
            var parameters = new Dictionary<string, object> {
                { "symbol", symbol }
            };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesFundingRateHistory>>(_baseClient.GetUrl(fundingRateHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Top Trader Long/Short Ratio (Accounts)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var url = _baseClient.GetUrl(topLongShortAccountRatioEndpoint, tradingDataapi);
            var parameters = new Dictionary<string, object> {
                { url.ToString().Contains("dapi") ? "pair": "symbol", symbolPair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesLongShortRatio>>(url, HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Top Trader Long/Short Ratio (Positions)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var url = _baseClient.GetUrl(topLongShortPositionRatioEndpoint, tradingDataapi);
            var parameters = new Dictionary<string, object> {
                { url.ToString().Contains("dapi") ? "pair": "symbol", symbolPair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesLongShortRatio>>(url, HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Global Long/Short Ratio (Accounts)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var url = _baseClient.GetUrl(globalLongShortAccountRatioEndpoint, tradingDataapi);
            var parameters = new Dictionary<string, object> {
                { url.ToString().Contains("dapi") ? "pair": "symbol", symbolPair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesLongShortRatio>>(url, HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Mark Price Kline/Candlestick Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMarkIndexKline>>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);

            var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };

            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitMarkIndexKline>>(_baseClient.GetUrl(markPriceKlinesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
        }

        #endregion

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new Dictionary<string, object> { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            var result = await _baseClient.SendRequestInternal<IEnumerable<HitoBitRecentTradeQuote>>(_baseClient.GetUrl(recentTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 5).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitRecentTrade>>(result.Data);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null,
            CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);
            var parameters = new Dictionary<string, object> { { "symbol", symbol } };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<IEnumerable<HitoBitRecentTradeQuote>>(_baseClient.GetUrl(historicalTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 20).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitRecentTrade>>(result.Data);
        }

        #region Mark Price

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesMarkPrice>> GetMarkPriceAsync(string symbol,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);

            return await _baseClient
                .SendRequestInternal<HitoBitFuturesMarkPrice>(
                    _baseClient.GetUrl(markPriceEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesMarkPrice>>> GetMarkPricesAsync(CancellationToken ct = default)
        {
            return await _baseClient
                .SendRequestInternal<IEnumerable<HitoBitFuturesMarkPrice>>(
                    _baseClient.GetUrl(markPriceEndpoint, api, publicVersion), HttpMethod.Get, ct)
                .ConfigureAwait(false);
        }
        #endregion

        #region 24h statistics
        /// <inheritdoc />
        public async Task<WebCallResult<IHitoBit24HPrice>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);

            var result = await _baseClient
                .SendRequestInternal<HitoBit24HPrice>(_baseClient.GetUrl(price24HEndpoint, api, publicVersion),
                    HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            return result.As<IHitoBit24HPrice>(result.Data);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBit24HPrice>>> GetTickersAsync(CancellationToken ct = default)
        {
            var result = await _baseClient
                .SendRequestInternal<IEnumerable<HitoBit24HPrice>>(_baseClient.GetUrl(price24HEndpoint, api, publicVersion),
                    HttpMethod.Get, ct, weight: 40).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBit24HPrice>>(result.Data);
        }
        #endregion

        #region Kline/Candlestick Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var result = await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesUsdtKline>>(_baseClient.GetUrl(klinesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region Symbol Order Book Ticker

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBookPrice>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);

            return await _baseClient.SendRequestInternal<HitoBitBookPrice>(_baseClient.GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBookPrice>>> GetBookPricesAsync(CancellationToken ct = default)
        {
            return await _baseClient
                .SendRequestInternal<IEnumerable<HitoBitBookPrice>>(
                    _baseClient.GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 2)
                .ConfigureAwait(false);
        }

        #endregion

        #region Open Interest

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };

            return await _baseClient.SendRequestInternal<HitoBitFuturesOpenInterest>(_baseClient.GetUrl(openInterestEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Open Interest History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesOpenInterestHistory>>> GetOpenInterestHistoryAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesOpenInterestHistory>>(_baseClient.GetUrl(openInterestHistoryEndpoint, tradingDataapi), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Taker Buy/Sell Volume Ratio

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesBuySellVolumeRatio>>(_baseClient.GetUrl(takerBuySellVolumeRatioEndpoint, tradingDataapi), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Composite index symbol information

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesCompositeIndexInfo>>> GetCompositeIndexInfoAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);
            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesCompositeIndexInfo>>(_baseClient.GetUrl(compositeIndexapi, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Get price

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitPrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };

            return await _baseClient.SendRequestInternal<HitoBitPrice>(_baseClient.GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitPrice>>> GetPricesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitPrice>>(_baseClient.GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 2).ConfigureAwait(false);
        }
        #endregion

        #region Continuous contract Kline Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetContinuousContractKlinesAsync(string pair, ContractType contractType, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) },
                { "contractType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)) }
            };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var result = await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesUsdtKline>>(_baseClient.GetUrl(continuousContractKlineEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region Index Price Kline Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetIndexPriceKlinesAsync(string pair, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1500);
            var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
            var result = await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesUsdtKline>>(_baseClient.GetUrl(indexPriceKlinesKlineEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
            return result.As<IEnumerable<IHitoBitKline>>(result.Data);
        }

        #endregion

        #region Asset index

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesAssetIndex>>> GetAssetIndexesAsync(CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFuturesAssetIndex>>(_baseClient.GetUrl(assetIndexEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 10).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesAssetIndex>> GetAssetIndexAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };
            return await _baseClient.SendRequestInternal<HitoBitFuturesAssetIndex>(_baseClient.GetUrl(assetIndexEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion
    }
}
