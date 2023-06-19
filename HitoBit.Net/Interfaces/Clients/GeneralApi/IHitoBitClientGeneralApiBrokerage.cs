﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Enums;
using HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData;
using CryptoExchange.Net.Objects;

namespace HitoBit.Net.Interfaces.Clients.GeneralApi
{
    /// <summary>
    /// HitoBit brokerage endpoints.
    /// </summary>
    public interface IHitoBitClientGeneralApiBrokerage
    {
        /// <summary>
        /// Create a Sub Account
        /// <para>This request will generate a sub account under your brokerage master account</para>
        /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created sub-account id</returns>
        Task<WebCallResult<HitoBitBrokerageSubAccountCreateResult>> CreateSubAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Enable Margin for Sub Account
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Enable Margin result</returns>
        Task<WebCallResult<HitoBitBrokerageEnableMarginResult>> EnableMarginForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Enable Futures for Sub Account
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Enable Futures result</returns>
        Task<WebCallResult<HitoBitBrokerageEnableFuturesResult>> EnableFuturesForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Enable Leverage Token for Sub Account
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Enable Leverage Token result</returns>
        Task<WebCallResult<HitoBitBrokerageEnableLeverageTokenResult>> EnableLeverageTokenForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Create Api Key for Sub Account
        /// <para>This request will generate a api key for a sub account</para>
        /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
        /// <para>Sub account should be enable margin before its api-key's marginTrade being enabled</para>
        /// <para>Sub account should be enable futures before its api-key's futuresTrade being enabled</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="isSpotTradingEnabled">Is spot trading enabled</param>
        /// <param name="isMarginTradingEnabled">Is margin trading enabled</param>
        /// <param name="isFuturesTradingEnabled">Is futures trading enabled</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Api key result</returns>
        Task<WebCallResult<HitoBitBrokerageApiKeyCreateResult>> CreateApiKeyForSubAccountAsync(string subAccountId, bool isSpotTradingEnabled,
            bool? isMarginTradingEnabled = null, bool? isFuturesTradingEnabled = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Delete Sub Account Api Key
        /// <para>This request will delete a api key for a sub account</para>
        /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="apiKey"></param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        Task<object> DeleteSubAccountApiKeyAsync(string subAccountId, string apiKey, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account Api Key
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="size">Size (default 500, max 500)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Api key result</returns>
        Task<WebCallResult<HitoBitBrokerageSubAccountApiKey>> GetSubAccountApiKeyAsync(string subAccountId, string? apiKey = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Change Sub Account Api Permission
        /// <para>This request will change the api permission for a sub account</para>
        /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
        /// <para>Sub account should be enable margin before its api-key's marginTrade being enabled</para>
        /// <para>Sub account should be enable futures before its api-key's futuresTrade being enabled</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="isSpotTradingEnabled">Is spot trading enabled</param>
        /// <param name="isMarginTradingEnabled">Is margin trading enabled</param>
        /// <param name="isFuturesTradingEnabled">Is futures trading enabled</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Api key result</returns>
        Task<WebCallResult<HitoBitBrokerageSubAccountApiKey>> ChangeSubAccountApiKeyPermissionAsync(string subAccountId, string apiKey,
            bool isSpotTradingEnabled, bool isMarginTradingEnabled, bool isFuturesTradingEnabled, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Add IP Restriction for Sub Account Api Key
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="ipAddress">IP address</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Restriction result</returns>
        Task<WebCallResult<HitoBitBrokerageAddIpRestrictionResult>> AddIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Enable or Disable IP Restriction for Sub Account Api Key
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="ipRestrict">IP restrict</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Restriction result</returns>
        Task<WebCallResult<HitoBitBrokerageIpRestriction>> ChangeIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, bool ipRestrict, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get IP Restriction for Sub Account Api Key
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Restriction result</returns>
        Task<WebCallResult<HitoBitBrokerageIpRestriction>> GetIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Delete IP Restriction for Sub Account Api Key
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="apiKey">Api key</param>
        /// <param name="ipAddress">IP address</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Restriction result</returns>
        Task<WebCallResult<HitoBitBrokerageIpRestrictionBase>> DeleteIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
            string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="size">Size (default 500)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Sub accounts</returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccount>>> GetSubAccountsAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Change Sub Account Commission
        /// <para>This request will change the commission for a sub account</para>
        /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
        /// <para>If margin disabled, it is not allowed to send marginMakerCommission or marginTakerCommission</para>
        /// <para>If margin enabled, marginMakerCommission or marginTakerCommission has default value as spotMakerCommission or spotTakerCommission</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="makerCommission">Maker commission</param>
        /// <param name="takerCommission">Taker commission</param>
        /// <param name="marginMakerCommission">Margin maker commission</param>
        /// <param name="marginTakerCommission">Margin taker commission</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Sub account commission result</returns>
        Task<WebCallResult<HitoBitBrokerageSubAccountCommission>> ChangeSubAccountCommissionAsync(string subAccountId, decimal makerCommission, decimal takerCommission,
            decimal? marginMakerCommission = null, decimal? marginTakerCommission = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Change Sub Account USDT-Ⓜ Futures Commission Adjustment
        /// <para>This request will change the USDT-Ⓜ futures commission for a sub account</para>
        /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
        /// <para>The sub-account's USDT-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
        /// <para>If futures disabled, it is not allowed to set subaccount's USDT-Ⓜ futures commission adjustment on any symbol</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="symbol">Symbol</param>
        /// <param name="makerAdjustment">Maker adjustment (100 for 0.01%)</param>
        /// <param name="takerAdjustment">Taker adjustment (100 for 0.01%)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Sub account USDT-Ⓜ futures commission result</returns>
        Task<WebCallResult<HitoBitBrokerageSubAccountFuturesCommission>> ChangeSubAccountFuturesCommissionAdjustmentAsync(string subAccountId, string symbol,
            int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account USDT-Ⓜ Futures Commission Adjustment
        /// <para>The sub-account's USDT-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
        /// <para>If symbol not sent, commission adjustment of all symbols will be returned</para>
        /// <para>If futures disabled, it is not allowed to set subaccount's USDT-Ⓜ futures commission adjustment on any symbol</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="symbol">Symbol</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Sub account USDT-Ⓜ futures commissions result</returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccountFuturesCommission>>> GetSubAccountFuturesCommissionAdjustmentAsync(string subAccountId,
            string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Change Sub Account COIN-Ⓜ Futures Commission Adjustment
        /// <para>This request will change the COIN-Ⓜ futures commission for a sub account</para>
        /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
        /// <para>The sub-account's COIN-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
        /// <para>If futures disabled, it is not allowed to set subaccount's COIN-Ⓜ futures commission adjustment on any symbol</para>
        /// <para>Different symbols have the same commission for the same pair</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="pair">Pair</param>
        /// <param name="makerAdjustment">Maker adjustment (100 for 0.01%)</param>
        /// <param name="takerAdjustment">Taker adjustment (100 for 0.01%)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Sub account coin futures commission result</returns>
        Task<WebCallResult<HitoBitBrokerageSubAccountCoinFuturesCommission>> ChangeSubAccountCoinFuturesCommissionAdjustmentAsync(string subAccountId,
            string pair, int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account COIN-Ⓜ Futures Commission Adjustment
        /// <para>The sub-account's COIN-Ⓜ futures commission of a symbol equals to the base commission of the symbol on the sub-account's fee tier plus the commission adjustment</para>
        /// <para>If pair not sent, commission adjustment of all symbols will be returned</para>
        /// <para>If futures disabled, it is not allowed to set subaccount's COIN-Ⓜ futures commission adjustment on any symbol</para>
        /// <para>Different symbols have the same commission for the same pair</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="pair">Pair</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Sub account coin futures commissions result</returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccountFuturesCommission>>> GetSubAccountCoinFuturesCommissionAdjustmentAsync(string subAccountId,
            string? pair = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Broker Account Information
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Broker information</returns>
        Task<WebCallResult<HitoBitBrokerageAccountInfo>> GetBrokerAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Sub Account Transfer Universal
        /// <para>You need to enable "internal transfer" option for the api key which requests this endpoint</para>
        /// <para>Transfer from master account if fromId not sent</para>
        /// <para>Transfer to master account if toId not sent</para>
        /// <para>Transfer between futures account is not supported</para>
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="fromId">From id</param>
        /// <param name="fromAccountType">From type</param>
        /// <param name="toId">To id</param>
        /// <param name="toAccountType">To type</param>
        /// <param name="clientTransferId">Client transfer id, must be unique</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transfer result</returns>
        Task<WebCallResult<HitoBitBrokerageTransferResult>> TransferUniversalAsync(string asset, decimal quantity,
            string? fromId, BrokerageAccountType fromAccountType, string? toId, BrokerageAccountType toAccountType,
            string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account Transfer History Universal
        /// <para>Either fromId or toId must be sent. Return fromId equal master account by default</para>
        /// <para>Only get the latest history of past 30 days</para>
        /// <para>If showAllStatus is true, the status in response will show four types: INIT,PROCESS,SUCCESS,FAILURE</para>
        /// </summary>
        /// <param name="fromId">From id</param>
        /// <param name="toId">To id</param>
        /// <param name="clientTransferId">Client transfer id</param>
        /// <param name="startDate">From date</param>
        /// <param name="endDate">To date</param>
        /// <param name="page">Page</param>
        /// <param name="limit">Limit (default 500, max 500)</param>
        /// <param name="showAllStatus">Show all status</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transfer history</returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageTransferTransactionUniversal>>> GetTransferHistoryUniversalAsync(
            string? fromId = null, string? toId = null, string? clientTransferId = null, DateTime? startDate = null,
            DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Sub Account Transfer (Spot)
        /// <para>You need to enable "internal transfer" option for the api key which requests this endpoint</para>
        /// <para>Transfer from master account if fromId not sent</para>
        /// <para>Transfer to master account if toId not sent</para>
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="fromId">From id</param>
        /// <param name="toId">To id</param>
        /// <param name="clientTransferId">Client transfer id, must be unique</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transfer result</returns>
        Task<WebCallResult<HitoBitBrokerageTransferResult>> TransferAsync(string asset, decimal quantity,
            string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Sub Account Transfer (Futures)
        /// <para>You need to enable "internal transfer" option for the api key which requests this endpoint</para>
        /// <para>Transfer from master account if fromId not sent</para>
        /// <para>Transfer to master account if toId not sent</para>
        /// <para>Each master account could transfer 5000 times/min</para>
        /// </summary>
        /// <param name="asset">Asset</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="futuresType">Futures type</param>
        /// <param name="fromId">From id</param>
        /// <param name="toId">To id</param>
        /// <param name="clientTransferId">Client transfer id</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transfer result</returns>
        Task<WebCallResult<HitoBitBrokerageTransferFuturesResult>> TransferFuturesAsync(string asset, decimal quantity, HitoBitBrokerageFuturesType futuresType,
            string? fromId, string? toId, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account Transfer History (Spot)
        /// <para>If showAllStatus is true, the status in response will show four types: INIT,PROCESS,SUCCESS,FAILURE</para>
        /// <para>If showAllStatus is false, the status in response will show three types: INIT,PROCESS,SUCCESS</para>
        /// </summary>
        /// <param name="fromId">From id</param>
        /// <param name="toId">To id</param>
        /// <param name="clientTransferId">Client transfer id</param>
        /// <param name="startDate">From date</param>
        /// <param name="endDate">To date</param>
        /// <param name="page">Page</param>
        /// <param name="limit">Limit (default 500, max 500)</param>
        /// <param name="showAllStatus">Show all status</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transfer history</returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageTransferTransaction>>> GetTransferHistoryAsync(string? fromId = null, string? toId = null, string? clientTransferId = null,
            DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account Transfer History (Futures)
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="futuresType">Futures type</param>
        /// <param name="startDate">From date (default 30 days records)</param>
        /// <param name="endDate">To date (default 30 days records)</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="limit">Limit (default 50, max 500)</param>
        /// <param name="clientTransferId">Client transfer id</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transfer history</returns>
        Task<WebCallResult<HitoBitBrokerageTransferFuturesTransactions>> GetTransferFuturesHistoryAsync(string subAccountId,
            HitoBitBrokerageFuturesType futuresType, DateTime? startDate = null, DateTime? endDate = null,
            int? page = null, int? limit = null, string? clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get Sub Account Deposit History
        /// <para>Please notice the default startDate and endDate to make sure that time interval is within 0-7 days</para>
        /// <para>If both startDate and endDate are sent, time between startDate and endDate must be less than 7 days</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="coin">Coin</param>
        /// <param name="status">Status</param>
        /// <param name="startDate">From date (default 7 days from current timestamp)</param>
        /// <param name="endDate">To date (default present timestamp)</param>
        /// <param name="limit">Limit (default 500)</param>
        /// <param name="offset">Offset (default 0)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageSubAccountDepositTransaction>>> GetSubAccountDepositHistoryAsync(string? subAccountId = null,
            string? coin = null, HitoBitBrokerageSubAccountDepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null,
            int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Broker Commission Rebate Recent Record (Spot)
        /// <para>Only get the latest history of past 7 days</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="startDate">From date</param>
        /// <param name="endDate">To date</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="size">Size (default 500, max 500)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Rebates history</returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageRebate>>> GetBrokerCommissionRebatesRecentAsync(string subAccountId,
            DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Broker Futures Commission Rebate Record
        /// </summary>
        /// <param name="futuresType">Futures type</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="size">Size (default 10, max 100)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Rebate records</returns>
        Task<WebCallResult<IEnumerable<HitoBitBrokerageFuturesRebate>>> GetBrokerFuturesCommissionRebatesHistoryAsync(HitoBitBrokerageFuturesType futuresType,
            DateTime startDate, DateTime endDate, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Enable Or Disable BNB Burn for Sub Account SPOT and MARGIN
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="spotBnbBurn">"true" or "false", spot and margin whether use BNB to pay for transaction fees or not</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Result</returns>
        Task<WebCallResult<HitoBitBrokerageChangeBnbBurnSpotAndMarginResult>> ChangeBnbBurnForSubAccountSpotAndMarginAsync(string subAccountId, bool spotBnbBurn,
            int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Enable Or Disable BNB Burn for Sub Account Margin Interest
        /// <para>Sub account must be enabled margin before using this switch</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="interestBnbBurn">"true" or "false", margin loan whether uses BNB to pay for margin interest or not</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Result</returns>
        Task<WebCallResult<HitoBitBrokerageChangeBnbBurnMarginInterestResult>> ChangeBnbBurnForSubAccountMarginInterestAsync(string subAccountId, bool interestBnbBurn,
            int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get BNB Burn Status for Sub Account
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Status</returns>
        Task<WebCallResult<HitoBitBrokerageBnbBurnStatus>> GetBnbBurnStatusForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account Spot Asset info
        /// <para>If subAccountId is not sent, the size must be sent</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="size">Size (default 10, max 20)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Asset info</returns>
        Task<WebCallResult<HitoBitBrokerageSpotAssetInfo>> GetSubAccountSpotAssetInfoAsync(string? subAccountId = null,
            int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account Margin Asset info
        /// <para>If subAccountId is not sent, the size must be sent</para>
        /// </summary>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="size">Size (default 10, max 20)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Asset info</returns>
        Task<WebCallResult<HitoBitBrokerageMarginAssetInfo>> GetSubAccountMarginAssetInfoAsync(string? subAccountId = null,
            int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Query Sub Account Futures Asset info
        /// <para>If subAccountId is not sent, the size must be sent</para>
        /// </summary>
        /// <param name="futuresType">Futures type</param>
        /// <param name="subAccountId">Sub account id</param>
        /// <param name="page">Page (default 1)</param>
        /// <param name="size">Size (default 10, max 20)</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Asset info</returns>
        Task<WebCallResult<HitoBitBrokerageFuturesAssetInfo>> GetSubAccountFuturesAssetInfoAsync(HitoBitBrokerageFuturesType futuresType,
            string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);
    }
}