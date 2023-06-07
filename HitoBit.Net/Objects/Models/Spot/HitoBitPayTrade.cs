using System;
using System.Collections.Generic;
using System.Text;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// HitoBit pay trade
    /// </summary>
    public class HitoBitPayTrade
    {
        /// <summary>
        /// Order type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public PayOrderType OrderType { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Fund details
        /// </summary>
        [JsonProperty("fundsDetail")]
        public IEnumerable<HitoBitPayTradeDetails> Details { get; set; } = Array.Empty<HitoBitPayTradeDetails>();
    }

    /// <summary>
    /// Pay trade funds details
    /// </summary>
    public class HitoBitPayTradeDetails
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
    }
}
