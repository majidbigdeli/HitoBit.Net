namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Index price update
    /// </summary>
    public record HitoBitFuturesStreamIndexPrice: HitoBitStreamEvent
    {
        /// <summary>
        /// The pair
        /// </summary>
        [JsonPropertyName("i")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// The index price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal IndexPrice { get; set; }
    }
}
