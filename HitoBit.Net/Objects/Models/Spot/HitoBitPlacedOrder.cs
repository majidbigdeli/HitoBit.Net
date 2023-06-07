using System;
using System.Collections.Generic;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// The result of placing a new order
    /// </summary>
    public class HitoBitPlacedOrder: HitoBitOrderBase
    {
        /// <summary>
        /// The time the order was placed
        /// </summary>
        [JsonProperty("transactTime"), JsonConverter(typeof(DateTimeConverter))]
        public new DateTime CreateTime { get; set; }
        
        /// <summary>
        /// Trades for the order
        /// </summary>
        [JsonProperty("fills")]
        public IEnumerable<HitoBitOrderTrade>? Trades { get; set; }

        /// <summary>
        /// Only present if a margin trade happened
        /// </summary>
        [JsonProperty("marginBuyBorrowAmount")]
        public decimal? MarginBuyBorrowQuantity { get; set; }
        /// <summary>
        /// Only present if a margin trade happened
        /// </summary>
        public string? MarginBuyBorrowAsset { get; set; }
        /// <summary>
        /// Self trade prevention mode
        /// </summary>
        [JsonProperty("selfTradePreventionMode")]
        [JsonConverter(typeof(EnumConverter))]
        public SelfTradePreventionMode? SelfTradePreventionMode { get; set; }
    }
}
