using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Enums;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Blvt;
using HitoBit.Net.Objects.Models.Spot.IsolatedMargin;
using HitoBit.Net.Objects.Models.Spot.Margin;
using HitoBit.Net.Objects.Models.Spot.PortfolioMargin;
using HitoBit.Net.Objects.Models.Spot.Staking;
using CryptoExchange.Net.Objects;

namespace HitoBit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// HitoBit Spot account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IHitoBitRestClientSpotApiAccount
    {
        /// <summary>
        /// Gets account information, including balances
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#account-information-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The account information</returns>
        Task<WebCallResult<HitoBitAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get a daily account snapshot (balances)
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#daily-account-snapshot-user_data" /></para>
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="limit">The amount of days to retrieve</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get a daily account snapshot (assets)
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#daily-account-snapshot-user_data" /></para>
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="limit">The amount of days to retrieve</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get a daily account snapshot (assets and positions)
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#daily-account-snapshot-user_data" /></para>
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="limit">The amount of days to retrieve</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
            CancellationToken ct = default);

        /// <summary>
        /// Gets the status of the account associated with the api key/secret
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#account-status-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Account status</returns>
        Task<WebCallResult<HitoBitAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get funding wallet assets
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#funding-wallet-user_data" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="needBtcValuation">Return BTC valuation</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of assets</returns>
        Task<WebCallResult<IEnumerable<HitoBitFundingAsset>>> GetFundingWalletAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get permission info for the current API key
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-api-key-permission-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Permission info</returns>
        Task<WebCallResult<HitoBitAPIKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets information of assets for a user
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#all-coins-39-information-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Assets info</returns>
        Task<WebCallResult<IEnumerable<HitoBitUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Retrieve balance info
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#user-asset-user_data" /></para>
        /// </summary>
        /// <param name="asset">Return for this asset</param>
        /// <param name="needBtcValuation">Whether the response should include the BtcValuation. If false (default) BtcValuation will be 0.</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitUserBalance>>> GetBalancesAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default);
        
        /// <summary>
        /// Get asset dividend records
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#asset-dividend-record-user_data" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="startTime">Filter by start time from</param>
        /// <param name="endTime">Filter by end time till</param>
        /// <param name="limit">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dividend records</returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitDividendRecord>>> GetAssetDividendRecordsAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// This request will disable fastwithdraw switch under your account.
        /// You need to enable "trade" option for the api key which requests this endpoint.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#disable-fast-withdraw-switch-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> DisableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// This request will enable fastwithdraw switch under your account.
        /// You need to enable "trade" option for the api key which requests this endpoint.
        ///
        /// When Fast Withdraw Switch is on, transferring funds to a HitoBit account will be done instantly.
        /// There is no on-chain transaction, no transaction ID and no withdrawal fee.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#enable-fast-withdraw-switch-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the history of dust conversions
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#dustlog-user_data" /></para>
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The history of dust conversions</returns>
        Task<WebCallResult<HitoBitDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get assets that can be converted to BNB
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitElligableDusts>> GetAssetsForDustTransferAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Converts dust (small amounts of) assets to BNB 
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#dust-transfer-user_data" /></para>
        /// </summary>
        /// <param name="assets">The assets to convert to BNB</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dust transfer result</returns>
        Task<WebCallResult<HitoBitDustTransferResult>> DustTransferAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the status of the BNB burn switch for spot trading and margin interest
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-bnb-burn-status-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Sets the status of the BNB burn switch for spot trading and margin interest
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#toggle-bnb-burn-on-spot-trade-and-margin-interest-user_data" /></para>
        /// </summary>
        /// <param name="spotTrading">If BNB burning should be enabled for spot trading</param>
        /// <param name="marginInterest">If BNB burning should be enabled for margin interest</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitBnbBurnStatus>> SetBnbBurnStatusAsync(bool? spotTrading = null, bool? marginInterest = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Transfers between accounts
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#user-universal-transfer-user_data" /></para>
        /// </summary>
        /// <param name="type">The type of transfer</param>
        /// <param name="asset">The asset to transfer</param>
        /// <param name="quantity">The quantity to transfer</param>
        /// <param name="fromSymbol">From symbol when transfering from/to isolated margin</param>
        /// <param name="toSymbol">To symbol when transfering from/to isolated margin</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitTransaction>> TransferAsync(UniversalTransferType type, string asset, decimal quantity, string? fromSymbol = null, string? toSymbol = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get transfer history
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-user-universal-transfer-history-user_data" /></para>
        /// </summary>
        /// <param name="type">The type of transfer</param>
        /// <param name="startTime">Filter by startTime</param>
        /// <param name="endTime">Filter by endTime</param>
        /// <param name="page">The page</param>
        /// <param name="pageSize">Results per page</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitTransfer>>> GetTransfersAsync(UniversalTransferType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get Fiat payment history
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-fiat-payments-history-user_data" /></para>
        /// </summary>
        /// <param name="side">Filter by side</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Return a specific page</param>
        /// <param name="limit">The page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitFiatPayment>>> GetFiatPaymentHistoryAsync(OrderSide side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get Fiat deposit/withdrawal history
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#fiat-endpoints" /></para>
        /// </summary>
        /// <param name="side">Filter by side</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Return a specific page</param>
        /// <param name="limit">The page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitFiatWithdrawDeposit>>> GetFiatDepositWithdrawHistoryAsync(TransactionType side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Starts a user stream by requesting a listen key. This listen key can be used in subsequent requests to SubscribeToUserDataUpdates. The stream will close after 60 minutes unless a keep alive is send.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-spot" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Listen key</returns>
        Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Sends a keep alive for the current user stream listen key to keep the stream from closing. Stream auto closes after 60 minutes if no keep alive is send. 30 minute interval for keep alive is recommended.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-spot" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Stops the current user stream
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-spot" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> StopUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Withdraw assets from HitoBit to an address
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#withdraw-user_data" /></para>
        /// </summary>
        /// <param name="asset">The asset to withdraw</param>
        /// <param name="address">The address to send the funds to</param>
        /// <param name="addressTag">Secondary address identifier for assets like XRP,XMR etc.</param>
        /// <param name="withdrawOrderId">Custom client order id</param>
        /// <param name="transactionFeeFlag">When making internal transfer, true for returning the fee to the destination account; false for returning the fee back to the departure account. Default false.</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="network">The network to use</param>
        /// <param name="walletType">The wallet type for withdraw</param>
        /// <param name="name">Description of the address</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdrawal confirmation</returns>
        Task<WebCallResult<HitoBitWithdrawalPlaced>> WithdrawAsync(string asset, string address, decimal quantity, string? withdrawOrderId = null, string? network = null, string? addressTag = null, string? name = null, bool? transactionFeeFlag = null, WalletType? walletType = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the withdrawal history
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#withdraw-history-supporting-network-user_data" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="withdrawOrderId">Filter by withdraw order id</param>
        /// <param name="status">Filter by status</param>
        /// <param name="startTime">Filter start time from</param>
        /// <param name="endTime">Filter end time till</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="limit">Add limit. Default: 1000, Max: 1000</param>
        /// <param name="offset">Add offset</param>
        /// <returns>List of withdrawals</returns>
        Task<WebCallResult<IEnumerable<HitoBitWithdrawal>>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawOrderId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, int? offset = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the deposit history
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#deposit-history-supporting-network-user_data" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="status">Filter by status</param>
        /// <param name="limit">Amount of results</param>
        /// <param name="offset">Offset the results</param>
        /// <param name="startTime">Filter start time from</param>
        /// <param name="endTime">Filter end time till</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of deposits</returns>
        Task<WebCallResult<IEnumerable<HitoBitDeposit>>> GetDepositHistoryAsync(string? asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the deposit address for an asset
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#deposit-address-supporting-network-user_data" /></para>
        /// </summary>
        /// <param name="asset">Asset to get address for</param>
        /// <param name="network">Network</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit address</returns>
        Task<WebCallResult<HitoBitDepositAddress>> GetDepositAddressAsync(string asset, string? network = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get personal margin level information for your account
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-summary-of-margin-account-user_data" /></para>
        /// </summary>
        /// <param name="email">account email</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Margin Level Information</returns>
        Task<WebCallResult<HitoBitMarginLevel>> GetMarginLevelInformationAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Execute transfer between spot account and cross margin account.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#margin-account-trade" /></para>
        /// </summary>
        /// <param name="asset">The asset being transferred, e.g., BTC</param>
        /// <param name="quantity">The quantity to be transferred</param>
        /// <param name="type">TransferDirection (MainToMargin/MarginToMain)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transaction Id</returns>
        Task<WebCallResult<HitoBitTransaction>> CrossMarginTransferAsync(string asset, decimal quantity, TransferDirectionType type, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Borrow. Apply for a loan. 
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#margin-account-borrow-margin" /></para>
        /// </summary>
        /// <param name="asset">The asset being borrow, e.g., BTC</param>
        /// <param name="quantity">The quantity to be borrow</param>
        /// <param name="isIsolated">For isolated margin or not</param>
        /// <param name="symbol">The isolated symbol</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transaction Id</returns>
        Task<WebCallResult<HitoBitTransaction>> MarginBorrowAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Repay loan for margin account.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#margin-account-repay-margin" /></para>
        /// </summary>
        /// <param name="asset">The asset being repay, e.g., BTC</param>
        /// <param name="quantity">The quantity to be borrow</param>
        /// <param name="isIsolated">For isolated margin or not</param>
        /// <param name="symbol">The isolated symbol</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transaction Id</returns>
        Task<WebCallResult<HitoBitTransaction>> MarginRepayAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the history of margin dust conversions
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#margin-dustlog-user_data" /></para>
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The history of dust conversions</returns>
        Task<WebCallResult<HitoBitDustLogList>> GetMarginDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get history of transfers
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#margin-account-cancel-all-open-orders-on-a-symbol-trade" /></para>
        /// </summary>
        /// <param name="direction">The direction of the the transfers to retrieve</param>
        /// <param name="page">Results page</param>
        /// <param name="startTime">Filter by startTime from</param>
        /// <param name="endTime">Filter by endTime from</param>
        /// <param name="limit">Limit of the amount of results</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of transfers</returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitTransferHistory>>> GetCrossMarginTransferHistoryAsync(TransferDirection direction, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query loan records
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-loan-record-user_data" /></para>
        /// </summary>
        /// <param name="asset">The records asset</param>
        /// <param name="transactionId">The id of loan transaction</param>
        /// <param name="startTime">Time to start getting records from</param>
        /// <param name="endTime">Time to stop getting records to</param>
        /// <param name="current">Number of page records</param>
        /// <param name="isolatedSymbol">Filter by isolated symbol</param>
        /// <param name="limit">The records count size need show</param>
        /// <param name="archived">Set to true for archived data from 6 months ago</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Loan records</returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitLoan>>> GetMarginLoansAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10, string? isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query repay records
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-repay-record-user_data" /></para>
        /// </summary>
        /// <param name="asset">The records asset</param>
        /// <param name="transactionId">The id of repay transaction</param>
        /// <param name="startTime">Time to start getting records from</param>
        /// <param name="endTime">Time to stop getting records to</param>
        /// <param name="current">Filter by number</param>
        /// <param name="isolatedSymbol">Filter by isolated symbol</param>
        /// <param name="size">The records count size need show</param>
        /// <param name="archived">Set to true for archived data from 6 months ago</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Repay records</returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitRepay>>> GetMarginRepaysAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? size = null, string? isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get history of interest
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-interest-history-user_data" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="page">Results page</param>
        /// <param name="startTime">Filter by startTime from</param>
        /// <param name="endTime">Filter by endTime from</param>
        /// <param name="isolatedSymbol">Filter by isolated symbol</param>
        /// <param name="limit">Limit of the amount of results</param>
        /// <param name="archived">Set to true for archived data from 6 months ago</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of interest events</returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitInterestHistory>>> GetMarginInterestHistoryAsync(string? asset = null, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get history of interest rate
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-margin-interest-rate-history-user_data" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="vipLevel">Vip level</param>
        /// <param name="startTime">Filter by startTime from</param>
        /// <param name="endTime">Filter by endTime from</param>
        /// <param name="limit">Limit of the amount of results</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of interest rate</returns>
        Task<WebCallResult<IEnumerable<HitoBitInterestRateHistory>>> GetMarginInterestRateHistoryAsync(string asset, string? vipLevel = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get cross margin interest data
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-cross-margin-fee-data-user_data" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset</param>
        /// <param name="vipLevel">Vip level</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitInterestMarginData>>> GetInterestMarginDataAsync(string? asset = null, string? vipLevel = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get history of forced liquidations
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-force-liquidation-record-user_data" /></para>
        /// </summary>
        /// <param name="page">Results page</param>
        /// <param name="startTime">Filter by startTime from</param>
        /// <param name="endTime">Filter by endTime from</param>
        /// <param name="isolatedSymbol">Filter by isolated symbol</param>
        /// <param name="limit">Limit of the amount of results</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of forced liquidations</returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitForcedLiquidation>>> GetMarginForcedLiquidationHistoryAsync(int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query margin account details
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-cross-margin-account-details-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The margin account information</returns>
        Task<WebCallResult<HitoBitMarginAccount>> GetMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query max borrow quantity
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-max-borrow-user_data" /></para>
        /// </summary>
        /// <param name="asset">The records asset</param>
        /// <param name="isolatedSymbol">The isolated symbol</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Max quantity</returns>
        Task<WebCallResult<HitoBitMarginAmount>> GetMarginMaxBorrowAmountAsync(string asset, string? isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query max transfer-out quantity 
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-max-transfer-out-amount-user_data" /></para>
        /// </summary>
        /// <param name="asset">The records asset</param>
        /// <param name="isolatedSymbol">The isolated symbol</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Max quantity</returns>
        Task<WebCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string? isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get isolated margin tier data
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-isolated-margin-tier-data-user_data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="tier">Tier</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitIsolatedMarginTierData>>> GetIsolatedMarginTierDataAsync(string symbol, int? tier = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get history of transfer to and from the isolated margin account
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-isolated-margin-transfer-history-user_data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="asset">Filter by asset</param>
        /// <param name="from">Filter by direction</param>
        /// <param name="to">Filter by direction</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="current">Current page</param>
        /// <param name="limit">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitIsolatedMarginTransfer>>> GetIsolatedMarginAccountTransferHistoryAsync(string symbol, string? asset = null,
                IsolatedMarginTransferDirection? from = null, IsolatedMarginTransferDirection? to = null,
                DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10,
                int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Isolated margin account info
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-isolated-margin-account-info-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(
            int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get max number of enabled isolated margin accounts
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-enabled-isolated-margin-account-limit-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(
            int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Enable an isolated margin account
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#enable-isolated-margin-account-trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to enable isoldated margin account for</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<CreateIsolatedMarginAccountResult>> EnableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Disabled an isolated margin account info
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#disable-isolated-margin-account-trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to enable isoldated margin account for</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<CreateIsolatedMarginAccountResult>> DisableIsolatedMarginAccountAsync(string symbol,
            int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer from or to isolated margin account
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#isolated-margin-account-transfer-margin" /></para>
        /// </summary>
        /// <param name="asset">The asset</param>
        /// <param name="symbol">Isolated symbol</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="quantity">Quantity to transfer</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitTransaction>> IsolatedMarginAccountTransferAsync(string asset,
            string symbol, IsolatedMarginTransferDirection from, IsolatedMarginTransferDirection to, decimal quantity,
            int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get isolated margin order rate limits
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitCurrentRateLimit>>> GetMarginOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Starts a user stream by requesting a listen key. This listen key can be used in subsequent requests to HitoBitSocketClient.Futures.SubscribeToUserDataUpdates. The stream will close after 60 minutes unless a keep alive is send.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-margin" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Listen key</returns>
        Task<WebCallResult<string>> StartMarginUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Sends a keep alive for the current user stream listen key to keep the stream from closing. Stream auto closes after 60 minutes if no keep alive is send. 30 minute interval for keep alive is recommended.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-margin" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> KeepAliveMarginUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Stops the current user stream
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-margin" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> StopMarginUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Starts a user stream  for margin account by requesting a listen key. 
        /// This listen key can be used in subsequent requests to  HitoBitSocketClient.Spot.SubscribeToUserDataUpdates  
        /// The stream will close after 60 minutes unless a keep alive is send.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-isolated-margin" /></para>
        /// </summary>
        /// <param name="symbol">The isolated symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Listen key</returns>
        Task<WebCallResult<string>> StartIsolatedMarginUserStreamAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Sends a keep alive for the current user stream for margin account listen key to keep the stream from closing. 
        /// Stream auto closes after 60 minutes if no keep alive is send. 30 minute interval for keep alive is recommended.
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-isolated-margin" /></para>
        /// </summary>
        /// <param name="symbol">The isolated symbol</param>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> KeepAliveIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Close the user stream for margin account
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#listen-key-isolated-margin" /></para>
        /// </summary>
        /// <param name="symbol">The isolated symbol</param>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<object>> CloseIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Gets the trading status for the current account
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#account-api-trading-status-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The trading status of the account</returns>
        Task<WebCallResult<HitoBitTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get the current used order rate limits
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-current-order-count-usage-trade" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitCurrentRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get rebate history
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-spot-rebate-history-records-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Results per page</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitRebateWrapper>> GetRebateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get leveraged tokens user limits
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-blvt-user-limit-info-user_data" /></para>
        /// </summary>
        /// <param name="tokenName">Filter by token name</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<IEnumerable<HitoBitBlvtUserLimit>>> GetLeveragedTokensUserLimitAsync(string? tokenName = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Set auto staking for a product
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#set-auto-staking-user_data" /></para>
        /// </summary>
        /// <param name="product">The staking product</param>
        /// <param name="positionId">The position</param>
        /// <param name="renewable">Renewable</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitStakingResult>> SetAutoStakingAsync(StakingProductType product, string positionId, bool renewable, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get personal staking quota
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-personal-left-quota-of-staking-product-user_data" /></para>
        /// </summary>
        /// <param name="product">The staking product</param>
        /// <param name="productId">Product id</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitStakingPersonalQuota>> GetStakingPersonalQuotaAsync(StakingProductType product, string productId, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Portfolio margin account info
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#get-portfolio-margin-account-info-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitPortfolioMarginInfo>> GetPortfolioMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get portfolio margin account collateral rates
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#portfolio-margin-collateral-rate-market_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitPortfolioMarginCollateralRate>>> GetPortfolioMarginCollateralRateAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get portfolio margin bankrupty loan amount
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-portfolio-margin-bankruptcy-loan-amount-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitPortfolioMarginLoan>> GetPortfolioMarginBankruptcyLoanAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Repay portfolio margin bankruptcy loan
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#portfolio-margin-bankruptcy-loan-repay" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitTransaction>> PortfolioMarginBankruptcyLoanRepayAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get auto conversion settings
        /// <para><a href="https://HitoBit-docs.github.io/apidocs/spot/en/#query-auto-converting-stable-coins-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitAutoConversionSettings>> GetAutoConvertStableCoinConfigAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Set auto conversion configuration
        /// </summary>
        /// <param name="asset">Asset to configure (USDC, USDP or TUSD)</param>
        /// <param name="enable">Enable or not</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> SetAutoConvertStableCoinConfigAsync(string asset, bool enable, long? receiveWindow = null, CancellationToken ct = default);
    }
}
