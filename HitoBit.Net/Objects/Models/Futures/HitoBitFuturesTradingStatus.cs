﻿namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Trading rules status
    /// </summary>
    public record HitoBitFuturesTradingStatus
    {
        /// <summary>
        /// The trading rule indicators
        /// </summary>
        [JsonPropertyName("indicators")]
        public Dictionary<string, IEnumerable<HitoBitFuturesTradingStatusIndicator>> Indicators { get; set; } = new Dictionary<string, IEnumerable<HitoBitFuturesTradingStatusIndicator>>();
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// Indicator details
    /// </summary>
    public record HitoBitFuturesTradingStatusIndicator
    {
        /// <summary>
        /// Locked
        /// </summary>
        [JsonPropertyName("isLocked")]
        public bool IsLocked { get; set; }
        /// <summary>
        /// Planned time when indicator is unlocked
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("plannedRecoveryTime")]
        public DateTime? PlannedRecoveryTime { get; set; }
        /// <summary>
        /// The indicator name
        /// </summary>
        [JsonPropertyName("indicator")]
        public string Indicator { get; set; } = string.Empty;
        /// <summary>
        /// Current value of the indicator
        /// </summary>
        [JsonPropertyName("value")]
        public decimal Value { get; set; }
        /// <summary>
        /// The trigger value of the indicator
        /// </summary>
        [JsonPropertyName("triggerValue")]
        public decimal TriggerValue { get; set; }
    }
}
