using System;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Loans
{
    /// <summary>
    /// Crypto loan income info
    /// </summary>
    public class HitoBitCryptoLoanIncome
    {
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Income type
        /// </summary>
        [JsonConverter(typeof(LoanIncomeTypeConverter))]
        public LoanIncomeType Type { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonProperty("tranId")]
        public string TransactionId { get; set; } = string.Empty;
    }
}
