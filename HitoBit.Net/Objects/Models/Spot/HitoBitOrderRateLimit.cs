namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Rate limit info
    /// </summary>
    public record HitoBitCurrentRateLimit: HitoBitRateLimit
    {
        /// <summary>
        /// The current used amount
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
