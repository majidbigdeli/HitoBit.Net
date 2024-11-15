﻿namespace HitoBit.Net.Objects.Models.Spot.CopyTrading
{
    /// <summary>
    /// Copy trading user status
    /// </summary>
    public record HitoBitCopyTradingUserStatus
    {
        /// <summary>
        /// Is lead trader
        /// </summary>
        [JsonPropertyName("isLeadTrader")]
        public bool IsLeadTrader { get; set; }
        /// <summary>
        /// Time
        /// </summary>
        [JsonPropertyName("time")]
        public long Timestamp { get; set; }
    }
}
