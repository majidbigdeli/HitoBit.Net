using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Margin;
using HitoBit.Net.Objects.Models.Spot.SubAccountData;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc />
    internal class HitoBitRestClientGeneralApiSubAccount : IHitoBitRestClientGeneralApiSubAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiSubAccount(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Query Sub-account List(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccount>>> GetSubAccountsAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, bool? isFreeze = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("email", email);
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("isFreeze", isFreeze);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/list", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitSubAccountWrapper>(request, parameters, ct).ConfigureAwait(false);
            return result ? result.As<IEnumerable<HitoBitSubAccount>>(result.Data.SubAccounts) : result.As<IEnumerable<HitoBitSubAccount>>(default);
        }

        #endregion

        #region Query Sub-account Transfer History(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountTransfer>>> GetSubAccountTransferHistoryForMasterAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("fromEmail", fromEmail);
            parameters.AddOptionalParameter("toEmail", toEmail);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/sub/transfer/history", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitSubAccountTransfer>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Sub-account Transfer(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> TransferSubAccountAsync(TransferAccountType fromAccountType, TransferAccountType toAccountType, string asset, decimal quantity, string? fromEmail = null, string? toEmail = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(fromEmail) && string.IsNullOrEmpty(toEmail))
                throw new ArgumentException("fromEmail and/or toEmail should be provided");
            asset.ValidateNotNull(nameof(asset));

            var parameters = new ParameterCollection
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddEnum("fromAccountType", fromAccountType);
            parameters.AddEnum("toAccountType", toAccountType);
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("fromEmail", fromEmail);
            parameters.AddOptionalParameter("toEmail", toEmail);

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/universalTransfer", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitTransaction>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Query Sub-account Assets(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBalance>>> GetSubAccountAssetsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new ParameterCollection
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v3/sub-account/assets", HitoBitExchange.RateLimiter.SpotRestUid, 60, true);
            var result = await _baseClient.SendAsync<HitoBitSubAccountAsset>(request, parameters, ct).ConfigureAwait(false);
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

            var parameters = new ParameterCollection
            {
                { "email", email },
                { "coin", asset }
            };

            parameters.AddOptionalParameter("network", network);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/capital/deposit/subAddress", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitSubAccountDepositAddress>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Get Sub-account Deposit History (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountDeposit>>> GetSubAccountDepositHistoryAsync(string email, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new ParameterCollection
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/capital/deposit/subHisrec", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitSubAccountDeposit>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Sub-account's Status on Margin/Futures(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountStatus>>> GetSubAccountStatusAsync(string? email = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("email", email);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/status", HitoBitExchange.RateLimiter.SpotRestIp, 10, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitSubAccountStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Enable Margin for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountMarginEnabled>> EnableMarginForSubAccountAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new ParameterCollection
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/margin/enable", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountMarginEnabled>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Detail on Sub-account's Margin Account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountMarginDetails>> GetSubAccountMarginDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new ParameterCollection
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/margin/account", HitoBitExchange.RateLimiter.SpotRestIp, 10, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountMarginDetails>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Summary of Sub-account's Margin Account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountsMarginSummary>> GetSubAccountsMarginSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/margin/accountSummary", HitoBitExchange.RateLimiter.SpotRestIp, 10, true);
            return await _baseClient.SendAsync<HitoBitSubAccountsMarginSummary>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Enable Futures for Sub-account (For Master Account) 

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesEnabled>> EnableFuturesForSubAccountAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            var parameters = new ParameterCollection
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/futures/enable", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountFuturesEnabled>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Detail on Sub-account's Futures Account (For Master Account) 

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesDetails>> GetSubAccountFuturesDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new ParameterCollection
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/futures/account", HitoBitExchange.RateLimiter.SpotRestIp, 10, true);
            return await _baseClient.SendAsync<HitoBitSubAccountFuturesDetails>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesDetailsV2>> GetSubAccountFuturesDetailsAsync(FuturesAccountType futuresAccountType, string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new ParameterCollection
            {
                { "email", email },
            };

            parameters.AddEnum("futuresType", futuresAccountType);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v2/sub-account/futures/account", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitSubAccountFuturesDetailsV2>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Summary of Sub-account's Futures Account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountsFuturesSummary>> GetSubAccountsFuturesSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/futures/accountSummary", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitSubAccountsFuturesSummary>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Futures Postion-Risk of Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountFuturesPositionRisk>>> GetSubAccountsFuturesPositionRiskAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "email", email }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/futures/positionRisk", HitoBitExchange.RateLimiter.SpotRestIp, 10, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitSubAccountFuturesPositionRisk>>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountFuturesPositionRiskV2>> GetSubAccountsFuturesPositionRiskAsync(FuturesAccountType futuresAccountType, string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "email", email },
            };
            parameters.AddEnum("futuresType", futuresAccountType);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v2/sub-account/futures/positionRisk", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitSubAccountFuturesPositionRiskV2>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Futures Transfer for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountFuturesAsync(string email, string asset, decimal quantity, FuturesTransferType type, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            asset.ValidateNotNull(nameof(asset));

            var parameters = new ParameterCollection
            {
                { "email", email },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddEnum("type", type);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/futures/transfer", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountTransaction>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Margin Transfer for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountMarginAsync(string email, string asset, decimal quantity, SubAccountMarginTransferType type, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            asset.ValidateNotNull(nameof(asset));

            var parameters = new ParameterCollection
            {
                { "email", email },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddEnum("type", type);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/margin/transfer", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountTransaction>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Transfer to Sub-account of Same Master (For Sub-account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountToSubAccountAsync(string email, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));
            asset.ValidateNotNull(nameof(asset));

            var parameters = new ParameterCollection
            {
                { "toEmail", email },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/transfer/subToSub", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountTransaction>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Transfer to Master (For Sub-account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> TransferSubAccountToMasterAsync(string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new ParameterCollection
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/transfer/subToMaster", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitSubAccountTransaction>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Sub-account Transfer History (For Sub-account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountTransferSubAccount>>> GetSubAccountTransferHistoryForSubAccountAsync(string? asset = null, SubAccountTransferSubAccountType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalEnum("type", type);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/transfer/subUserHistory", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<IEnumerable<HitoBitSubAccountTransferSubAccount>>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Query Sub-account Spot Assets Summary (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountSpotAssetsSummary>> GetSubAccountBtcValuesAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("email", email);
            parameters.AddOptionalParameter("page", page);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/spotSummary", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitSubAccountSpotAssetsSummary>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Create a Virtual Sub-account(For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountEmail>> CreateVirtualSubAccountAsync(string subAccountString, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "subAccountString", subAccountString }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/virtualSubAccount", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountEmail>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Enable Leverage Token for Sub-account (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountBlvt>> EnableBlvtForSubAccountAsync(string email, bool enable, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "email", email },
                { "enableBlvt", enable }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/blvt/enable", HitoBitExchange.RateLimiter.SpotRestIp, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitSubAccountBlvt>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Query Universal Transfer History (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountUniversalTransferTransaction>>> GetUniversalTransferHistoryAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("fromEmail", fromEmail);
            parameters.AddOptionalParameter("toEmail", toEmail);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/universalTransfer", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitSubAccountUniversalTransfersList>(request, parameters, ct).ConfigureAwait(false);
            return result ? result.As<IEnumerable<HitoBitSubAccountUniversalTransferTransaction>>(result.Data.Transactions) : result.As<IEnumerable<HitoBitSubAccountUniversalTransferTransaction>>(default);
        }

        #endregion

        #region IP restrictions
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitIpRestriction>> UpdateIpRestrictionForSubAccountApiKeyAsync(string email, string apiKey, bool ipRestrict, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "email", email },
                { "subAccountApiKey", apiKey },
                { "status", ipRestrict ? 2: 1 }
            };

            if (ipAddresses != null)
                parameters.AddOptionalParameter("ipAddress", string.Join(",", ipAddresses));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v2/sub-account/subAccountApi/ipRestriction", HitoBitExchange.RateLimiter.SpotRestUid, 3000, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitIpRestriction>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitIpRestriction>> RemoveIpRestrictionForSubAccountApiKeyAsync(string email, string apiKey, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "email", email },
                { "subAccountApiKey", apiKey }
            };

            if(ipAddresses != null)
                parameters.AddOptionalParameter("ipAddress", string.Join(",", ipAddresses));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Delete, "sapi/v1/sub-account/subAccountApi/ipRestriction/ipList", HitoBitExchange.RateLimiter.SpotRestUid, 3000, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<HitoBitIpRestriction>(request, parameters, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitIpRestriction>> GetIpRestrictionForSubAccountApiKeyAsync(string email, string apiKey, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "email", email },
                { "subAccountApiKey", apiKey },
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/subAccountApi/ipRestriction", HitoBitExchange.RateLimiter.SpotRestUid, 3000, true);
            return await _baseClient.SendAsync<HitoBitIpRestriction>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion

        #region Query Sub-account Futures Asset Transfer History (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSubAccountAssetTransferHistory>>> GetFuturesAssetTransferHistoryAsync(string email, FuturesAccountType accountType, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "email", email },
                { "futuresType", EnumConverter.GetString(accountType) },
            };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/sub-account/futures/internalTransfer", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            var result = await _baseClient.SendAsync<HitoBitSubAccountAssetTransferHistoryList>(request, parameters, ct).ConfigureAwait(false);
            return result ? result.As(result.Data.Transfers) : result.As<IEnumerable<HitoBitSubAccountAssetTransferHistory>>(default);
        }

        #endregion

        #region Sub-account Futures Asset Transfer (For Master Account)

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitSubAccountTransaction>> FuturesAssetTransferAsync(string fromEmail, string toEmail, FuturesAccountType accountType, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new ParameterCollection
            {
                { "fromEmail", fromEmail },
                { "toEmail", toEmail },
                { "futuresType", EnumConverter.GetString(accountType) },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "sapi/v1/sub-account/futures/internalTransfer", HitoBitExchange.RateLimiter.SpotRestIp, 1, true);
            return await _baseClient.SendAsync<HitoBitSubAccountTransaction>(request, parameters, ct).ConfigureAwait(false);
        }
        #endregion
    }
}
