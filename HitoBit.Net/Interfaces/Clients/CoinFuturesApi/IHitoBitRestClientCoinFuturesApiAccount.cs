﻿using HitoBit.Net.Enums;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures;

namespace HitoBit.Net.Interfaces.Clients.CoinFuturesApi
{
    /// <summary>
    /// HitoBit COIN-M futures account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IHitoBitRestClientCoinFuturesApiAccount
    {
        /// <summary>
        /// Gets account position information
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#position-information-user_data" /></para>
        /// </summary>
        /// <param name="marginAsset">Filter by margin asset, for example `ETH`</param>
        /// <param name="pair">Filter by pair, for example `BTCUSD`</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of Positions</returns>
        Task<WebCallResult<IEnumerable<HitoBitPositionDetailsCoin>>> GetPositionInformationAsync(string? marginAsset = null, string? pair = null,
            long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets account information, including balances
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#account-information-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The account information</returns>
        Task<WebCallResult<HitoBitFuturesCoinAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>.
        /// Gets account balances
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#futures-account-balance-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The account information</returns>
        Task<WebCallResult<IEnumerable<HitoBitCoinFuturesAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Change user's position mode (Hedge Mode or One-way Mode ) on EVERY symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#change-position-mode-trade" /></para>
        /// </summary>
        /// <param name="dualPositionSide">User position mode</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Whether the request was successful</returns>
        Task<WebCallResult<HitoBitResult>> ModifyPositionModeAsync(bool dualPositionSide, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get user's position mode (Hedge Mode or One-way Mode ) on EVERY symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#get-current-position-mode-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Whether the request was successful</returns>
        Task<WebCallResult<HitoBitFuturesPositionMode>> GetPositionModeAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Requests to change the initial leverage of the given symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#change-initial-leverage-trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to change the initial leverage for, for example `BTCUSD_PERP`</param>
        /// <param name="leverage">The amount of initial leverage to change to</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Result of the initial leverage change request</returns>
        Task<WebCallResult<HitoBitFuturesInitialLeverageChangeResult>> ChangeInitialLeverageAsync(string symbol, int leverage, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Change the margin type for an open position
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#change-margin-type-trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to change the position type for, for example `BTCUSD_PERP`</param>
        /// <param name="marginType">The type of margin to use</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Whether the request was successful</returns>
        Task<WebCallResult<HitoBitFuturesChangeMarginTypeResult>> ChangeMarginTypeAsync(string symbol, FuturesMarginType marginType, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Change the margin on an open position
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#modify-isolated-position-margin-trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to adjust the position margin for, for example `BTCUSD_PERP`</param>
        /// <param name="amount">The amount of margin to be used</param>
        /// <param name="type">Whether to reduce or add margin to the position</param>
        /// <param name="positionSide">Default BOTH for One-way Mode ; LONG or SHORT for Hedge Mode. It must be sent with Hedge Mode.</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The new position margin</returns>
        Task<WebCallResult<HitoBitFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal amount, FuturesMarginChangeDirectionType type, PositionSide? positionSide = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Requests the margin change history for a specific symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#get-position-margin-change-history-trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to get margin history for, for example `BTCUSD_PERP`</param>
        /// <param name="type">Filter the history by the direction of margin change</param>
        /// <param name="startTime">Margin changes newer than this date will be retrieved</param>
        /// <param name="endTime">Margin changes older than this date will be retrieved</param>
        /// <param name="limit">The max number of results</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of all margin changes for the symbol</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, FuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the income history for the futures account
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#get-income-history-user_data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get income history from, for example `BTCUSD_PERP`</param>
        /// <param name="incomeType">The income type filter to apply to the request</param>
        /// <param name="startTime">Time to start getting income history from</param>
        /// <param name="endTime">Time to stop getting income history from</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The income history for the futures account</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, string? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets Notional and Leverage Brackets.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#notional-bracket-for-pair-user_data" /></para>
        /// </summary>
        /// <param name="symbolOrPair">The symbol or pair to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Notional and Leverage Brackets</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesSymbolBracket>>> GetBracketsAsync(string? symbolOrPair = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get position ADL quantile estimations
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#position-adl-quantile-estimation-user_data" /></para>
        /// </summary>
        /// <param name="symbol">Only get for this symbol, for example `BTCUSD_PERP`</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(
            string? symbol = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Gets account commission rates
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#user-commission-rate-user_data" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `BTCUSD_PERP`</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>User commission rate information</returns>
        Task<WebCallResult<HitoBitFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Start a user stream. The resulting listen key can be used to subscribe to the user stream using the socket client.The stream will close after 60 minutes unless <see cref="KeepAliveUserStreamAsync">KeepAliveUserStreamAsync</see> is called.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#start-user-data-stream-user_stream" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

        /// <summary>
        /// Keep alive the user stream. This should be called every 30 minutes to prevent the user stream being stopped
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#keepalive-user-data-stream-user_stream" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key to keep alive</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Stop the user stream, no updates will be send anymore
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#close-user-data-stream-user_stream" /></para>
        /// </summary>
        /// <param name="listenKey">The listen key to stop</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default);

        /// <summary>
        /// Get download id for downloading transaction history
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#get-download-id-for-futures-transaction-history-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Start time of the data to download</param>
        /// <param name="endTime">End time of the data to download</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get the download link for transaction history by download id
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#get-futures-transaction-history-download-link-by-id-user_data" /></para>
        /// </summary>
        /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTransactionHistoryAsync" /></param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get download id for downloading order history
        /// <para><a href="https://developers.hitobit.com/docs/derivatives/coin-margined-futures/account/Get-Download-Id-For-Futures-Order-History" /></para>
        /// </summary>
        /// <param name="startTime">Start time of the data to download</param>
        /// <param name="endTime">End time of the data to download</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get the download link for order history by download id
        /// <para><a href="https://developers.hitobit.com/docs/derivatives/coin-margined-futures/account/Get-Futures-Order-History-Download-Link-by-Id" /></para>
        /// </summary>
        /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForOrderHistoryAsync" /></param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get download id for downloading trade history
        /// <para><a href="https://developers.hitobit.com/docs/derivatives/coin-margined-futures/account/Get-Download-Id-For-Futures-Trade-History" /></para>
        /// </summary>
        /// <param name="startTime">Start time of the data to download</param>
        /// <param name="endTime">End time of the data to download</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get the download link for order history by download id
        /// <para><a href="https://developers.hitobit.com/docs/derivatives/coin-margined-futures/account/Get-Futures-Trade-Download-Link-by-Id" /></para>
        /// </summary>
        /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTradeHistoryAsync" /></param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default);

    }
}
