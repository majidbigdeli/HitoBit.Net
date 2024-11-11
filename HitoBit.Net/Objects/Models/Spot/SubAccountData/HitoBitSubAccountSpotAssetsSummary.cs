namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    /// <summary>
    /// Sub accounts btc value summary
    /// </summary>
    public record HitoBitSubAccountSpotAssetsSummary
    {
        /// <summary>
        /// Total records
        /// </summary>
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
        /// <summary>
        /// Master account total asset value
        /// </summary>
        [JsonPropertyName("masterAccountTotalAsset")]
        public decimal MasterAccountTotalAsset { get; set; }
        /// <summary>
        /// Sub account values
        /// </summary>
        [JsonPropertyName("spotSubUserAssetBtcVoList")]
        public IEnumerable<HitoBitSubAccountBtcValue> SubAccountsBtcValues { get; set; } = Array.Empty<HitoBitSubAccountBtcValue>();
    }

    /// <summary>
    /// Sub account btc value
    /// </summary>
    public record HitoBitSubAccountBtcValue
    {
        /// <summary>
        /// Sub account email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Sub account total asset 
        /// </summary>
        [JsonPropertyName("totalAsset")]
        public decimal TotalAsset { get; set; }
    }
}
