using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.Mining;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc />
    internal class HitoBitRestClientGeneralApiMining : IHitoBitRestClientGeneralApiMining
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiMining(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }


        #region Acquiring CoinName
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMiningCoin>>> GetMiningCoinListAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/pub/coinList", HitoBitExchange.RateLimiter.SpotRestIp);
            var result = await _baseClient.SendAsync<HitoBitResult<IEnumerable<HitoBitMiningCoin>>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<IEnumerable<HitoBitMiningCoin>>(default);

            if (result.Data?.Code != 0)
                return result.AsError<IEnumerable<HitoBitMiningCoin>>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion Acquiring CoinName

        #region Acquiring Algorithm 
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMiningAlgorithm>>> GetMiningAlgorithmListAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/pub/algoList", HitoBitExchange.RateLimiter.SpotRestIp);
            var result = await _baseClient.SendAsync<HitoBitResult<IEnumerable<HitoBitMiningAlgorithm>>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<IEnumerable<HitoBitMiningAlgorithm>>(default);

            if (result.Data?.Code != 0)
                return result.AsError<IEnumerable<HitoBitMiningAlgorithm>>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Request Detail Miner List

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMinerDetails>>> GetMinerDetailsAsync(string algorithm, string userName, string workerName, CancellationToken ct = default)
        {
            algorithm.ValidateNotNull(nameof(algorithm));
            userName.ValidateNotNull(nameof(userName));
            workerName.ValidateNotNull(nameof(workerName));

            var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName},
                {"workerName", workerName}
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/worker/detail", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<IEnumerable<HitoBitMinerDetails>>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<IEnumerable<HitoBitMinerDetails>>(default);

            if (result.Data?.Code != 0)
                return result.AsError<IEnumerable<HitoBitMinerDetails>>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Request Miner List
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitMinerList>> GetMinerListAsync(string algorithm, string userName, int? page = null, bool? sortAscending = null, string? sortColumn = null, MinerStatus? workerStatus = null, CancellationToken ct = default)
        {
            algorithm.ValidateNotNull(nameof(algorithm));
            userName.ValidateNotNull(nameof(userName));

            var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("sortAscending", sortAscending == null ? null : sortAscending == true ? "1" : "0");
            parameters.AddOptionalParameter("sortColumn", sortColumn);
            parameters.AddOptionalEnum("workerStatus", workerStatus);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/worker/list", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<HitoBitMinerList>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitMinerList>(default);

            if (result.Data?.Code != 0)
                return result.AsError<HitoBitMinerList>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Revenue List
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitRevenueList>> GetMiningRevenueListAsync(string algorithm, string userName, string? coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            algorithm.ValidateNotNull(nameof(algorithm));
            userName.ValidateNotNull(nameof(userName));

            var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("coin", coin);
            parameters.AddOptionalParameter("startDate", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endDate", DateTimeConverter.ConvertToMilliseconds(endDate));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/payment/list", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<HitoBitRevenueList>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitRevenueList>(default);

            if (result.Data?.Code != 0)
                return result.AsError<HitoBitRevenueList>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Other Revenue List
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitOtherRevenueList>> GetMiningOtherRevenueListAsync(string algorithm, string userName, string? coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            algorithm.ValidateNotNull(nameof(algorithm));
            userName.ValidateNotNull(nameof(userName));

            var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("coin", coin);
            parameters.AddOptionalParameter("startDate", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endDate", DateTimeConverter.ConvertToMilliseconds(endDate));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/payment/other", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<HitoBitOtherRevenueList>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitOtherRevenueList>(default);

            if (result.Data?.Code != 0)
                return result.AsError<HitoBitOtherRevenueList>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }
        #endregion

        #region Statistics list
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitMiningStatistic>> GetMiningStatisticsAsync(string algorithm, string userName, CancellationToken ct = default)
        {
            algorithm.ValidateNotNull(nameof(algorithm));
            userName.ValidateNotNull(nameof(userName));

            var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/statistics/user/status", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<HitoBitMiningStatistic>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitMiningStatistic>(default);

            if (result.Data?.Code != 0)
                return result.AsError<HitoBitMiningStatistic>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }
        #endregion

        #region Account List
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMiningAccount>>> GetMiningAccountListAsync(string algorithm, string userName, CancellationToken ct = default)
        {
            algorithm.ValidateNotNull(nameof(algorithm));
            userName.ValidateNotNull(nameof(userName));

            var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/statistics/user/list", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<IEnumerable<HitoBitMiningAccount>>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<IEnumerable<HitoBitMiningAccount>>(default);

            if (result.Data?.Code != 0)
                return result.AsError<IEnumerable<HitoBitMiningAccount>>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }
        #endregion

        #region Hashrate Resale List
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitHashrateResaleList>> GetHashrateResaleListAsync(int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/hash-transfer/config/details/list", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<HitoBitHashrateResaleList>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitHashrateResaleList>(default);

            if (result.Data?.Code != 0)
                return result.AsError<HitoBitHashrateResaleList>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Hashrate Resale Details
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitHashrateResaleDetails>> GetHashrateResaleDetailsAsync(int configId, string userName, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            userName.ValidateNotNull(nameof(userName));

            var parameters = new ParameterCollection()
            {
                { "configId", configId.ToString(CultureInfo.InvariantCulture) },
                { "userName", userName }
            };

            parameters.AddOptionalParameter("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/hash-transfer/profit/details", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<HitoBitHashrateResaleDetails>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitHashrateResaleDetails>(default);

            if (result.Data?.Code != 0)
                result.AsError<HitoBitHashrateResaleDetails>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Hashrate Resale Request

        /// <inheritdoc />
        public async Task<WebCallResult<int>> PlaceHashrateResaleRequestAsync(string userName, string algorithm, DateTime startDate, DateTime endDate, string toUser, decimal hashRate, CancellationToken ct = default)
        {
            userName.ValidateNotNull(nameof(userName));
            algorithm.ValidateNotNull(nameof(algorithm));
            toUser.ValidateNotNull(nameof(toUser));

            var parameters = new ParameterCollection()
            {
                { "userName", userName },
                { "algo", algorithm },
                { "startDate", DateTimeConverter.ConvertToMilliseconds(startDate)! },
                { "endDate", DateTimeConverter.ConvertToMilliseconds(endDate)! },
                { "toPoolUser", toUser },
                { "hashRate", hashRate }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/mining/hash-transfer/config", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<int>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<int>(default);

            if (result.Data?.Code != 0)
                return result.AsError<int>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Cancel Hashrate Resale Configuration

        /// <inheritdoc />
        public async Task<WebCallResult<bool>> CancelHashrateResaleRequestAsync(int configId, string userName, CancellationToken ct = default)
        {
            userName.ValidateNotNull(nameof(userName));

            var parameters = new ParameterCollection()
            {
                { "configId", configId },
                { "userName", userName }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/mining/hash-transfer/config/cancel", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<bool>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<bool>(default);

            if (result.Data?.Code != 0)
                return result.AsError<bool>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Get Mining Account Earnings
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitMiningEarnings>> GetMiningAccountEarningsAsync(string algo, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "algo", algo }
            };
            parameters.AddOptionalParameter("startDate", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endDate", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/mining/payment/uid", HitoBitExchange.RateLimiter.SpotRestIp, 5, true);
            var result = await _baseClient.SendAsync<HitoBitResult<HitoBitMiningEarnings>>(request, parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitMiningEarnings>(default);

            if (result.Data?.Code != 0)
                return result.AsError<HitoBitMiningEarnings>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion Acquiring CoinName
    }
}
