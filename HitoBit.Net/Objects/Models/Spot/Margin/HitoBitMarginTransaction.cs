using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// The result of transferring
    /// </summary>
    public class HitoBitTransaction
    {
        /// <summary>
        /// The Transaction id as assigned by HitoBit
        /// </summary>
        [JsonProperty("tranId")]
        public long TransactionId { get; set; }
    }
}
