namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Result
    /// </summary>
    public record HitoBitCrossMarginLeverageResult
    {
        /// <summary>
        /// Success
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
