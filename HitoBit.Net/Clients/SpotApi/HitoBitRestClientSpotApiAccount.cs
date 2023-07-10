using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using HitoBit.Net.Objects.Models.Spot.IsolatedMargin;
using HitoBit.Net.Objects.Models.Spot.Margin;
using HitoBit.Net.Objects.Models.Spot.PortfolioMargin;
using HitoBit.Net.Objects.Models.Spot.Staking;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace HitoBit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class HitoBitRestClientSpotApiAccount : IHitoBitRestClientSpotApiAccount
    {
        private const string accountInfoEndpoint = "account";
        private const string fiatDepositWithdrawHistoryEndpoint = "fiat/orders";
        private const string fiatPaymentHistoryEndpoint = "fiat/payments";
        private const string withdrawEndpoint = "capital/withdraw/apply";
        private const string withdrawHistoryEndpoint = "capital/withdraw/history";
        private const string depositHistoryEndpoint = "capital/deposit/hisrec";
        private const string depositAddressEndpoint = "capital/deposit/address";
        private const string accountSnapshotEndpoint = "accountSnapshot";
        private const string accountStatusEndpoint = "account/status";
        private const string fundingWalletEndpoint = "asset/get-funding-asset";
        private const string apiRestrictionsEndpoint = "account/apiRestrictions ";
        private const string dividendRecordsEndpoint = "asset/assetDividend";
        private const string userCoinsEndpoint = "capital/config/getall";
        private const string disableFastWithdrawSwitchEndpoint = "account/disableFastWithdrawSwitch";
        private const string enableFastWithdrawSwitchEndpoint = "account/enableFastWithdrawSwitch";
        private const string dustLogEndpoint = "asset/dribblet";
        private const string balancesEndpoint = "asset/getUserAsset";
        private const string dustTransferEndpoint = "asset/dust";
        private const string dustElligableEndpoint = "asset/dust-btc";
        private const string toggleBnbBurnEndpoint = "bnbBurn";
        private const string getBnbBurnEndpoint = "bnbBurn";
        private const string universalTransferEndpoint = "asset/transfer";
        private const string tradingStatusEndpoint = "account/apiTradingStatus";
        private const string orderRateLimitEndpoint = "rateLimit/order";

        // Margin
        private const string marginTransferEndpoint = "margin/transfer";
        private const string marginBorrowEndpoint = "margin/loan";
        private const string marginRepayEndpoint = "margin/repay";
        private const string getLoanEndpoint = "margin/loan";
        private const string getRepayEndpoint = "margin/repay";
        private const string marginDustLogEndpoint = "margin/dribblet";
        private const string marginAccountInfoEndpoint = "margin/account";
        private const string maxBorrowableEndpoint = "margin/maxBorrowable";
        private const string maxTransferableEndpoint = "margin/maxTransferable";
        private const string transferHistoryEndpoint = "margin/transfer";
        private const string interestHistoryEndpoint = "margin/interestHistory";
        private const string interestRateHistoryEndpoint = "margin/interestRateHistory";
        private const string interestMarginDataEndpoint = "margin/crossMarginData";
        private const string forceLiquidationHistoryEndpoint = "margin/forceLiquidationRec";
        private const string marginLevelInformation = "margin/tradeCoeff";

        private const string isolatedMargingTierEndpoint = "margin/isolatedMarginTier";

        private const string isolatedMarginTransferHistoryEndpoint = "margin/isolated/transfer";
        private const string isolatedMarginAccountEndpoint = "margin/isolated/account";
        private const string isolatedMarginAccountLimitEndpoint = "margin/isolated/accountLimit";
        private const string transferIsolatedMarginAccountEndpoint = "margin/isolated/transfer";

        private const string marginOrderRateLimitEndpoint = "margin/rateLimit/order";

        private const string getListenKeyEndpoint = "userDataStream";
        private const string keepListenKeyAliveEndpoint = "userDataStream";
        private const string closeListenKeyEndpoint = "userDataStream";

        private const string getListenKeyIsolatedEndpoint = "userDataStream/isolated";
        private const string keepListenKeyAliveIsolatedEndpoint = "userDataStream/isolated";
        private const string closeListenKeyIsolatedEndpoint = "userDataStream/isolated";


        // Blvt
        private const string blvtUserLimitEndpoint = "blvt/userLimit";

        // Rebate
        private const string rebateHistoryEndpoint = "rebate/taxQuery";

        // Staking
        private const string setAutoStakingEndpoint = "staking/setAutoStaking";
        private const string stakingQuotaLeftEndpoint = "staking/personalLeftQuota";

        // Portfolio Margin
        private const string portfolioMarginAccountEndpoint = "portfolio/account";
        private const string portfolioMarginCollateralRateEndpoint = "portfolio/collateralRate";
        private const string portfolioMarginLoanEndpoint = "portfolio/pmLoan";
        private const string portfolioMarginRepayEndpoint = "portfolio/repay";


        private const string marginApi = "sapi";
        private const string marginVersion = "1";

        private readonly HitoBitRestClientSpotApi _baseClient;

        internal HitoBitRestClientSpotApiAccount(HitoBitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Account info
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitAccountInfo>(_baseClient.GetUrl(accountInfoEndpoint, "api", "3"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }
        #endregion

        #region Get Fiat Payments History 
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFiatPayment>>> GetFiatPaymentHistoryAsync(OrderSide side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "transactionType", side == OrderSide.Buy ? "0": "1" }
            };
            parameters.AddOptionalParameter("beginTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<IEnumerable<HitoBitFiatPayment>>>(_baseClient.GetUrl(fiatPaymentHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);

            return result.As(result.Data?.Data!);
        }

        #endregion

        #region Get Fiat Deposit Withdraw History 
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFiatWithdrawDeposit>>> GetFiatDepositWithdrawHistoryAsync(TransactionType side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "transactionType", side == TransactionType.Deposit ? "0": "1" }
            };
            parameters.AddOptionalParameter("beginTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<IEnumerable<HitoBitFiatWithdrawDeposit>>>(_baseClient.GetUrl(fiatDepositWithdrawHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 90000).ConfigureAwait(false);

            return result.As(result.Data?.Data!);
        }

        #endregion

        #region Withdraw
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitWithdrawalPlaced>> WithdrawAsync(string asset, string address, decimal quantity, string? withdrawOrderId = null, string? network = null, string? addressTag = null, string? name = null, bool? transactionFeeFlag = null, WalletType? walletType = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            address.ValidateNotNull(nameof(address));

            var parameters = new Dictionary<string, object>
            {
                { "coin", asset },
                { "address", address },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("name", name);
            parameters.AddOptionalParameter("withdrawOrderId", withdrawOrderId);
            parameters.AddOptionalParameter("network", network);
            parameters.AddOptionalParameter("transactionFeeFlag", transactionFeeFlag);
            parameters.AddOptionalParameter("addressTag", addressTag);
            parameters.AddOptionalParameter("walletType", walletType != null ? JsonConvert.SerializeObject(walletType, new WalletTypeConverter(false)) : null);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitWithdrawalPlaced>(_baseClient.GetUrl(withdrawEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, HttpMethodParameterPosition.InUri).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Withdraw History
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitWithdrawal>>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawOrderId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, int? offset = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("withdrawOrderId", withdrawOrderId);
            parameters.AddOptionalParameter("status", status != null ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("offset", offset);

            var result = await _baseClient.SendRequestInternal<IEnumerable<HitoBitWithdrawal>>(_baseClient.GetUrl(withdrawHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Deposit history        
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitDeposit>>> GetDepositHistoryAsync(string? asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("status", status != null ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)) : null);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitDeposit>>(
                    _baseClient.GetUrl(depositHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true)
                .ConfigureAwait(false);
        }
        #endregion

        #region Get Deposit Address

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitDepositAddress>> GetDepositAddressAsync(string asset, string? network = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "coin", asset }
            };
            parameters.AddOptionalParameter("network", network);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitDepositAddress>(_baseClient.GetUrl(depositAddressEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Daily snapshots
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
            CancellationToken ct = default) =>
            await GetDailyAccountSnapshot<IEnumerable<HitoBitSpotAccountSnapshot>>(AccountType.Spot, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
            CancellationToken ct = default) =>
            await GetDailyAccountSnapshot<IEnumerable<HitoBitMarginAccountSnapshot>>(AccountType.Margin, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

        // TODO Should be moved
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
            CancellationToken ct = default) =>
            await GetDailyAccountSnapshot<IEnumerable<HitoBitFuturesAccountSnapshot>>(AccountType.Futures, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);


        private async Task<WebCallResult<T>> GetDailyAccountSnapshot<T>(AccountType accountType, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
            CancellationToken ct = default) where T : class
        {
            limit?.ValidateIntBetween(nameof(limit), 5, 30);

            var parameters = new Dictionary<string, object>
            {
                { "type", EnumConverter.GetString(accountType) }
            };
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitSnapshotWrapper<T>>(_baseClient.GetUrl(accountSnapshotEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 2400).ConfigureAwait(false);
            if (!result.Success)
                return result.As<T>(default);

            if (result.Data.Code != 200)
                return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message));

            return result.As(result.Data.SnapshotData);
        }
        #endregion

        #region Account status
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitAccountStatus>(_baseClient.GetUrl(accountStatusEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Funding Wallet
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitFundingAsset>>> GetFundingWalletAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("needBtcValuation", needBtcValuation?.ToString());
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitFundingAsset>>(_baseClient.GetUrl(fundingWalletEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region API Key Permission
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAPIKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitAPIKeyPermissions>(_baseClient.GetUrl(apiRestrictionsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region User coins
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));


            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitUserAsset>>(_baseClient.GetUrl(userCoinsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }
        #endregion

        #region Balances
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitUserBalance>>> GetBalancesAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("needBtcValuation", needBtcValuation);
            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitUserBalance>>(_baseClient.GetUrl(balancesEndpoint, "sapi", "3"), HttpMethod.Post, ct, parameters, true, weight: 5).ConfigureAwait(false);
        }
        #endregion

        #region Asset Dividend Records
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitDividendRecord>>> GetAssetDividendRecordsAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitDividendRecord>>(_baseClient.GetUrl(dividendRecordsEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }
        #endregion

        #region Disable Fast Withdraw Switch
        /// <inheritdoc />
        public async Task<WebCallResult<object>> DisableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(disableFastWithdrawSwitchEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Enable Fast Withdraw Switch
        /// <inheritdoc />
        public async Task<WebCallResult<object>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(enableFastWithdrawSwitchEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region DustLog
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            var result = await _baseClient.SendRequestInternal<HitoBitDustLogList>(_baseClient.GetUrl(dustLogEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Dust Transfer
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitDustTransferResult>> DustTransferAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default)
        {
            var assetsArray = assets.ToArray();

            assetsArray.ValidateNotNull(nameof(assets));
            foreach (var asset in assetsArray)
                asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "asset", assetsArray }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitDustTransferResult>(_baseClient.GetUrl(dustTransferEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Dust Elligable
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitElligableDusts>> GetAssetsForDustTransferAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitElligableDusts>(_baseClient.GetUrl(dustElligableEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Get BNB Burn Status
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBnbBurnStatus>(_baseClient.GetUrl(getBnbBurnEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Set BNB Burn Status
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitBnbBurnStatus>> SetBnbBurnStatusAsync(bool? spotTrading = null, bool? marginInterest = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            if (spotTrading == null && marginInterest == null)
                throw new ArgumentException("SpotTrading or MarginInterest should be provided");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("spotBNBBurn", spotTrading);
            parameters.AddOptionalParameter("interestBNBBurn", marginInterest);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitBnbBurnStatus>(_baseClient.GetUrl(toggleBnbBurnEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region User Universal Transfer
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> TransferAsync(UniversalTransferType type, string asset, decimal quantity, string? fromSymbol = null, string? toSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "type", JsonConvert.SerializeObject(type, new UniversalTransferTypeConverter(false)) },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("fromSymbol", fromSymbol);
            parameters.AddOptionalParameter("toSymbol", toSymbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitTransaction>(_baseClient.GetUrl(universalTransferEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Get User Universal Transfers
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitTransfer>>> GetTransfersAsync(UniversalTransferType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "type", JsonConvert.SerializeObject(type, new UniversalTransferTypeConverter(false)) }
            };
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", pageSize?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitTransfer>>(_baseClient.GetUrl(universalTransferEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Create a ListenKey 
        /// <inheritdoc />
        public async Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternal<HitoBitListenKey>(_baseClient.GetUrl(getListenKeyEndpoint, "api", "3"), HttpMethod.Post, ct).ConfigureAwait(false);
            return result.As(result.Data?.ListenKey!);
        }

        #endregion

        #region Ping/Keep-alive a ListenKey

        /// <inheritdoc />
        public async Task<WebCallResult<object>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(keepListenKeyAliveEndpoint, "api", "3"), HttpMethod.Put, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Invalidate a ListenKey
        /// <inheritdoc />
        public async Task<WebCallResult<object>> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(closeListenKeyEndpoint, "api", "3"), HttpMethod.Delete, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Margin Level Information

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitMarginLevel>> GetMarginLevelInformationAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
        {
            email.ValidateNotNull(nameof(email));

            var parameters = new Dictionary<string, object>
            {
                { "email", email },
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitMarginLevel>(_baseClient.GetUrl(marginLevelInformation, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Margin Account Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> CrossMarginTransferAsync(string asset, decimal quantity, TransferDirectionType type, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "type", JsonConvert.SerializeObject(type, new TransferDirectionTypeConverter(false)) }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitTransaction>(_baseClient.GetUrl(marginTransferEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 600).ConfigureAwait(false);
        }

        #endregion

        #region Margin Account Borrow

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> MarginBorrowAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            if (isIsolated == true && symbol == null)
                throw new ArgumentException("Symbol should be specified when using isolated margin");

            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString().ToLower());
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitTransaction>(_baseClient.GetUrl(marginBorrowEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        }

        #endregion

        #region Margin Account Repay

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> MarginRepayAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString().ToLower());
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitTransaction>(_baseClient.GetUrl(marginRepayEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        }

        #endregion

        #region Margin DustLog
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitDustLogList>> GetMarginDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            var result = await _baseClient.SendRequestInternal<HitoBitDustLogList>(_baseClient.GetUrl(marginDustLogEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Transfer History

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitTransferHistory>>> GetCrossMarginTransferHistoryAsync(TransferDirection direction, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 100);

            var parameters = new Dictionary<string, object>
            {
                { "direction", JsonConvert.SerializeObject(direction, new TransferDirectionConverter(false)) }
            };
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitTransferHistory>>(_baseClient.GetUrl(transferHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Query Loan Record

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitLoan>>> GetMarginLoansAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10, string? isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            limit?.ValidateIntBetween(nameof(limit), 1, 100);
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };
            parameters.AddOptionalParameter("txId", transactionId?.ToString(CultureInfo.InvariantCulture));

            // TxId or startTime must be sent. txId takes precedence.
            if (!transactionId.HasValue)
            {
                parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime ?? DateTime.MinValue));
            }
            else
            {
                parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            }

            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("current", current?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("archived", archived);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitLoan>>(_baseClient.GetUrl(getLoanEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Query Repay Record

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitRepay>>> GetMarginRepaysAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? size = null, string? isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };
            parameters.AddOptionalParameter("txId", transactionId?.ToString(CultureInfo.InvariantCulture));

            // TxId or startTime must be sent. txId takes precedence.
            if (!transactionId.HasValue)
            {
                parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime ?? DateTime.MinValue));
            }
            else
            {
                parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            }

            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("current", current?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
            parameters.AddOptionalParameter("archived", archived);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitRepay>>(_baseClient.GetUrl(getRepayEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Get Cross Margin Interest Data

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitInterestMarginData>>> GetInterestMarginDataAsync(string? asset = null, string? vipLevel = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            asset?.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>();

            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("vipLevel", vipLevel?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitInterestMarginData>>(_baseClient.GetUrl(interestMarginDataEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Interest History

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitInterestHistory>>> GetMarginInterestHistoryAsync(string? asset = null, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 100);
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
            parameters.AddOptionalParameter("archived", archived);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitInterestHistory>>(_baseClient.GetUrl(interestHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Interest Rate History

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitInterestRateHistory>>> GetMarginInterestRateHistoryAsync(string asset, string? vipLevel = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            asset?.ValidateNotNull(nameof(asset));
            limit?.ValidateIntBetween(nameof(limit), 1, 100);

            var parameters = new Dictionary<string, object>
            {
                { "asset", asset! }
            };
            parameters.AddOptionalParameter("vipLevel", vipLevel?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitInterestRateHistory>>(_baseClient.GetUrl(interestRateHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Force Liquidation Record
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitForcedLiquidation>>> GetMarginForcedLiquidationHistoryAsync(int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 100);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitForcedLiquidation>>(_baseClient.GetUrl(forceLiquidationHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get Isolated margin tier data
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitIsolatedMarginTierData>>> GetIsolatedMarginTierDataAsync(string symbol, int? tier = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };
            parameters.AddOptionalParameter("tier", tier);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitIsolatedMarginTierData>>(_baseClient.GetUrl(isolatedMargingTierEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Query Margin Account Details

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitMarginAccount>> GetMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitMarginAccount>(_baseClient.GetUrl(marginAccountInfoEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Query Max Borrow

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitMarginAmount>> GetMarginMaxBorrowAmountAsync(string asset, string? isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));

            var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };

            parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitMarginAmount>(_baseClient.GetUrl(maxBorrowableEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);
        }

        #endregion

        #region Query Max Transfer-Out Amount

        /// <inheritdoc />
        public async Task<WebCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string? isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };

            parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitMarginAmount>(_baseClient.GetUrl(maxTransferableEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);

            if (!result)
                return result.As<decimal>(default);

            return result.As(result.Data.Quantity);
        }

        #endregion

        #region Query isolated margin transfer history
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitIsolatedMarginTransfer>>>
            GetIsolatedMarginAccountTransferHistoryAsync(string symbol, string? asset = null,
                IsolatedMarginTransferDirection? from = null, IsolatedMarginTransferDirection? to = null,
                DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10,
                int? receiveWindow = null, CancellationToken ct = default)
        {
            symbol.ValidateHitoBitSymbol();

            var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("from",
                !from.HasValue
                    ? null
                    : JsonConvert.SerializeObject(from, new IsolatedMarginTransferDirectionConverter(false)));
            parameters.AddOptionalParameter("to",
                !to.HasValue
                    ? null
                    : JsonConvert.SerializeObject(to, new IsolatedMarginTransferDirectionConverter(false)));
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime)?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime)?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("current", current?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow",
                receiveWindow?.ToString(CultureInfo.InvariantCulture) ??
                _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient
                .SendRequestInternal<HitoBitQueryRecords<HitoBitIsolatedMarginTransfer>>(
                    _baseClient.GetUrl(isolatedMarginTransferHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct,
                    parameters, true, weight: 600).ConfigureAwait(false);
        }
        #endregion

        #region Query isolated margin account

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(
            int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();

            parameters.AddOptionalParameter("recvWindow",
                receiveWindow?.ToString(CultureInfo.InvariantCulture) ??
                _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient
                .SendRequestInternal<HitoBitIsolatedMarginAccount>(
                    _baseClient.GetUrl(isolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Get, ct,
                    parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Disable isolated margin account

        /// <inheritdoc />
        public async Task<WebCallResult<CreateIsolatedMarginAccountResult>> DisableIsolatedMarginAccountAsync(string symbol,
            int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

            parameters.AddOptionalParameter("recvWindow",
                receiveWindow?.ToString(CultureInfo.InvariantCulture) ??
                _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient
                .SendRequestInternal<CreateIsolatedMarginAccountResult>(
                    _baseClient.GetUrl(isolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Delete, ct,
                    parameters, true, weight: 300).ConfigureAwait(false);
        }

        #endregion

        #region Enable isolated margin account

        /// <inheritdoc />
        public async Task<WebCallResult<CreateIsolatedMarginAccountResult>> EnableIsolatedMarginAccountAsync(string symbol,
            int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

            parameters.AddOptionalParameter("recvWindow",
                receiveWindow?.ToString(CultureInfo.InvariantCulture) ??
                _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient
                .SendRequestInternal<CreateIsolatedMarginAccountResult>(
                    _baseClient.GetUrl(isolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Post, ct,
                    parameters, true, weight: 300).ConfigureAwait(false);
        }

        #endregion

        #region Get enabled isolated margin account

        /// <inheritdoc />
        public async Task<WebCallResult<IsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(
            int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();

            parameters.AddOptionalParameter("recvWindow",
                receiveWindow?.ToString(CultureInfo.InvariantCulture) ??
                _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient
                .SendRequestInternal<IsolatedMarginAccountLimit>(
                    _baseClient.GetUrl(isolatedMarginAccountLimitEndpoint, "sapi", "1"), HttpMethod.Get, ct,
                    parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Isolated margin account transfer

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> IsolatedMarginAccountTransferAsync(string asset,
            string symbol, IsolatedMarginTransferDirection from, IsolatedMarginTransferDirection to, decimal quantity,
            int? receiveWindow = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            symbol.ValidateNotNull(nameof(symbol));

            var parameters = new Dictionary<string, object>
            {
                {"asset", asset},
                {"symbol", symbol},
                {"transFrom", JsonConvert.SerializeObject(from, new IsolatedMarginTransferDirectionConverter(false))},
                {"transTo", JsonConvert.SerializeObject(to, new IsolatedMarginTransferDirectionConverter(false))},
                {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
            parameters.AddOptionalParameter("recvWindow",
                receiveWindow?.ToString(CultureInfo.InvariantCulture) ??
                _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient
                .SendRequestInternal<HitoBitTransaction>(
                    _baseClient.GetUrl(transferIsolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Post, ct,
                    parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Margin order rate limit
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitCurrentRateLimit>>> GetMarginOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitCurrentRateLimit>>(_baseClient.GetUrl(marginOrderRateLimitEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 20).ConfigureAwait(false);
        }
        #endregion

        #region Create a ListenKey

        /// <inheritdoc />
        public async Task<WebCallResult<string>> StartMarginUserStreamAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendRequestInternal<HitoBitListenKey>(_baseClient.GetUrl(getListenKeyEndpoint, "sapi", "1"), HttpMethod.Post, ct).ConfigureAwait(false);
            return result.As(result.Data?.ListenKey!);
        }

        #endregion

        #region Ping/Keep-alive a ListenKey

        /// <inheritdoc />
        public async Task<WebCallResult<object>> KeepAliveMarginUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));

            var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey },
            };

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(keepListenKeyAliveEndpoint, "sapi", "1"), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Invalidate a ListenKey

        /// <inheritdoc />
        public async Task<WebCallResult<object>> StopMarginUserStreamAsync(string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));
            var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(closeListenKeyEndpoint, "sapi", "1"), HttpMethod.Delete, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Create a ListenKey

        /// <inheritdoc />
        public async Task<WebCallResult<string>> StartIsolatedMarginUserStreamAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateHitoBitSymbol();

            var parameters = new Dictionary<string, object>()
            {
                {"symbol", symbol}
            };

            var result = await _baseClient.SendRequestInternal<HitoBitListenKey>(_baseClient.GetUrl(getListenKeyIsolatedEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters).ConfigureAwait(false);
            return result.As(result.Data?.ListenKey!);
        }

        #endregion

        #region Ping/Keep-alive a ListenKey

        /// <inheritdoc />
        public async Task<WebCallResult<object>> KeepAliveIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));
            var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey },
                {"symbol", symbol}
            };

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(keepListenKeyAliveIsolatedEndpoint, "sapi", "1"), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Invalidate a ListenKey
        /// <inheritdoc />
        public async Task<WebCallResult<object>> CloseIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
        {
            listenKey.ValidateNotNull(nameof(listenKey));
            var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey },
                {"symbol", symbol}
            };

            return await _baseClient.SendRequestInternal<object>(_baseClient.GetUrl(closeListenKeyIsolatedEndpoint, "sapi", "1"), HttpMethod.Delete, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Trading status
        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitTradingStatus>>(_baseClient.GetUrl(tradingStatusEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result)
                return result.As<HitoBitTradingStatus>(default);

            return !string.IsNullOrEmpty(result.Data.Message) ? result.AsError<HitoBitTradingStatus>(new ServerError(result.Data.Message!)) : result.As(result.Data.Data);
        }
        #endregion

        #region Order rate limit
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitCurrentRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitCurrentRateLimit>>(_baseClient.GetUrl(orderRateLimitEndpoint, "api", "3"), HttpMethod.Get, ct, parameters, true, weight: 20).ConfigureAwait(false);
        }
        #endregion

        #region Rebate

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitRebateWrapper>> GetRebateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("page", page);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            var result = await _baseClient.SendRequestInternal<HitoBitResult<HitoBitRebateWrapper>>(_baseClient.GetUrl(rebateHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 3000).ConfigureAwait(false);
            if (!result.Success)
                return result.As<HitoBitRebateWrapper>(default);

            if (result.Data?.Code != 0)
                return result.AsError<HitoBitRebateWrapper>(new ServerError(result.Data!.Code, result.Data!.Message));

            return result.As(result.Data.Data);
        }

        #endregion

        #region Blvt

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitBlvtUserLimit>>> GetLeveragedTokensUserLimitAsync(string? tokenName = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("tokenName", tokenName);
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitBlvtUserLimit>>(_baseClient.GetUrl(blvtUserLimitEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);           
        }

        #endregion

        #region Staking

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitStakingResult>> SetAutoStakingAsync(StakingProductType product, string positionId, bool renewable, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) },
                { "positionId", positionId },
                { "renewable", renewable },
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitStakingResult>(_baseClient.GetUrl(setAutoStakingEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitStakingPersonalQuota>> GetStakingPersonalQuotaAsync(StakingProductType product, string productId, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) },
                { "productId", productId }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitStakingPersonalQuota>(_baseClient.GetUrl(stakingQuotaLeftEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
        #endregion

        #region Portfolio margin

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitPortfolioMarginInfo>> GetPortfolioMarginAccountInfoAsync (long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            return await _baseClient.SendRequestInternal<HitoBitPortfolioMarginInfo>(_baseClient.GetUrl(portfolioMarginAccountEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 1).ConfigureAwait(false);
        }
        
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitPortfolioMarginCollateralRate>>> GetPortfolioMarginCollateralRateAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitPortfolioMarginCollateralRate>>(_baseClient.GetUrl(portfolioMarginCollateralRateEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitPortfolioMarginLoan>> GetPortfolioMarginBankruptcyLoanAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            return await _baseClient.SendRequestInternal<HitoBitPortfolioMarginLoan>(_baseClient.GetUrl(portfolioMarginLoanEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 500).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> PortfolioMarginBankruptcyLoanRepayAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            return await _baseClient.SendRequestInternal<HitoBitTransaction>(_baseClient.GetUrl(portfolioMarginRepayEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        }

        #endregion

        #region Get Auto Conversion config

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitAutoConversionSettings>> GetAutoConvertStableCoinConfigAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            return await _baseClient.SendRequestInternal<HitoBitAutoConversionSettings>(_baseClient.GetUrl("capital/contract/convertible-coins", marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 600).ConfigureAwait(false);
        }

        #endregion

        #region Set Auto Conversion config

        /// <inheritdoc />
        public async Task<WebCallResult> SetAutoConvertStableCoinConfigAsync(string asset, bool enable, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "coin", asset },
                { "enable", enable }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            return await _baseClient.SendRequestInternal(_baseClient.GetUrl("capital/contract/convertible-coins", marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 600).ConfigureAwait(false);
        }

        #endregion
    }
}
