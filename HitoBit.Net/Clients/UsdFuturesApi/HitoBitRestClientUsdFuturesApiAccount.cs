using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures;

namespace HitoBit.Net.Clients.UsdFuturesApi
{
    /// <inheritdoc />
    internal class HitoBitRestClientUsdFuturesApiAccount : IHitoBitRestClientUsdFuturesApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new();
        private readonly HitoBitRestClientUsdFuturesApi _baseClient;

        internal HitoBitRestClientUsdFuturesApiAccount(HitoBitRestClientUsdFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Change Position Mode

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitResult>> ModifyPositionModeAsync(bool dualPositionSide, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "dualSidePosition", dualPositionSide.ToString().ToLower() }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/positionSide/dual", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<HitoBitResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Current Position Mode

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesPositionMode>> GetPositionModeAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/positionSide/dual", HitoBitExchange.RateLimiter.FuturesRest, 30, true);
            return await _baseClient.SendAsync<HitoBitFuturesPositionMode>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Change Initial Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesInitialLeverageChangeResult>> ChangeInitialLeverageAsync(string symbol, int leverage, long? receiveWindow = null, CancellationToken ct = default)
        {
            leverage.ValidateIntBetween(nameof(leverage), 1, 125);
            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "leverage", leverage }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/leverage", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<HitoBitFuturesInitialLeverageChangeResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Change Margin Type

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesChangeMarginTypeResult>> ChangeMarginTypeAsync(string symbol, FuturesMarginType marginType, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
            };
            parameters.AddEnum("marginType", marginType);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/marginType", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<HitoBitFuturesChangeMarginTypeResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Modify Isolated Position Margin

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal quantity, FuturesMarginChangeDirectionType type, PositionSide? positionSide = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            };
            parameters.AddEnum("type", type);
            parameters.AddOptionalEnum("positionSide", positionSide);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/positionMargin", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<HitoBitFuturesPositionMarginResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Postion Margin Change History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, FuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddOptionalEnum("type", type);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/positionMargin/history", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesMarginChangeHistoryResult>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Income History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, string? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("incomeType", incomeType);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/income", HitoBitExchange.RateLimiter.FuturesRest, 30, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesIncomeHistory>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Notional and Leverage Brackets

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesSymbolBracket>>> GetBracketsAsync(string? symbolOrPair = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbolOrPair);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/leverageBracket", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesSymbolBracket>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Position ADL Quantile Estimations

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            if (symbol == null)
            {
                var request1 = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/adlQuantile", HitoBitExchange.RateLimiter.FuturesRest, 5, true);
                return await _baseClient.SendAsync<IEnumerable<HitoBitFuturesQuantileEstimation>>(request1, parameters, ct).ConfigureAwait(false);
            }

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/adlQuantile", HitoBitExchange.RateLimiter.FuturesRest, 5, true);
            var result = await _baseClient.SendAsync<HitoBitFuturesQuantileEstimation>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<HitoBitFuturesQuantileEstimation>>(null);

            return result.As<IEnumerable<HitoBitFuturesQuantileEstimation>>(new[] { result.Data });
        }

        #endregion

        #region Start User Data Stream
        /// <inheritdoc />
        public async Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/listenKey", HitoBitExchange.RateLimiter.FuturesRest, 1);
            var result = await _baseClient.SendAsync<Objects.Models.Spot.HitoBitListenKey>(request, null, ct).ConfigureAwait(false);
            return result.As(result.Data?.ListenKey!);
        }

        #endregion

        #region Keepalive User Data Stream

        /// <inheritdoc />
        public async Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Put, "fapi/v1/listenKey", HitoBitExchange.RateLimiter.FuturesRest, 1);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Close User Data Stream

        /// <inheritdoc />
        public async Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));
            var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Delete, "fapi/v1/listenKey", HitoBitExchange.RateLimiter.FuturesRest, 1);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Info

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesAccountInfo>> GetAccountInfoV2Async(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v2/account", HitoBitExchange.RateLimiter.FuturesRest, 10, true);
            return await _baseClient.SendAsync<HitoBitFuturesAccountInfo>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Info

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesAccountInfoV3>> GetAccountInfoV3Async(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v3/account", HitoBitExchange.RateLimiter.FuturesRest, 5, true);
            var result = await _baseClient.SendAsync<HitoBitFuturesAccountInfoV3>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Future Account Balance

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitUsdFuturesAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v3/balance", HitoBitExchange.RateLimiter.FuturesRest, 5, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitUsdFuturesAccountBalance>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Multi assets mode

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesMultiAssetMode>> GetMultiAssetsModeAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/multiAssetsMargin", HitoBitExchange.RateLimiter.FuturesRest, 30, true);
            return await _baseClient.SendAsync<HitoBitFuturesMultiAssetMode>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitResult>> SetMultiAssetsModeAsync(bool enabled, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "multiAssetsMargin", enabled.ToString() }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/multiAssetsMargin", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<HitoBitResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Position Information

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitPositionDetailsUsdt>>> GetPositionInformationAsync(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v2/positionRisk", HitoBitExchange.RateLimiter.FuturesRest, 10, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitPositionDetailsUsdt>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Trading status
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/apiTradingStatus", HitoBitExchange.RateLimiter.FuturesRest, 10, true);
            return await _baseClient.SendAsync<HitoBitFuturesTradingStatus>(request, parameters, ct).ConfigureAwait(false);

        }
        #endregion

        #region Future Account User Commission Rate
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol}
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/commissionRate", HitoBitExchange.RateLimiter.FuturesRest, 20, true);
            return await _baseClient.SendAsync<HitoBitFuturesAccountUserCommissionRate>(request, parameters, ct).ConfigureAwait(false); 
        }
        #endregion

        #region Get download id for transaction history
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "startTime", DateTimeConverter.ConvertToMilliseconds(startTime) },
                { "endTime", DateTimeConverter.ConvertToMilliseconds(endTime) },
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/income/asyn", HitoBitExchange.RateLimiter.FuturesRest, 1500, true);
            return await _baseClient.SendAsync<HitoBitFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Download transaction history
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "downloadId", downloadId }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/income/asyn/id", HitoBitExchange.RateLimiter.FuturesRest, 10, true);
            return await _baseClient.SendAsync<HitoBitFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Get download id for transaction history
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "startTime", DateTimeConverter.ConvertToMilliseconds(startTime) },
                { "endTime", DateTimeConverter.ConvertToMilliseconds(endTime) },
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/order/asyn", HitoBitExchange.RateLimiter.FuturesRest, 1500, true);
            return await _baseClient.SendAsync<HitoBitFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Download order history
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "downloadId", downloadId }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/order/asyn/id", HitoBitExchange.RateLimiter.FuturesRest, 10, true);
            return await _baseClient.SendAsync<HitoBitFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Get download id for trade history
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "startTime", DateTimeConverter.ConvertToMilliseconds(startTime) },
                { "endTime", DateTimeConverter.ConvertToMilliseconds(endTime) },
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/trade/asyn", HitoBitExchange.RateLimiter.FuturesRest, 1500, true);
            return await _baseClient.SendAsync<HitoBitFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Download trade history
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "downloadId", downloadId }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/trade/asyn/id", HitoBitExchange.RateLimiter.FuturesRest, 10, true);
            return await _baseClient.SendAsync<HitoBitFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Get Order Rate Limit

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitRateLimit>>> GetOrderRateLimitAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/rateLimit/order", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitRateLimit>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get BNB Burn Status

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBnbBurnStatus>> GetBnbBurnStatusAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/feeBurn", HitoBitExchange.RateLimiter.FuturesRest, 30, true);
            return await _baseClient.SendAsync<HitoBitBnbBurnStatus>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set BNB Burn Status

        /// <inheritdoc />
        public async Task<WebCallResult> SetBnbBurnStatusAsync(bool feeBurn, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "feeBurn", feeBurn.ToString() }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/feeBurn", HitoBitExchange.RateLimiter.FuturesRest, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbol Configuration

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSymbolConfiguration>>> GetSymbolConfigurationAsync(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v1/symbolConfig", HitoBitExchange.RateLimiter.FuturesRest, 5, true);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitSymbolConfiguration>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Account Configuration

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitFuturesAccountConfiguration>> GetAccountConfigurationAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v1/accountConfig", HitoBitExchange.RateLimiter.FuturesRest, 5, true);
            var result = await _baseClient.SendAsync<HitoBitFuturesAccountConfiguration>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
