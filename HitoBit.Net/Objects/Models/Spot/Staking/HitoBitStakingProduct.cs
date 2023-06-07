using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Staking
{
    /// <summary>
    /// Staking product info
    /// </summary>
    public class HitoBitStakingProduct
    {
        /// <summary>
        /// Project id
        /// </summary>
        public string ProjectId { get; set; } = string.Empty;
        /// <summary>
        /// Product details
        /// </summary>
        [JsonProperty("detail")]
        public HitoBitStakingProductDetails Details { get; set; } = null!;
        /// <summary>
        /// Product quota
        /// </summary>
        public HitoBitStakingQuota Quota { get; set; } = null!;
    }

    /// <summary>
    /// Staking product details
    /// </summary>
    public class HitoBitStakingProductDetails
    {
        /// <summary>
        /// Lock up asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Reward asset
        /// </summary>
        public string RewardAsset { get; set; } = string.Empty;
        /// <summary>
        /// Duration in days
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// Renewable
        /// </summary>
        public bool Renewable { get; set; }
        /// <summary>
        /// Apy
        /// </summary>
        public decimal Apy { get; set; }
    }

    /// <summary>
    /// Staking product quota
    /// </summary>
    public class HitoBitStakingQuota
    {
        /// <summary>
        /// Total Personal quota
        /// </summary>
        [JsonProperty("totalPersonalQuota")]
        public decimal Quota { get; set; }
        /// <summary>
        /// Minimum amount per order
        /// </summary>
        public decimal Minimum { get; set; }
    }
}
