using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Margin;
using HitoBit.Net.Objects.Models.Spot.SubAccountData;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc />
    public class HitoBitRestClientGeneralApiSubAccount : IHitoBitRestClientGeneralApiSubAccount
    {
        private const string subAccountListEndpoint = "sub-account/list";
        private const string subAccountTransferHistoryEndpoint = "sub-account/sub/transfer/history";
        private const string transferSubAccountEndpoint = "sub-account/universalTransfer";
        private const string queryUniversalTransferHistoryEndpoint = "sub-account/universalTransfer";
        private const string subAccountStatusEndpoint = "sub-account/status";
        private const string subAccountAssetsEndpoint = "sub-account/assets";

        private const string subAccountDepositAddressEndpoint = "capital/deposit/subAddress";
        private const string subAccountDepositHistoryEndpoint = "capital/deposit/subHisrec";


        private const string subAccountEnableMarginEndpoint = "sub-account/margin/enable";
        private const string subAccountMarginDetailsEndpoint = "sub-account/margin/account";
        private const string subAccountMarginSummaryEndpoint = "sub-account/margin/accountSummary";
        private const string subAccountTransferMarginSpotEndpoint = "sub-account/margin/transfer";

        private const string subAccountEnableFuturesEndpoint = "sub-account/futures/enable";
        private const string subAccountFuturesDetailsEndpoint = "sub-account/futures/account";
        private const string subAccountFuturesSummaryEndpoint = "sub-account/futures/accountSummary";
        private const string subAccountTransferFuturesSpotEndpoint = "sub-account/futures/transfer";
        private const string subAccountFuturesPositionRiskEndpoint = "sub-account/futures/positionRisk";

        private const string subAccountTransferToSubEndpoint = "sub-account/transfer/subToSub";
        private const string subAccountTransferToMasterEndpoint = "sub-account/transfer/subToMaster";
        private const string subAccountTransferHistorySubAccountEndpoint = "sub-account/transfer/subUserHistory";

        private const string subAccountSpotSummaryEndpoint = "sub-account/spotSummary";

        private const string subAccountCreateVirtualEndpoint = "sub-account/virtualSubAccount";
        private const string subAccountEnableBlvtEndpoint = "sub-account/blvt/enable";

        private const string updateIpRestrictionListEndpoint = "sub-account/subAccountApi/ipRestriction";
        private const string getIpRestrictionListEndpoint = "sub-account/subAccountApi/ipRestriction";
        private const string removeIpRestrictionListEndpoint = "sub-account/subAccountApi/ipRestriction/ipList";

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiSubAccount(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Query Sub-account List(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccount>>> GetSubAccountsAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, bool? isFreeze = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("email", email);
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("isFreeze", isFreeze);

            var result = await _baseClient.SendRequestInternal<HitoBitSubAccountWrapper>(_baseClient.GetUrl(subAccountListEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result ? result.As<IEnumerable<HitoBitSubAccount>>(result.Data.SubAccounts) : result.As<IEnumerable<HitoBitSubAccount>>(default);
        }

        #endregion

        #region Query Sub-account Transfer History(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountTransfer>>> GetSubAccountTransferHistoryForMasterAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("fromEmail", fromEmail);
            parameters.AddOptionalParameter("toEmail", toEmail);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<IEnumerable<HitoBitSubAccountTransfer>>(_baseClient.GetUrl(subAccountTransferHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Sub-account Transfer(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> TransferSubAccountAsync(TransferAccountType fromAccountType, TransferAccountType toAccountType, string asset, decimal quantity, string? fromEmail = null, string? toEmail = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(fromEmail) && string.IsNullOrEmpty(toEmail))
                throw new ArgumentException("fromEmail and/or toEmail should be provided");
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "fromAccountType", JsonConvert.SerializeObject(fromAccountType, new TransferAccountTypeConverter(false)) },
                { "toAccountType", JsonConvert.SerializeObject(toAccountType, new TransferAccountTypeConverter(false)) },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("fromEmail", fromEmail);
            parameters.AddOptionalParameter("toEmail", toEmail);

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitTransaction>(_baseClient.GetUrl(transferSubAccountEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }

        #endregion

        #region Query Sub-account Assets(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBalance>>> GetSubAccountAssetsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitSubAccountAsset>(_baseClient.GetUrl(subAccountAssetsEndpoint, "sapi", "3"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success)
                return result.As<IEnumerable<HitoBitBalance>>(default);

            if (!result.Data.Success)
                return result.AsError<IEnumerable<HitoBitBalance>>(new ServerError(result.Data!.Message));

            return result.As(result.Data.Balances);
        }
        #endregion

        #region Get Sub-account Deposit Address (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountDepositAddress>> GetSubAccountDepositAddressAsync(string email, string asset, string? network = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "coin", asset }
            };

            parameters.AddOptionalParameter("network", network);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountDepositAddress>(_baseClient.GetUrl(subAccountDepositAddressEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Get Sub-account Deposit History (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountDeposit>>> GetSubAccountDepositHistoryAsync(string email, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitSubAccountDeposit>>(_baseClient.GetUrl(subAccountDepositHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Sub-account's Status on Margin/Futures(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountStatus>>> GetSubAccountStatusAsync(string? email = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("email", email);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitSubAccountStatus>>(_baseClient.GetUrl(subAccountStatusEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Enable Margin for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountMarginEnabled>> EnableMarginForSubAccountAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountMarginEnabled>(_baseClient.GetUrl(subAccountEnableMarginEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }

        #endregion

        #region Get Detail on Sub-account's Margin Account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountMarginDetails>> GetSubAccountMarginDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountMarginDetails>(_baseClient.GetUrl(subAccountMarginDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Get Summary of Sub-account's Margin Account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountsMarginSummary>> GetSubAccountsMarginSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountsMarginSummary>(_baseClient.GetUrl(subAccountMarginSummaryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Enable Futures for Sub-account (For Master Account) 

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesEnabled>> EnableFuturesForSubAccountAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountFuturesEnabled>(_baseClient.GetUrl(subAccountEnableFuturesEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }

        #endregion

        #region Get Detail on Sub-account's Futures Account (For Master Account) 

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesDetails>> GetSubAccountFuturesDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountFuturesDetails>(_baseClient.GetUrl(subAccountFuturesDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesDetailsV2>> GetSubAccountFuturesDetailsAsync(FuturesAccountType futuresAccountType, string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "futuresType", JsonConvert.SerializeObject(futuresAccountType, new FuturesAccountTypeConverter(false)) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountFuturesDetailsV2>(_baseClient.GetUrl(subAccountFuturesDetailsEndpoint, "sapi", "2"), HttpMethod.Get, ct, parameters, true, weight: 1).ConfigureAwait(false);
        }

        #endregion

        #region Get Summary of Sub-account's Futures Account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountsFuturesSummary>> GetSubAccountsFuturesSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountsFuturesSummary>(_baseClient.GetUrl(subAccountFuturesSummaryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Futures Postion-Risk of Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountFuturesPositionRisk>>> GetSubAccountsFuturesPositionRiskAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitSubAccountFuturesPositionRisk>>(_baseClient.GetUrl(subAccountFuturesPositionRiskEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesPositionRiskV2>> GetSubAccountsFuturesPositionRiskAsync(FuturesAccountType futuresAccountType, string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "futuresType", JsonConvert.SerializeObject(futuresAccountType, new FuturesAccountTypeConverter(false)) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountFuturesPositionRiskV2>(_baseClient.GetUrl(subAccountFuturesPositionRiskEndpoint, "sapi", "2"), HttpMethod.Get, ct, parameters, true, weight: 1).ConfigureAwait(false);
        }

        #endregion

        #region Futures Transfer for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountFuturesAsync(string email, string asset, decimal quantity, SubAccountFuturesTransferType type, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "asset", asset },
                { "type", JsonConvert.SerializeObject(type, new SubAccountFuturesTransferTypeConverter(false)) },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountTransaction>(_baseClient.GetUrl(subAccountTransferFuturesSpotEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }
        #endregion

        #region Margin Transfer for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountMarginAsync(string email, string asset, decimal quantity, SubAccountMarginTransferType type, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "asset", asset },
                { "type", JsonConvert.SerializeObject(type, new SubAccountMarginTransferTypeConverter(false)) },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountTransaction>(_baseClient.GetUrl(subAccountTransferMarginSpotEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }
        #endregion

        #region Transfer to Sub-account of Same Master (For Sub-account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountToSubAccountAsync(string email, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "toEmail", email },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountTransaction>(_baseClient.GetUrl(subAccountTransferToSubEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }
        #endregion

        #region Transfer to Master (For Sub-account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountToMasterAsync(string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountTransaction>(_baseClient.GetUrl(subAccountTransferToMasterEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Sub-account Transfer History (For Sub-account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountTransferSubAccount>>> GetSubAccountTransferHistoryForSubAccountAsync(string? asset = null, SubAccountTransferSubAccountType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("type", type == null ? null : JsonConvert.SerializeObject(type, new SubAccountTransferSubAccountTypeConverter(false)));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitSubAccountTransferSubAccount>>(_baseClient.GetUrl(subAccountTransferHistorySubAccountEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Query Sub-account Spot Assets Summary (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountSpotAssetsSummary>> GetSubAccountBtcValuesAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("email", email);
            parameters.AddOptionalParameter("page", page);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountSpotAssetsSummary>(_baseClient.GetUrl(subAccountSpotSummaryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Create a Virtual Sub-account(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountEmail>> CreateVirtualSubAccountAsync(string subAccountString, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "subAccountString", subAccountString }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountEmail>(_baseClient.GetUrl(subAccountCreateVirtualEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, postPosition: HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }

        #endregion

        #region Enable Leverage Token for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountBlvt>> EnableBlvtForSubAccountAsync(string email, bool enable, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "enableBlvt", enable }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitSubAccountBlvt>(_baseClient.GetUrl(subAccountEnableBlvtEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }

        #endregion

        #region Query Universal Transfer History (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountUniversalTransferTransaction>>> GetUniversalTransferHistoryAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("fromEmail", fromEmail);
            parameters.AddOptionalParameter("toEmail", toEmail);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitSubAccountUniversalTransfersList>(_baseClient.GetUrl(queryUniversalTransferHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result ? result.As<IEnumerable<HitoBitSubAccountUniversalTransferTransaction>>(result.Data.Transactions) : result.As<IEnumerable<HitoBitSubAccountUniversalTransferTransaction>>(default);
        }

        #endregion

        #region IP restrictions
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitIpRestriction>> UpdateIpRestrictionForSubAccountApiKeyAsync(string email, string apiKey, bool ipRestrict, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "subAccountApiKey", apiKey },
                { "status", ipRestrict ? 2: 1 }
            };

            if (ipAddresses != null)
                parameters.AddOptionalParameter("ipAddress", string.Join(",", ipAddresses));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitIpRestriction>(_baseClient.GetUrl(updateIpRestrictionListEndpoint, "sapi", "2"), HttpMethod.Post, ct, parameters, true, weight: 3000, postPosition: HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitIpRestriction>> RemoveIpRestrictionForSubAccountApiKeyAsync(string email, string apiKey, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "subAccountApiKey", apiKey }
            };

            if(ipAddresses != null)
                parameters.AddOptionalParameter("ipAddress", string.Join(",", ipAddresses));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitIpRestriction>(_baseClient.GetUrl(removeIpRestrictionListEndpoint, "sapi", "1"), HttpMethod.Delete, ct, parameters, true, weight: 3000, postPosition: HttpMethodParameterPosition.InUri).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitIpRestriction>> GetIpRestrictionForSubAccountApiKeyAsync(string email, string apiKey, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "subAccountApiKey", apiKey },
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitIpRestriction>(_baseClient.GetUrl(getIpRestrictionListEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        }
        #endregion
    }
}
