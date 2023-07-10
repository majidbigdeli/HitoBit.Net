using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.Mining;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc />
    public class HitoBitRestClientGeneralApiMining : IHitoBitRestClientGeneralApiMining
    {
        private const string coinListEndpoint = "mining/pub/coinList";
        private const string algorithmEndpoint = "mining/pub/algoList";
        private const string minerDetailsEndpoint = "mining/worker/detail";
        private const string minerListEndpoint = "mining/worker/list";
        private const string miningRevenueEndpoint = "mining/payment/list";
        private const string miningOtherRevenueEndpoint = "mining/payment/other";
        private const string miningStatisticsEndpoint = "mining/statistics/user/status";
        private const string miningAccountListEndpoint = "mining/statistics/user/list";
        private const string miningHashrateResaleListEndpoint = "mining/hash-transfer/config/details/list";
        private const string miningHashrateResaleDetailsEndpoint = "mining/hash-transfer/profit/details";
        private const string miningHashrateResaleRequest = "mining/hash-transfer/config";
        private const string miningHashrateResaleCancel = "mining/hash-transfer/config/cancel";

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiMining(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }


        #region Acquiring CoinName
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMiningCoin>>> GetMiningCoinListAsync(CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            var result = await _baseClient.SendRequestInternal<HitoBitResult<IEnumerable<HitoBitMiningCoin>>>(_baseClient.GetUrl(coinListEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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
            var parameters = new Dictionary<string, object>();
            var result = await _baseClient.SendRequestInternal<HitoBitResult<IEnumerable<HitoBitMiningAlgorithm>>>(_baseClient.GetUrl(algorithmEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName},
                {"workerName", workerName}
            };

            var result = await _baseClient.SendRequestInternal<HitoBitResult<IEnumerable<HitoBitMinerDetails>>>(_baseClient.GetUrl(minerDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("sortAscending", sortAscending == null ? null : sortAscending == true ? "1" : "0");
            parameters.AddOptionalParameter("sortColumn", sortColumn);
            parameters.AddOptionalParameter("workerStatus", workerStatus == null ? null : JsonConvert.SerializeObject(workerStatus, new MinerStatusConverter(false)));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitMinerList>>(_baseClient.GetUrl(minerListEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("coin", coin);
            parameters.AddOptionalParameter("startDate", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endDate", DateTimeConverter.ConvertToMilliseconds(endDate));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitRevenueList>>(_baseClient.GetUrl(miningRevenueEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("coin", coin);
            parameters.AddOptionalParameter("startDate", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endDate", DateTimeConverter.ConvertToMilliseconds(endDate));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitOtherRevenueList>>(_baseClient.GetUrl(miningOtherRevenueEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitMiningStatistic>>(_baseClient.GetUrl(miningStatisticsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

            var result = await _baseClient.SendRequestInternal<HitoBitResult<IEnumerable<HitoBitMiningAccount>>>(_baseClient.GetUrl(miningAccountListEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitHashrateResaleList>>(_baseClient.GetUrl(miningHashrateResaleListEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                { "configId", configId.ToString(CultureInfo.InvariantCulture) },
                { "userName", userName }
            };

            parameters.AddOptionalParameter("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitHashrateResaleDetails>>(_baseClient.GetUrl(miningHashrateResaleDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                { "userName", userName },
                { "algo", algorithm },
                { "startDate", DateTimeConverter.ConvertToMilliseconds(startDate)! },
                { "endDate", DateTimeConverter.ConvertToMilliseconds(endDate)! },
                { "toPoolUser", toUser },
                { "hashRate", hashRate }
            };

            var result = await _baseClient.SendRequestInternal<HitoBitResult<int>>(_baseClient.GetUrl(miningHashrateResaleRequest, "sapi", "1"), HttpMethod.Post, ct, parameters, true, weight: 5).ConfigureAwait(false);
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

            var parameters = new Dictionary<string, object>()
            {
                { "configId", configId },
                { "userName", userName }
            };

            var result = await _baseClient.SendRequestInternal<HitoBitResult<bool>>(_baseClient.GetUrl(miningHashrateResaleCancel, "sapi", "1"), HttpMethod.Post, ct, parameters, true, weight: 5).ConfigureAwait(false);
            if (!result.Success)
                return result.As<bool>(default);

            if (result.Data?.Code != 0)
                return result.AsError<bool>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion
    }
}
