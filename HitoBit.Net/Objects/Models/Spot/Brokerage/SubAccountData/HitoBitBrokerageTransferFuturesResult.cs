using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// Transfer Futures Result
    /// </summary>
    public class HitoBitBrokerageTransferFuturesResult
    {
        /// <summary>
        /// Transaction Id
        /// </summary>
        [JsonProperty("txnId")]
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Client Transfer Id
        /// </summary>
        [JsonProperty("clientTranId")]
        public string ClientTransferId { get; set; } = string.Empty;
    }
}