namespace HitoBit.Net.Objects.Models.Spot.PortfolioMargin
{
    /// <summary>
    /// Portfolio margin collateral rate info
    /// </summary>
    public record HitoBitPortfolioMarginCollateralRate
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// Collateral rate
        /// </summary>
        [JsonPropertyName("collateralRate")]
        public decimal CollateralRate { get; set; }
    }
}
