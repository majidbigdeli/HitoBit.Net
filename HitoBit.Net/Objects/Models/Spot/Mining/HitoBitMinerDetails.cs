namespace HitoBit.Net.Objects.Models.Spot.Mining
{
    /// <summary>
    /// Miner details
    /// </summary>
    public record HitoBitMinerDetails
    {
        /// <summary>
        /// Name of the worker
        /// </summary>
        [JsonPropertyName("workerName")]
        public string WorkerName { get; set; } = string.Empty;

        /// <summary>
        /// Data type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Hash rate data
        /// </summary>
        [JsonPropertyName("hashRateDatas")]
        public IEnumerable<HitoBitHashRate> HashRateDatas { get; set; } = Array.Empty<HitoBitHashRate>();
    }

    /// <summary>
    /// Hash rate
    /// </summary>
    public record HitoBitHashRate
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Hashrate
        /// </summary>
        [JsonPropertyName("hashRate")]
        public decimal HashRate { get; set; }
        /// <summary>
        /// Rejected
        /// </summary>
        [JsonPropertyName("reject")]
        public decimal Reject { get; set; }
    }
}
