using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.Staking;

namespace HitoBit.Net.Interfaces.Clients.GeneralApi
{
    /// <summary>
    /// HitoBit Staking endpoints
    /// </summary>
    public interface IHitoBitRestClientGeneralApiStaking
    {
        /// <summary>
        /// Subscribe to ETH staking
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#subscribe-eth-staking-trade" /></para>
        /// </summary>
        /// <param name="quantity">Amount</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitStakingResult>> SubscribeEthStakingAsync(decimal quantity, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Redeem from ETH staking
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#redeem-eth-trade" /></para>
        /// </summary>
        /// <param name="quantity">Amount</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitStakingResult>> RedeemEthStakingAsync(decimal quantity, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get ETH staking history
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#get-eth-staking-history-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitEthStakingHistory>>> GetEthStakingHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get ETH redemption history
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#get-eth-redemption-history-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitEthRedemptionHistory>>> GetEthRedemptionHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get ETH rewards history
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#get-eth-rewards-distribution-history-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitEthRewardsHistory>>> GetEthRewardsHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get ETH staking quotas
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#get-current-eth-staking-quota-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitEthStakingQuota>> GetEthStakingQuotaAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get Beth rate history
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#get-beth-rate-history-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitBethRateHistory>>> GetBethRateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get eth staking account
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#eth-staking-account-user_data" /></para>
        /// </summary>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitEthStakingAccount>> GetEthStakingAccountAsync(long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Wrap Beth
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#wrap-beth-trade" /></para>
        /// </summary>
        /// <param name="quantity">Quantity to wrap</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitStakingResult>> WrapBethAsync(decimal quantity, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get wrap history
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#get-wbeth-wrap-history-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitBethWrapHistory>>> GetBethWrapHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default);

        /// <summary>
        /// Get unwrap history
        /// <para><a href="https://hitobit-docs.github.io/apidocs/spot/en/#get-wbeth-unwrap-history-user_data" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<HitoBitQueryRecords<HitoBitBethWrapHistory>>> GetBethUnwrapHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default);
    }
}
