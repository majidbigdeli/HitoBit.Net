namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Future hourly interest rate
    /// </summary>
    public record HitoBitFuturesInterestRate
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Next interest rate
        /// </summary>
        [JsonPropertyName("nextHourlyInterestRate")]
        public decimal NextHourlyInterestRate { get; set; }
    }
}
