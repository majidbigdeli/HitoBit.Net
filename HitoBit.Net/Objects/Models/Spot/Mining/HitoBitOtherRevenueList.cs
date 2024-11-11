using HitoBit.Net.Converters;
using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot.Mining
{
    /// <summary>
    /// Revenue list
    /// </summary>
    public record HitoBitOtherRevenueList
    {
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonPropertyName("totalName")]
        public int TotalNum { get; set; }
        /// <summary>
        /// Page size
        /// </summary>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        /// <summary>
        /// Revenue items
        /// </summary>
        [JsonPropertyName("otherProfits")]
        public IEnumerable<HitoBitOtherRevenueItem> OtherProfits { get; set; } = Array.Empty<HitoBitOtherRevenueItem>();
    }

    /// <summary>
    /// Revenue
    /// </summary>
    public record HitoBitOtherRevenueItem
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Coin
        /// </summary>
        [JsonPropertyName("coinName")]
        public string Coin { get; set; } = string.Empty;
        /// <summary>
        /// Earning type
        /// </summary>
        [JsonPropertyName("type")]
        public EarningType Type { get; set; }
        /// <summary>
        /// Profit quantity
        /// </summary>
        [JsonPropertyName("profitAmount")]
        public decimal ProfitQuantity { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public MinerStatus Status { get; set; }
    }
}
