using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// BNB Burn Status
    /// </summary>
    public class HitoBitBrokerageBnbBurnStatus
    {
        /// <summary>
        /// Sub Account Id
        /// </summary>
        public string SubAccountId { get; set; } = string.Empty;
        
        /// <summary>
        /// Is Spot BNB Burn
        /// </summary>
        [JsonProperty("spotBNBBurn")]
        public bool IsSpotBnbBurn { get; set; }
        
        /// <summary>
        /// Is Interest BNB Burn
        /// </summary>
        [JsonProperty("interestBNBBurn")]
        public bool IsInterestBnbBurn { get; set; }
    }
}