namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// The price of a symbol
    /// </summary>
    public record HitoBitPrice
    {
        /// <summary>
        /// The symbol the price is for
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The price of the symbol
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Futures-Coin price
    /// </summary>
    public record HitoBitFuturesCoinPrice: HitoBitPrice
    {
        /// <summary>
        /// Name of the pair
        /// </summary>
        [JsonPropertyName("ps")]
        public string Pair { get; set; } = string.Empty;
    }
}
