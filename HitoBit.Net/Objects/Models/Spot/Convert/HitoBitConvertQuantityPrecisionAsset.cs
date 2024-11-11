namespace HitoBit.Net.Objects.Models.Spot.Convert
{
    /// <summary>
    /// Precision per asset
    /// </summary>
    public record HitoBitConvertQuantityPrecisionAsset
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Fraction
        /// </summary>
        [JsonPropertyName("fraction")]
        public int Fraction { get; set; }
    }
}
