namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Account status info
    /// </summary>
    public record HitoBitAccountStatus
    {
        /// <summary>
        /// The result status
        /// </summary>
        [JsonPropertyName("data")]
        public string? Data { get; set; }
    }
}
