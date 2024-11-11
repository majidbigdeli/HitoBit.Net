namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// Spot Asset Info
    /// </summary>
    public record HitoBitBrokerageSpotAssetInfo
    {
        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<HitoBitBrokerageSubAccountSpotAssetInfo> Data { get; set; } = Array.Empty<HitoBitBrokerageSubAccountSpotAssetInfo>();

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Account Spot Asset Info
    /// </summary>
    public record HitoBitBrokerageSubAccountSpotAssetInfo
    {
        /// <summary>
        /// Sub Account Id
        /// </summary>
        [JsonPropertyName("subaccountId")]
        public string SubAccountId { get; set; } = string.Empty;
        
        /// <summary>
        /// Total Balance Of Btc
        /// </summary>
        [JsonPropertyName("totalBalanceOfBtc")]
        public decimal TotalBalanceOfBtc { get; set; }
    }
}