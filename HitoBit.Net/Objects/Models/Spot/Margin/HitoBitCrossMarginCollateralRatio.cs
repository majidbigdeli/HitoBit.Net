namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Cross margin collateral info
    /// </summary>
    public record HitoBitCrossMarginCollateralRatio
    {
        /// <summary>
        /// Collaterals
        /// </summary>
        [JsonPropertyName("collaterals")]
        public IEnumerable<HitoBitCrossMarginCollateral> Collaterals { get; set; } = Array.Empty<HitoBitCrossMarginCollateral>();
        /// <summary>
        /// Asset names
        /// </summary>
        [JsonPropertyName("assetNames")]
        public IEnumerable<string> AssetNames { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Collateral info
    /// </summary>
    public record HitoBitCrossMarginCollateral
    {
        /// <summary>
        /// Min usd value
        /// </summary>
        [JsonPropertyName("minUsdValue")]
        public decimal MinUsdValue { get; set; }
        /// <summary>
        /// Max usd value
        /// </summary>
        [JsonPropertyName("maxUsdValue")]
        public decimal? MaxUsdValue { get; set; }
        /// <summary>
        /// Discount rate
        /// </summary>
        [JsonPropertyName("discountRate")]
        public decimal DiscountRate { get; set; }
    }
}
