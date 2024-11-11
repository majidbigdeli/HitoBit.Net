using System;
using System.Collections.Generic;
using System.Text;
using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Symbol configuration
    /// </summary>
    public record HitoBitSymbolConfiguration
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Margin type
        /// </summary>
        [JsonPropertyName("marginType")]
        public FuturesMarginType? MarginType { get; set; }
        /// <summary>
        /// Is auto add margin
        /// </summary>
        [JsonPropertyName("isAutoAddMargin")]
        public bool IsAutoAddMargin { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public decimal Leverage { get; set; }
        /// <summary>
        /// Max notional value
        /// </summary>
        [JsonPropertyName("maxNotionalValue")]
        public decimal MaxNotionalValue { get; set; }
    }


}
