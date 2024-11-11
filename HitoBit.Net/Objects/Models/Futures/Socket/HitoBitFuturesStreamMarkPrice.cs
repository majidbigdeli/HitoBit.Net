using HitoBit.Net.Interfaces;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Mark price update
    /// </summary>
    public record HitoBitFuturesStreamMarkPrice: HitoBitStreamEvent, IHitoBitFuturesMarkPrice
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal MarkPrice { get; set; }

        /// <summary>
        /// Estimated Settle Price, only useful in the last hour before the settlement starts
        /// </summary>
        [JsonPropertyName("P")]
        public decimal EstimatedSettlePrice { get; set; }

        /// <summary>
        /// Next Funding Rate
        /// </summary>
        [JsonPropertyName("r")]
        public decimal? FundingRate { get; set; }
        
        /// <summary>
        /// Next Funding Time
        /// </summary>
        [JsonPropertyName("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime NextFundingTime { get; set; }
    }

    /// <summary>
    /// Mark price update
    /// </summary>
    public record HitoBitFuturesUsdtStreamMarkPrice : HitoBitFuturesStreamMarkPrice
    {
        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonPropertyName("i")]
        public decimal IndexPrice { get; set; }
    }

    /// <summary>
    /// Mark price update
    /// </summary>
    public record HitoBitFuturesCoinStreamMarkPrice : HitoBitFuturesStreamMarkPrice
    {
        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonPropertyName("P")]
        public new decimal EstimatedSettlePrice { get; set; }

        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonPropertyName("i")]
        public decimal IndexPrice { get; set; }
    }
}
