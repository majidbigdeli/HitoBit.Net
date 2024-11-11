using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.SimpleEarn;
using CryptoExchange.Net.RateLimiting.Guards;

namespace HitoBit.Net.Clients.GeneralApi
{
    internal class HitoBitRestClientGeneralApiSimpleEarn: IHitoBitRestClientGeneralApiSimpleEarn
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiSimpleEarn(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Flexible Products

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleProduct>>> GetFlexibleProductsAsync(string? asset = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("asset", asset);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/list", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleProduct>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Locked Products

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnLockedProduct>>> GetLockedProductsAsync(string? asset = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("asset", asset);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);
            
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/locked/list", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnLockedProduct>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Subscribe Flexible Product

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnPurchase>> SubscribeFlexibleProductAsync(string productId, decimal quantity, bool? autoSubscribe = null, AccountSource? sourceAccount = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "productId", productId },
                { "amount", quantity }
            };
            parameters.AddOptional("autoSubscribe", autoSubscribe);
            parameters.AddOptionalEnum("sourceAccount", sourceAccount);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/simple-earn/flexible/subscribe", HitoBitExchange.RateLimiter.EndpointLimit, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(3), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<HitoBitSimpleEarnPurchase>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Subscribe Locked Product

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnPurchase>> SubscribeLockedProductAsync(string projectId, decimal quantity, bool? autoSubscribe = null, AccountSource? sourceAccount = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "projectId", projectId },
                { "amount", quantity }
            };
            parameters.AddOptional("autoSubscribe", autoSubscribe);
            parameters.AddOptionalEnum("sourceAccount", sourceAccount);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/simple-earn/locked/subscribe", HitoBitExchange.RateLimiter.EndpointLimit, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(3), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<HitoBitSimpleEarnPurchase>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Redeem Flexible Product

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnRedemption>> RedeemFlexibleProductAsync(string productId, bool? redeemAll = null, decimal? quantity = null, AccountSource? destinationAccount = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "productId", productId },
            };
            parameters.AddOptional("redeemAll", redeemAll);
            parameters.AddOptional("amount", quantity);
            parameters.AddOptionalEnum("destAccount", destinationAccount);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/simple-earn/flexible/redeem", HitoBitExchange.RateLimiter.EndpointLimit, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(3), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<HitoBitSimpleEarnRedemption>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Redeem Locked Product

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnRedemption>> RedeemLockedProductAsync(string positionId, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "positionId", positionId },
            };
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/simple-earn/locked/redeem", HitoBitExchange.RateLimiter.EndpointLimit, 1, true,
                limitGuard: new SingleLimitGuard(1, TimeSpan.FromSeconds(3), RateLimitWindowType.Sliding, keySelector: SingleLimitGuard.PerApiKey));
            return await _baseClient.SendAsync<HitoBitSimpleEarnRedemption>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Flexible Product Positions

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnFlexiblePosition>>> GetFlexibleProductPositionsAsync(string? asset = null, string? productId = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("asset", asset);
            parameters.AddOptional("productId", productId);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);
            
            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/position", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnFlexiblePosition>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Locked Product Positions

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnLockedPosition>>> GetLockedProductPositionsAsync(string? asset = null, string? positionId = null, string? projectId = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("asset", asset);
            parameters.AddOptional("positionId", positionId);
            parameters.AddOptional("projectId", projectId);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/locked/position", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnLockedPosition>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnAccount>> GetAccountAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/account", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitSimpleEarnAccount>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Flexible Subscription Records

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleRecord>>> GetFlexibleSubscriptionRecordsAsync(string? productId = null, string? purchaseId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("productId", productId);
            parameters.AddOptional("purchaseId", purchaseId);
            parameters.AddOptional("asset", asset);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/history/subscriptionRecord", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Locked Subscription Records

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnLockedRecord>>> GetLockedSubscriptionRecordsAsync(string? purchaseId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("purchaseId", purchaseId);
            parameters.AddOptional("asset", asset);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/locked/history/subscriptionRecord", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnLockedRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Flexible Redemption Records

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleRedemptionRecord>>> GetFlexibleRedemptionRecordsAsync(string? productId = null, string? redeemId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("productId", productId);
            parameters.AddOptional("redeemId", redeemId);
            parameters.AddOptional("asset", asset);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/history/redemptionRecord", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleRedemptionRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Locked Redemption Records

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnLockedRedemptionRecord>>> GetLockedRedemptionRecordsAsync(string? positionId = null, string? redeemId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("positionId", positionId);
            parameters.AddOptional("redeemId", redeemId);
            parameters.AddOptional("asset", asset);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/locked/history/redemptionRecord", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnLockedRedemptionRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Flexible Reward Records

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleRewardRecord>>> GetFlexibleRewardRecordsAsync(RewardType type, string? productId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddEnum("type", type);
            parameters.AddOptional("productId", productId);
            parameters.AddOptional("asset", asset);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/history/rewardsRecord", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnFlexibleRewardRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Locked Reward Records

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnLockedRewardRecord>>> GetLockedRewardRecordsAsync(string? positionId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("positionId", positionId);
            parameters.AddOptional("asset", asset);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/locked/history/rewardsRecord", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnLockedRewardRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Flexible Auto Subscribe

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnResult>> SetFlexibleAutoSubscribeAsync(string productId, bool autoSubscribe, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "productId", productId },
                { "autoSubscribe", autoSubscribe }
            };
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/simple-earn/flexible/setAutoSubscribe", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitSimpleEarnResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Locked Auto Subscribe

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnResult>> SetLockedAutoSubscribeAsync(string positionId, bool autoSubscribe, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "positionId", positionId },
                { "autoSubscribe", autoSubscribe }
            };
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/simple-earn/locked/setAutoSubscribe", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitSimpleEarnResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Flexible Personal Quota Left

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnPersonalQuotaLeft>> GetFlexiblePersonalQuotaLeftAsync(string productId, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "productId", productId }
            };
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/personalLeftQuota", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitSimpleEarnPersonalQuotaLeft>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Locked Personal Quota Left

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnPersonalQuotaLeft>> GetLockedPersonalQuotaLeftAsync(string projectId, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "projectId", projectId }
            };
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/locked/personalLeftQuota", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitSimpleEarnPersonalQuotaLeft>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Flexible Subscription Preview

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSimpleEarnFlexiblePreview>> GetFlexibleSubscriptionPreviewAsync(string productId, decimal quantity, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "productId", productId },
                { "amount", quantity }
            };
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/subscriptionPreview", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitSimpleEarnFlexiblePreview>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Locked Subscription Preview

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSimpleEarnLockedPreview>>> GetLockedSubscriptionPreviewAsync(string projectId, decimal quantity, bool? autoSubscribe = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "projectId", projectId },
                { "amount", quantity }
            };
            parameters.AddOptional("autoSubscribe", autoSubscribe);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/locked/subscriptionPreview", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitSimpleEarnLockedPreview>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Rate History

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnRateRecord>>> GetRateHistoryAsync(string productId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("productId", productId);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/history/rateHistory", HitoBitExchange.RateLimiter.SpotRestIp, 150, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnRateRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Collateral Records

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSimpleEarnCollateralRecord>>> GetCollateralRecordsAsync(string productId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("productId", productId);
            parameters.AddOptionalMilliseconds("startTime", startTime);
            parameters.AddOptionalMilliseconds("endTime", endTime);
            parameters.AddOptional("current", page);
            parameters.AddOptional("size", pageSize);
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/simple-earn/flexible/history/collateralRecord", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitQueryRecords<HitoBitSimpleEarnCollateralRecord>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
