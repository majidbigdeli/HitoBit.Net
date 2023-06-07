using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// Transfer Result
    /// </summary>
    public class HitoBitBrokerageTransferResult
    {
        /// <summary>
        /// Transaction Id
        /// </summary>
        [JsonProperty("txnId")]
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// Client Transfer Id
        /// </summary>
        [JsonProperty("clientTranId")]
        public string ClientTransferId { get; set; } = string.Empty;
    }
}