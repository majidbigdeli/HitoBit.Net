﻿using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Futures stream symbol update
    /// </summary>
    public record HitoBitFuturesStreamSymbolUpdate : HitoBitStreamEvent
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Pair
        /// </summary>
        [JsonPropertyName("ps")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("ct")]
        [JsonConverter(typeof(EnumConverter))]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// Delivery date
        /// </summary>
        [JsonPropertyName("dt")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// Onboard date
        /// </summary>
        [JsonPropertyName("ot")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? OnboardDate { get; set; }
        /// <summary>
        /// Symbol status
        /// </summary>
        [JsonPropertyName("cs")]
        [JsonConverter(typeof(EnumConverter))]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Brackets
        /// </summary>
        [JsonPropertyName("bks")]
        public IEnumerable<HitoBitBracketUpdate>? Brackets { get; set; }
    }

    /// <summary>
    /// Bracket update
    /// </summary>
    public record HitoBitBracketUpdate
    {
        /// <summary>
        /// Notional bracket
        /// </summary>
        [JsonPropertyName("bs")]
        public int NotionalBracket { get; set; }
        /// <summary>
        /// Floor notional
        /// </summary>
        [JsonPropertyName("bnf")]
        public decimal FloorNotional { get; set; }
        /// <summary>
        /// Max notional
        /// </summary>
        [JsonPropertyName("bnc")]
        public decimal MaxNotional { get; set; }
        /// <summary>
        /// Maintenance ratio
        /// </summary>
        [JsonPropertyName("mmr")]
        public decimal MaintenanceRatio { get; set; }
        /// <summary>
        /// Min leverage
        /// </summary>
        [JsonPropertyName("mi")]
        public decimal MinLeverage { get; set; }
        /// <summary>
        /// Max leverage
        /// </summary>
        [JsonPropertyName("ma")]
        public decimal MaxLeverage { get; set; }
        /// <summary>
        /// Auxiliary number for quick calculation
        /// </summary>
        [JsonPropertyName("cf")]
        public decimal Auxiliary { get; set; }
    }
}
