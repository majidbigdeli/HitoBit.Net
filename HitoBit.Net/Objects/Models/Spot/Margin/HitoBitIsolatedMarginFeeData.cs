namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Fee data
    /// </summary>
    public record HitoBitIsolatedMarginFeeData
    {
        /// <summary>
        /// Vip level
        /// </summary>
        [JsonPropertyName("vipLevel")]
        public int VipLevel { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonPropertyName("leverage")]
        public int Leverage { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<HitoBitIsolatedMarginFeeInfo> FeeInfo { get; set; } = Array.Empty<HitoBitIsolatedMarginFeeInfo>();
    }

    /// <summary>
    /// Fee info
    /// </summary>
    public record HitoBitIsolatedMarginFeeInfo
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Daily interest
        /// </summary>
        [JsonPropertyName("dailyInterest")]
        public decimal DailyInterest { get; set; }
        /// <summary>
        /// Borrow limit
        /// </summary>
        [JsonPropertyName("borrowLimit")]
        public decimal BorrowLimit { get; set; }
    }
}
