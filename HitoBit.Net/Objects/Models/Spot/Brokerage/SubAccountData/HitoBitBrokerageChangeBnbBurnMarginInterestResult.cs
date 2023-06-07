using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// Enable Or Disable BNB Burn Margin Interest Result
    /// </summary>
    public class HitoBitBrokerageChangeBnbBurnMarginInterestResult
    {
        /// <summary>
        /// Sub Account Id
        /// </summary>
        public string SubAccountId { get; set; } = string.Empty;
        
        /// <summary>
        /// Is Interest BNB Burn
        /// </summary> 
        [JsonProperty("interestBNBBurn")]
        public bool IsInterestBnbBurn { get; set; }
    }
}