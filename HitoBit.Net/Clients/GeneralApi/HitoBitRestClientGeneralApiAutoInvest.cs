using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.AutoInvest;
using HitoBit.Net.Objects.Models.Spot.Loans;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc />
    internal class HitoBitRestClientGeneralApiAutoInvest : IHitoBitRestClientGeneralApiAutoInvest
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiAutoInvest(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Source And Target Assets

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestAssets>> GetSourceAndTargetAssetsAsync(CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/all/asset", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestAssets>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Source Assets

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestSourceAssets>> GetSourceAssetsAsync(string usageType, string? targetAsset = null, bool? flexibleAllowedToUse = null, string? sourceType = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("targetAsset", targetAsset);
            parameters.Add("usageType", usageType);
            parameters.AddOptional("flexibleAllowedToUse", flexibleAllowedToUse);
            parameters.AddOptional("sourceType", sourceType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/source-asset/list", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestSourceAssets>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Target Assets

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestTargetAssets>> GetTargetAssetsAsync(string? targetAsset = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("targetAsset", targetAsset);
            parameters.AddOptional("page", page);
            parameters.AddOptional("pageSize", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/target-asset/list", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestTargetAssets>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Target Asset Rois

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitAutoInvestRoi>>> GetTargetAssetRoisAsync(string asset, AutoInvestRoiType roiType, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("targetAsset", asset);
            parameters.AddEnum("hisRoiType", roiType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/target-asset/roi/list", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitAutoInvestRoi>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Info

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestIndex>> GetIndexInfoAsync(string indexId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("indexId", indexId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/index/info", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestIndex>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Plans

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestPlan>> GetPlansAsync(AutoInvestPlanType planType, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("planType", planType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/plan/list", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestPlan>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region One Time Transaction

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestTradeResult>> OneTimeTransactionAsync(string sourceType, string requestId, decimal subscriptionQuantity, string sourceAsset, bool flexibleAllowedToUse, long indexId, Dictionary<string, decimal> subscriptionDetails, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("sourceType", sourceType);
            parameters.Add("requestId", requestId);
            parameters.Add("subscriptionAmount", subscriptionQuantity);
            parameters.Add("sourceAsset", sourceAsset);
            parameters.Add("flexibleAllowedToUse", flexibleAllowedToUse);
            parameters.Add("indexid", indexId);
            parameters.Add("details", subscriptionDetails.Select(x => new
            {
                targetAsset = x.Key,
                percentage = x.Value
            }).ToList());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/sapi/v1/lending/auto-invest/one-off", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestTradeResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Edit Plan Status

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestEditStatusResult>> EditPlanStatusAsync(long planId, AutoInvestPlanStatus status, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("planId", planId);
            parameters.AddEnum("status", status);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/sapi/v1/lending/auto-invest/plan/edit-status", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestEditStatusResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Edit Plan

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestEditResult>> EditPlanAsync(string planId, decimal subscriptionQuantity, AutoInvestSubscriptionCycle subscriptionCycle, string sourceAsset, Dictionary<string, decimal> subscriptionDetails, int? subscriptionStartDay = null, string? subscriptionStartWeekday = null, int? subscriptionStartTime = null, bool? flexibleAllowedToUse = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("planId", planId);
            parameters.Add("subscriptionAmount", subscriptionQuantity);
            parameters.AddEnum("subscriptionCycle", subscriptionCycle);
            parameters.Add("sourceAsset", sourceAsset);
            parameters.Add("details", subscriptionDetails.Select(x => new
            {
                targetAsset = x.Key,
                percentage = x.Value
            }).ToList());
            parameters.AddOptional("subscriptionStartDay", subscriptionStartDay);
            parameters.AddOptional("subscriptionStartWeekday", subscriptionStartWeekday);
            parameters.AddOptional("subscriptionStartTime", subscriptionStartTime);
            parameters.AddOptional("flexibleAllowedToUse", flexibleAllowedToUse);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/sapi/v1/lending/auto-invest/plan/edit", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestEditResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Redeem Index Linked Plan

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestRedemptionResult>> RedeemIndexLinkedPlanAsync(string indexId, string requestId, int redemptionPercentage, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("indexId", indexId);
            parameters.Add("requestId", requestId);
            parameters.Add("redemptionPercentage", redemptionPercentage);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/sapi/v1/lending/auto-invest/redeem", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestRedemptionResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Subscription Transaction History

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestPlanTransactions>> GetSubscriptionTransactionHistoryAsync(long? planId = null, DateTime? startTime = null, DateTime? endTime = null, string? targetAsset = null, AutoInvestPlanType? planType = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("planId", planId);
            parameters.AddOptionalMillisecondsString("startTime", startTime);
            parameters.AddOptionalMillisecondsString("endTime", endTime);
            parameters.AddOptional("targetAsset", targetAsset);
            parameters.AddOptionalEnum("planType", planType);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/history/list", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestPlanTransactions>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get One Time Transaction Status

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestOneTimeTransactionStatus>> GetOneTimeTransactionStatusAsync(long transactionId, string requestId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("transactionId", transactionId);
            parameters.Add("requestId", requestId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/one-off/status", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestOneTimeTransactionStatus>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Create Plan

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestTradeResult>> CreatePlanAsync(string sourceType, AutoInvestPlanType planType, decimal subscriptionQuantity, AutoInvestSubscriptionCycle subscriptionCycle, int subscriptionStartTime, string sourceAsset, Dictionary<string, decimal> subscriptionDetails, string? requestId = null, int? subscriptionStartDay = null, string? subscriptionStartWeekday = null, bool? flexibleAllowedToUse = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("sourceType", sourceType);
            parameters.AddEnum("planType", planType);
            parameters.Add("subscriptionAmount", subscriptionQuantity);
            parameters.AddEnum("subscriptionCycle", subscriptionCycle);
            parameters.Add("subscriptionStartTime", subscriptionStartTime);
            parameters.Add("sourceAsset", sourceAsset);
            parameters.Add("details", subscriptionDetails.Select(x => new
            {
                targetAsset = x.Key,
                percentage = x.Value
            }).ToList());
            parameters.AddOptional("requestId", requestId);
            parameters.AddOptional("subscriptionStartDay", subscriptionStartDay);
            parameters.AddOptional("subscriptionStartWeekday", subscriptionStartWeekday);
            parameters.AddOptional("flexibleAllowedToUse", flexibleAllowedToUse);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "/sapi/v1/lending/auto-invest/plan/add", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestTradeResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Linked Plan Redemption History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitAutoInvestRedemption>>> GetIndexLinkedPlanRedemptionHistoryAsync(long requestId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, string? asset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("requestId", requestId);
            parameters.AddOptionalMillisecondsString("", startTime);
            parameters.AddOptionalMillisecondsString("", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptional("asset", asset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/redeem/history", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitAutoInvestRedemption>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Plan Holdings

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestPlanHoldings>> GetPlanHoldingsAsync(long? planId = null, string? requestId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("planId", planId);
            parameters.AddOptional("requestId", requestId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/plan/id", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestPlanHoldings>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Linked Plan Position Details

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoInvestIndexPlanPosition>> GetIndexLinkedPlanPositionDetailsAsync(long indexId, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("indexId", indexId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/index/user-summary", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitAutoInvestIndexPlanPosition>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Index Linked Plan Rebalance History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitAutoInvestRebalanceInfo>>> GetIndexLinkedPlanRebalanceHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalMillisecondsString("startTime", startTime);
            parameters.AddOptionalMillisecondsString("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/sapi/v1/lending/auto-invest/rebalance/history", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<IEnumerable<HitoBitAutoInvestRebalanceInfo>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
