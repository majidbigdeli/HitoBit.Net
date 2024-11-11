﻿using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Trade status
    /// </summary>
    public record HitoBitTradingStatus
    {
        /// <summary>
        /// Is locked
        /// </summary>
        [JsonPropertyName("isLocked")]
        public bool IsLocked { get; set; }
        /// <summary>
        /// Planned time of recovery
        /// </summary>
        [JsonPropertyName("plannedRecoverTime")]
        public int PlannedRecoverTime { get; set; }

        /// <summary>
        /// Conditions
        /// </summary>
        [JsonPropertyName("triggerCondition")]
        public Dictionary<string, int> TriggerConditions { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Dictionary of indicator lists for symbols
        /// </summary>
        [JsonPropertyName("indicators")]
        public Dictionary<string, IEnumerable<HitoBitIndicator>> Indicators { get; set; } = new Dictionary<string, IEnumerable<HitoBitIndicator>>();
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// Indicator info
    /// </summary>
    public record HitoBitIndicator
    {
        /// <summary>
        /// Indicator name
        /// </summary>
        [JsonPropertyName("i")]
        public IndicatorType IndicatorType { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [JsonPropertyName("c")]
        public int Count { get; set; }
        /// <summary>
        /// Current value
        /// </summary>
        [JsonPropertyName("v")]
        public decimal CurrentValue { get; set; }
        /// <summary>
        /// Trigger value
        /// </summary>
        [JsonPropertyName("t")]
        public decimal TriggerValue { get; set; }
    }
}
