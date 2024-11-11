namespace HitoBit.Net.Objects.Models.Spot.SimpleEarn
{
    /// <summary>
    /// Simple Earn personal quota left
    /// </summary>
    public record HitoBitSimpleEarnPersonalQuotaLeft
    {
        /// <summary>
        /// Personal quota left
        /// </summary>
        [JsonPropertyName("leftPersonalQuota")]
        public decimal PersonalQuotaLeft { get; set; }
    }
}
