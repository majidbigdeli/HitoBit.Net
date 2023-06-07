using System;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Open interest
    /// </summary>
    public class HitoBitFuturesOpenInterest
    {
        /// <summary>
        /// The symbol the information is about
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Open Interest info
        /// </summary>
        public decimal OpenInterest { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Open interest
    /// </summary>
    public class HitoBitFuturesCoinOpenInterest: HitoBitFuturesOpenInterest
    {
        /// <summary>
        /// The pair
        /// </summary>
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// The contract type
        /// </summary>
        [JsonConverter(typeof(ContractTypeConverter))]
        public ContractType ContractType { get; set; }
    }

}