﻿using HitoBit.Net.Converters;
using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Open Interest History info
    /// </summary>
    public record HitoBitFuturesCoinOpenInterestHistory
    {
        /// <summary>
        /// The symbol the information is about
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;

        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType ContractType { get; set; }

        /// <summary>
        /// Total open interest
        /// </summary>
        [JsonPropertyName("sumOpenInterest")]
        public decimal SumOpenInterest { get; set; }

        /// <summary>
        /// Total open interest value
        /// </summary>
        [JsonPropertyName("sumOpenInterestValue")]
        public decimal SumOpenInterestValue { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }
    }
}
