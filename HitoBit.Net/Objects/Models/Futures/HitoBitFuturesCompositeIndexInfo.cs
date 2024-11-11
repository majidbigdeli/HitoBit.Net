namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Index info
    /// </summary>
    public record HitoBitFuturesCompositeIndexInfo
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter)), JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Component asset
        /// </summary>
        [JsonPropertyName("component")]
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// Base asset list
        /// </summary>
        [JsonPropertyName("baseAssetList")]
        public IEnumerable<HitoBitFuturesCompositeIndexInfoAsset> BaseAssets { get; set; } = Array.Empty<HitoBitFuturesCompositeIndexInfoAsset>();
    }

    /// <summary>
    /// Composite index asset
    /// </summary>
    public record HitoBitFuturesCompositeIndexInfoAsset
    {
        /// <summary>
        /// Base asset name
        /// </summary>
        [JsonPropertyName("baseAsset")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Quote asset name
        /// </summary>
        [JsonPropertyName("quoteAsset")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Weight in quantity
        /// </summary>
        [JsonPropertyName("weightInQuantity")]
        public decimal WeightInQuantity { get; set; }
        /// <summary>
        /// Weight in percentage
        /// </summary>
        [JsonPropertyName("weightInPercentage")]
        public decimal WeightInPercentage { get; set; }
    }
}
