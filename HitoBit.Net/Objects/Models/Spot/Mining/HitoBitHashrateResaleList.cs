using HitoBit.Net.Converters;
using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot.Mining
{
    /// <summary>
    /// Resale list
    /// </summary>
    public record HitoBitHashrateResaleList
    {
        /// <summary>
        /// Total number of results
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Details
        /// </summary>
        [JsonPropertyName("configDetails")]
        public IEnumerable<HitoBitHashrateResaleItem> ResaleItmes { get; set; } = Array.Empty<HitoBitHashrateResaleItem>();
    }

    /// <summary>
    /// Resale item
    /// </summary>
    public record HitoBitHashrateResaleItem
    {
        /// <summary>
        /// Mining id
        /// </summary>
        [JsonPropertyName("configId")]
        public int ConfigId { get; set; }
        /// <summary>
        /// From user
        /// </summary>
        [JsonPropertyName("poolUsername")]
        public string PoolUserName { get; set; } = string.Empty;
        /// <summary>
        /// To user
        /// </summary>
        [JsonPropertyName("toPoolUsername")]
        public string ToPoolUserName { get; set; } = string.Empty;
        /// <summary>
        /// Algorithm
        /// </summary>
        [JsonPropertyName("algoName")]
        public string AlgoName { get; set; } = string.Empty;
        /// <summary>
        /// Hash rate
        /// </summary>
        [JsonPropertyName("hashRate")]
        public decimal Hashrate { get; set; }
        /// <summary>
        /// Start day
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("startDay")]
        public DateTime StartDay { get; set; }
        /// <summary>
        /// End day
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("endDay")]
        public DateTime EndDay { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public HashrateResaleStatus Status { get; set; }
    }
}
