﻿namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Result of setting a countdown timer
    /// </summary>
    public record HitoBitFuturesCountDownResult
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Count down time in milliseconds
        /// </summary>
        [JsonPropertyName("countdownTime")]
        public int CountDownTime { get; set; }
    }
}
