using System;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Loan info
    /// </summary>
    public class HitoBitLoan
    {
        /// <summary>
        /// Isolated symbol
        /// </summary>
        public string? IsolatedSymbol { get; set; }
        /// <summary>
        /// The asset of the loan
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The transaction id of the loan
        /// </summary>
        [JsonProperty("txId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Principal repaid 
        /// </summary>
        public decimal Principal { get; set; }
        /// <summary>
        /// Time of repay completed
        /// </summary>
        [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The status of the loan
        /// </summary>
        [JsonConverter(typeof(MarginStatusConverter))]
        public MarginStatus Status { get; set; }
    }
}
