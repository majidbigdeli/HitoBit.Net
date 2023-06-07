using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    /// <summary>
    /// Transaction
    /// </summary>
    public class HitoBitSubAccountTransaction
    {
        /// <summary>
        /// The transaction id
        /// </summary>
        [JsonProperty("txnId")]
        public string TransactionId { get; set; } = string.Empty;
    }
}
