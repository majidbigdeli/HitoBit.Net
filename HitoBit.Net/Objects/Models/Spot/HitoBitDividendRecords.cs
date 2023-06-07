using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Dividend record
    /// </summary>
    public class HitoBitDividendRecord
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp of the transaction
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter)), JsonProperty("divTime")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonProperty("tranId")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Info
        /// </summary>
        [JsonProperty("enInfo")]
        public string? Info { get; set; }
    }
}
