namespace HitoBit.Net.Objects.Models.Spot.SimpleEarn
{
    /// <summary>
    /// Simple Earn result
    /// </summary>
    public record HitoBitSimpleEarnResult
    {
        /// <summary>
        /// Result
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
