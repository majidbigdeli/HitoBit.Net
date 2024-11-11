namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Exchange info
    /// </summary>
    public record HitoBitFuturesExchangeInfo
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
        /// Filters
        /// </summary>
        [JsonPropertyName("exchangeFilters")]
        public IEnumerable<object> ExchangeFilters { get; set; } = Array.Empty<object>();
    }

    /// <summary>
    /// Exchange info
    /// </summary>
    public record HitoBitFuturesUsdtExchangeInfo: HitoBitFuturesExchangeInfo
    {
        /// <summary>
        /// All symbols supported
        /// </summary>
        [JsonPropertyName("symbols")]
        public IEnumerable<HitoBitFuturesUsdtSymbol> Symbols { get; set; } = Array.Empty<HitoBitFuturesUsdtSymbol>();

        /// <summary>
        /// All assets
        /// </summary>
        [JsonPropertyName("assets")]
        public IEnumerable<HitoBitFuturesUsdtAsset> Assets { get; set; } = Array.Empty<HitoBitFuturesUsdtAsset>();
    }

    /// <summary>
    /// Exchange info
    /// </summary>
    public record HitoBitFuturesCoinExchangeInfo : HitoBitFuturesExchangeInfo
    {
        /// <summary>
        /// All symbols supported
        /// </summary>
        [JsonPropertyName("symbols")]
        public IEnumerable<HitoBitFuturesCoinSymbol> Symbols { get; set; } = Array.Empty<HitoBitFuturesCoinSymbol>();
    }
}
