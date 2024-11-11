using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Simple Earn Reward type
    /// </summary>
    public enum RewardType
    {
        /// <summary>
        /// Bonus tiered APR
        /// </summary>
        [Map("BONUS")]
        BonusTieredApr,
        /// <summary>
        /// Realtime APR
        /// </summary>
        [Map("REALTIME")]
        RealtimeApr,
        /// <summary>
        /// Historical rewards
        /// </summary>
        [Map("REWARDS")]
        HistoricalRewards
    }
}
