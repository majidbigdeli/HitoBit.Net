using System;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Basis info
    /// </summary>
    public class HitoBitFuturesBasis
    {
        /// <summary>
        /// The pair
        /// </summary>
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonConverter(typeof(ContractTypeConverter))]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// Futures price
        /// </summary>
        public decimal FuturesPrice { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Basis
        /// </summary>
        public decimal Basis { get; set; }
        /// <summary>
        /// Basis rate
        /// </summary>
        public decimal BasisRate { get; set; }
        /// <summary>
        /// Data timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
