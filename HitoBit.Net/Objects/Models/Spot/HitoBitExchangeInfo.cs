namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Exchange info
    /// </summary>
    public record HitoBitExchangeInfo
    {
        /// <summary>
        /// The timezone the server uses
        /// </summary>
        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; } = string.Empty;
        /// <summary>
        /// The current server time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("serverTime")]
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// The rate limits used
        /// </summary>
        [JsonPropertyName("rateLimits")]
        public IEnumerable<HitoBitRateLimit> RateLimits { get; set; } = Array.Empty<HitoBitRateLimit>();
        /// <summary>
        /// All symbols supported
        /// </summary>
        [JsonPropertyName("symbols")]
        public IEnumerable<HitoBitSymbol> Symbols { get; set; } = Array.Empty<HitoBitSymbol>();
        /// <summary>
        /// Filters
        /// </summary>
        [JsonPropertyName("exchangeFilters")]
        public IEnumerable<object> ExchangeFilters { get; set; } = Array.Empty<object>();
    }
}
