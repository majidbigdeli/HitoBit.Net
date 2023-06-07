using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Margin update
    /// </summary>
    public class HitoBitFuturesStreamMarginUpdate : HitoBitStreamEvent
    {
        /// <summary>
        /// Cross Wallet Balance. Only pushed with crossed position margin call
        /// </summary>
        [JsonProperty("cw")]
        public decimal? CrossWalletBalance { get; set; }
        /// <summary>
        /// Positions
        /// </summary>
        public IEnumerable<HitoBitFuturesStreamMarginPosition> Positions { get; set; } = Array.Empty<HitoBitFuturesStreamMarginPosition>();

        /// <summary>
        /// The listen key the update was for
        /// </summary>
        public string ListenKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// Update data about an margin
    /// </summary>
    public class HitoBitFuturesStreamMarginPosition
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Position Side
        /// </summary>
        [JsonProperty("ps"), JsonConverter(typeof(PositionSideConverter))]
        public PositionSide PositionSide { get; set; }

        /// <summary>
        /// Position quantity
        /// </summary>
        [JsonProperty("pa")]
        public decimal PositionQuantity { get; set; }

        /// <summary>
        /// Margin type
        /// </summary>
        [JsonProperty("mt"), JsonConverter(typeof(FuturesMarginTypeConverter))]
        public FuturesMarginType MarginType { get; set; }

        /// <summary>
        /// Isolated Wallet (if isolated position)
        /// </summary>
        [JsonProperty("iw")]
        public decimal IsolatedWallet { get; set; }

        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonProperty("mp")]
        public decimal MarkPrice { get; set; }

        /// <summary>
        /// Unrealized PnL
        /// </summary>
        [JsonProperty("up")]
        public decimal UnrealizedPnl { get; set; }

        /// <summary>
        /// Maintenance Margin Required
        /// </summary>
        [JsonProperty("mm")]
        public decimal MaintMargin { get; set; }
    }
}
