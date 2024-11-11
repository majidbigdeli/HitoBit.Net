namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Account info
    /// </summary>
    public record HitoBitFuturesCoinAccountInfo
    {
        /// <summary>
        /// Can deposit
        /// </summary>
        [JsonPropertyName("canDeposit")]
        public bool CanDeposit { get; set; }
        /// <summary>
        /// Can trade
        /// </summary>
        [JsonPropertyName("canTrade")]
        public bool CanTrade { get; set; }
        /// <summary>
        /// Can withdraw
        /// </summary>
        [JsonPropertyName("canWithdraw")]
        public bool CanWithdraw { get; set; }
        /// <summary>
        /// Fee tier
        /// </summary>
        [JsonPropertyName("feeTier")]
        public int FeeTier { get; set; }
        /// <summary>
        /// Update tier
        /// </summary>
        [JsonPropertyName("updateTier")]
        public int UpdateTier { get; set; }

        /// <summary>
        /// Account assets
        /// </summary>
        [JsonPropertyName("assets")]
        public IEnumerable<HitoBitFuturesAccountAsset> Assets { get; set; } = Array.Empty<HitoBitFuturesAccountAsset>();
        /// <summary>
        /// Account positions
        /// </summary>
        [JsonPropertyName("positions")]
        public IEnumerable<HitoBitPositionInfoCoin> Positions { get; set; } = Array.Empty<HitoBitPositionInfoCoin>();
        /// <summary>
        /// Update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("updateTime")]
        public DateTime UpdateTime { get; set; }
    }
}
