namespace HitoBit.Net.Objects.Models.Spot.Mining
{
    /// <summary>
    /// Mining account
    /// </summary>
    public record HitoBitMiningAccount
    {
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// User name
        /// </summary>
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Hash rates
        /// </summary>
        [JsonPropertyName("list")]
        public IEnumerable<HitoBitHashRate> Hashrates { get; set; } = Array.Empty<HitoBitHashRate>();
    }
}
