using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc />
    public class HitoBitRestClientGeneralApiBrokerage : IHitoBitRestClientGeneralApiBrokerage
    {
        private const string brokerageApi = "sapi";
        private const string brokerageVersion = "1";

        private const string subAccountEndpoint = "broker/subAccount";
        private const string brokerAccountInfoEndpoint = "broker/info";
        private const string enableMarginForSubAccountEndpoint = "broker/subAccount/margin";
        private const string enableFuturesForSubAccountEndpoint = "broker/subAccount/futures";
        private const string enableLeverageTokenForSubAccountEndpoint = "broker/subAccount/blvt";
        private const string apiKeyEndpoint = "broker/subAccountApi";
        private const string apiKeyPermissionEndpoint = "broker/subAccountApi/permission";
        private const string apiKeyCommissionEndpoint = "broker/subAccountApi/commission";
        private const string apiKeyCommissionFuturesEndpoint = "broker/subAccountApi/commission/futures";
        private const string apiKeyCommissionCoinFuturesEndpoint = "broker/subAccountApi/commission/coinFutures";
        private const string apiKeyIpRestrictionEndpoint = "broker/subAccountApi/ipRestriction";
        private const string apiKeyIpRestrictionListEndpoint = "broker/subAccountApi/ipRestriction/ipList";
        private const string transferEndpoint = "broker/transfer";
        private const string transferUniversalEndpoint = "broker/universalTransfer";
        private const string transferFuturesEndpoint = "broker/transfer/futures";
        private const string rebatesRecentEndpoint = "broker/rebate/recentRecord";
        private const string rebatesHistoryEndpoint = "broker/rebate/historicalRecord";
        private const string rebatesFuturesHistoryEndpoint = "broker/rebate/futures/recentRecord";
        private const string changeBnbBurnForSubAccountSpotAndMarginEndpoint = "broker/subAccount/bnbBurn/spot";
        private const string changeBnbBurnForSubAccountMarginInterestEndpoint = "broker/subAccount/bnbBurn/marginInterest";
        private const string bnbBurnForSubAccountStatusEndpoint = "broker/subAccount/bnbBurn/status";
        private const string spotSummaryEndpoint = "broker/subAccount/spotSummary";
        private const string marginSummaryEndpoint = "broker/subAccount/marginSummary";
        private const string futuresSummaryEndpoint = "broker/subAccount/futuresSummary";
        private const string depositHistoryEndpoint = "broker/subAccount/depositHist";

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiBrokerage(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Sub accounts

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageSubAccountCreateResult>> CreateSubAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageSubAccountCreateResult>(_baseClient.GetUrl(subAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccount>>> GetSubAccountsAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("subAccountId", subAccountId);
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageSubAccount>>(_baseClient.GetUrl(subAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Sub accounts permissions

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageEnableMarginResult>> EnableMarginForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"margin", true},  // only true for now
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageEnableMarginResult>(_baseClient.GetUrl(enableMarginForSubAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageEnableFuturesResult>> EnableFuturesForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"futures", true},  // only true for now
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageEnableFuturesResult>(_baseClient.GetUrl(enableFuturesForSubAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageEnableLeverageTokenResult>> EnableLeverageTokenForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"blvt", true},  // only true for now
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageEnableLeverageTokenResult>(_baseClient.GetUrl(enableLeverageTokenForSubAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Sub accounts API keys

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageApiKeyCreateResult>> CreateApiKeyForSubAccountAsync(string subAccountId, bool isSpotTradingEnabled,
            bool? isMarginTradingEnabled = null, bool? isFuturesTradingEnabled = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"canTrade", isSpotTradingEnabled}
                             };
            parameters.AddOptionalParameter("marginTrade", isMarginTradingEnabled.ToString().ToLower());
            parameters.AddOptionalParameter("futuresTrade", isFuturesTradingEnabled.ToString().ToLower());
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageApiKeyCreateResult>(_baseClient.GetUrl(apiKeyEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<object> DeleteSubAccountApiKeyAsync(string subAccountId, string apiKey, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            apiKey.ValidateNotNull(nameof(apiKey));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(apiKeyEndpoint, brokerageApi, brokerageVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageSubAccountApiKey>> GetSubAccountApiKeyAsync(string subAccountId, string? apiKey = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                             };
            parameters.AddOptionalParameter("subAccountApiKey", apiKey);
            parameters.AddOptionalParameter("page", page);
            parameters.AddOptionalParameter("size", size);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageSubAccountApiKey>(_baseClient.GetUrl(apiKeyEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageSubAccountApiKey>> ChangeSubAccountApiKeyPermissionAsync(string subAccountId, string apiKey,
            bool isSpotTradingEnabled, bool isMarginTradingEnabled, bool isFuturesTradingEnabled, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            apiKey.ValidateNotNull(nameof(apiKey));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"canTrade", isSpotTradingEnabled.ToString().ToLower()},
                                 {"marginTrade", isMarginTradingEnabled.ToString().ToLower()},
                                 {"futuresTrade", isFuturesTradingEnabled.ToString().ToLower()}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageSubAccountApiKey>(_baseClient.GetUrl(apiKeyPermissionEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageAddIpRestrictionResult>> AddIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            apiKey.ValidateNotNull(nameof(apiKey));
            ipAddress.ValidateNotNull(nameof(ipAddress));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"ipAddress", ipAddress}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageAddIpRestrictionResult>(_baseClient.GetUrl(apiKeyIpRestrictionListEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageIpRestriction>> ChangeIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, bool ipRestrict, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            apiKey.ValidateNotNull(nameof(apiKey));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"ipRestrict", ipRestrict.ToString().ToLower()}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageIpRestriction>(_baseClient.GetUrl(apiKeyIpRestrictionEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageIpRestriction>> GetIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            apiKey.ValidateNotNull(nameof(apiKey));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageIpRestriction>(_baseClient.GetUrl(apiKeyIpRestrictionEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageIpRestrictionBase>> DeleteIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            apiKey.ValidateNotNull(nameof(apiKey));
            ipAddress.ValidateNotNull(nameof(ipAddress));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"ipAddress", ipAddress}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageIpRestrictionBase>(_baseClient.GetUrl(apiKeyIpRestrictionListEndpoint, brokerageApi, brokerageVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Sub accounts commission

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageSubAccountCommission>> ChangeSubAccountCommissionAsync(string subAccountId,
            decimal makerCommission, decimal takerCommission, decimal? marginMakerCommission = null, decimal? marginTakerCommission = null,
            int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"makerCommission", makerCommission.ToString(CultureInfo.InvariantCulture)},
                                 {"takerCommission", takerCommission.ToString(CultureInfo.InvariantCulture)}
                             };
            parameters.AddOptionalParameter("marginMakerCommission", marginMakerCommission?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("marginTakerCommission", marginTakerCommission?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageSubAccountCommission>(_baseClient.GetUrl(apiKeyCommissionEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageSubAccountFuturesCommission>> ChangeSubAccountFuturesCommissionAdjustmentAsync(string subAccountId, string symbol,
            int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            symbol.ValidateNotNull(nameof(symbol));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"symbol", symbol},
                                 {"makerAdjustment", makerAdjustment.ToString(CultureInfo.InvariantCulture)},
                                 {"takerAdjustment", takerAdjustment.ToString(CultureInfo.InvariantCulture)}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageSubAccountFuturesCommission>(_baseClient.GetUrl(apiKeyCommissionFuturesEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccountFuturesCommission>>> GetSubAccountFuturesCommissionAdjustmentAsync(string subAccountId,
            string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId}
                             };
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageSubAccountFuturesCommission>>(_baseClient.GetUrl(apiKeyCommissionFuturesEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageSubAccountCoinFuturesCommission>> ChangeSubAccountCoinFuturesCommissionAdjustmentAsync(string subAccountId,
            string pair, int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));
            pair.ValidateNotNull(nameof(pair));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"pair", pair},
                                 {"makerAdjustment", makerAdjustment.ToString(CultureInfo.InvariantCulture)},
                                 {"takerAdjustment", takerAdjustment.ToString(CultureInfo.InvariantCulture)}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageSubAccountCoinFuturesCommission>(_baseClient.GetUrl(apiKeyCommissionCoinFuturesEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccountFuturesCommission>>> GetSubAccountCoinFuturesCommissionAdjustmentAsync(string subAccountId,
            string? pair = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId}
                             };
            parameters.AddOptionalParameter("pair", pair);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageSubAccountFuturesCommission>>(_baseClient.GetUrl(apiKeyCommissionCoinFuturesEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Sub accounts asset summary

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageSpotAssetInfo>> GetSubAccountSpotAssetInfoAsync(
            string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("subAccountId", subAccountId);
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageSpotAssetInfo>(_baseClient.GetUrl(spotSummaryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageMarginAssetInfo>> GetSubAccountMarginAssetInfoAsync(
            string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("subAccountId", subAccountId);
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageMarginAssetInfo>(_baseClient.GetUrl(marginSummaryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageFuturesAssetInfo>> GetSubAccountFuturesAssetInfoAsync(HitoBitBrokerageFuturesType futuresType,
            string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
                             {
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)}
                             };
            parameters.AddOptionalParameter("subAccountId", subAccountId);
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageFuturesAssetInfo>(_baseClient.GetUrl(futuresSummaryEndpoint, brokerageApi, "2"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Sub accounts BNB burn

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageChangeBnbBurnSpotAndMarginResult>> ChangeBnbBurnForSubAccountSpotAndMarginAsync(string subAccountId, bool spotBnbBurn,
            int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"spotBNBBurn", spotBnbBurn.ToString().ToLower()}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageChangeBnbBurnSpotAndMarginResult>(_baseClient.GetUrl(changeBnbBurnForSubAccountSpotAndMarginEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageChangeBnbBurnMarginInterestResult>> ChangeBnbBurnForSubAccountMarginInterestAsync(string subAccountId, bool interestBnbBurn,
            int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"interestBNBBurn", interestBnbBurn.ToString().ToLower()}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageChangeBnbBurnMarginInterestResult>(_baseClient.GetUrl(changeBnbBurnForSubAccountMarginInterestEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageBnbBurnStatus>> GetBnbBurnStatusForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId}
                             };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageBnbBurnStatus>(_baseClient.GetUrl(bnbBurnForSubAccountStatusEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Transfer & history

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageTransferResult>> TransferUniversalAsync(string asset, decimal quantity,
            string? fromId, BrokerageAccountType fromAccountType, string? toId, BrokerageAccountType toAccountType,
            string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
                             {
                                 {"asset", asset},
                                 {"amount", quantity.ToString(CultureInfo.InvariantCulture)},
                                 {"fromAccountType", JsonConvert.SerializeObject(fromAccountType, new BrokerageAccountTypeConverter(false))},
                                 {"toAccountType", JsonConvert.SerializeObject(toAccountType, new BrokerageAccountTypeConverter(false))}
                             };
            parameters.AddOptionalParameter("fromId", fromId);
            parameters.AddOptionalParameter("toId", toId);
            parameters.AddOptionalParameter("clientTranId", clientTransferId);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageTransferResult>(_baseClient.GetUrl(transferUniversalEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageTransferTransactionUniversal>>> GetTransferHistoryUniversalAsync(
            string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null,
            int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
                             {
                                 {"showAllStatus", showAllStatus.ToString().ToLower()},
                             };
            parameters.AddOptionalParameter("fromId", fromId);
            parameters.AddOptionalParameter("toId", toId);
            parameters.AddOptionalParameter("clientTranId", clientTransferId);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endDate));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageTransferTransactionUniversal>>(_baseClient.GetUrl(transferUniversalEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageTransferResult>> TransferAsync(string asset, decimal quantity,
            string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
                             {
                                 {"asset", asset},
                                 {"amount", quantity.ToString(CultureInfo.InvariantCulture)},
                             };
            parameters.AddOptionalParameter("fromId", fromId);
            parameters.AddOptionalParameter("toId", toId);
            parameters.AddOptionalParameter("clientTranId", clientTransferId);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageTransferResult>(_baseClient.GetUrl(transferEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageTransferFuturesResult>> TransferFuturesAsync(string asset, decimal quantity, HitoBitBrokerageFuturesType futuresType,
            string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
                             {
                                 {"asset", asset},
                                 {"amount", quantity.ToString(CultureInfo.InvariantCulture)},
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)}
                             };
            parameters.AddOptionalParameter("fromId", fromId);
            parameters.AddOptionalParameter("toId", toId);
            parameters.AddOptionalParameter("clientTranId", clientTransferId);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageTransferFuturesResult>(_baseClient.GetUrl(transferFuturesEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageTransferTransaction>>> GetTransferHistoryAsync(string? fromId = null, string? toId = null,
            string? clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false,
            int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
                             {
                                 {"showAllStatus", showAllStatus.ToString().ToLower()},
                             };
            parameters.AddOptionalParameter("fromId", fromId);
            parameters.AddOptionalParameter("toId", toId);
            parameters.AddOptionalParameter("clientTranId", clientTransferId);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endDate));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageTransferTransaction>>(_baseClient.GetUrl(transferEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageTransferFuturesTransactions>> GetTransferFuturesHistoryAsync(string subAccountId,
            HitoBitBrokerageFuturesType futuresType, DateTime? startDate = null, DateTime? endDate = null,
            int? page = null, int? limit = null, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)}
                             };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endDate));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("clientTranId", clientTransferId);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageTransferFuturesTransactions>(_baseClient.GetUrl(transferFuturesEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccountDepositTransaction>>> GetSubAccountDepositHistoryAsync(string? subAccountId = null,
            string? asset = null, HitoBitBrokerageSubAccountDepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null,
            int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("subAccountId", subAccountId);
            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("status", status.HasValue ? ((int)status).ToString(CultureInfo.InvariantCulture) : null);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endDate));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageSubAccountDepositTransaction>>(_baseClient.GetUrl(depositHistoryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Broker

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBrokerageAccountInfo>> GetBrokerAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBrokerageAccountInfo>(_baseClient.GetUrl(brokerAccountInfoEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Broker commission rebates

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageRebate>>> GetBrokerCommissionRebatesRecentAsync(string subAccountId,
            DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            subAccountId.ValidateNotNull(nameof(subAccountId));

            var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                             };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startDate));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endDate));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageRebate>>(_baseClient.GetUrl(rebatesRecentEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBrokerageFuturesRebate>>> GetBrokerFuturesCommissionRebatesHistoryAsync(HitoBitBrokerageFuturesType futuresType,
            DateTime startDate, DateTime endDate, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
                             {
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)},
                                 {"startTime", DateTimeConverter.ConvertToMilliseconds(startDate)!},
                                 {"endTime",  DateTimeConverter.ConvertToMilliseconds(endDate)!}
                             };
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBrokerageFuturesRebate>>(_baseClient.GetUrl(rebatesFuturesHistoryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion
    }
}
