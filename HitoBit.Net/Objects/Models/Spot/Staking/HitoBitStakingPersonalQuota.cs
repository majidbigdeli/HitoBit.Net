using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Staking
{
    /// <summary>
    /// Quota left
    /// </summary>
    public class HitoBitStakingPersonalQuota
    {
        /// <summary>
        /// Quota left
        /// </summary>
        [JsonProperty("leftPersonalQuota")]
        public decimal PersonalQuotaLeft { get; set; }
    }
}
