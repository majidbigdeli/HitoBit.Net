using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Mining
{
    /// <summary>
    /// Revenue list
    /// </summary>
    public class HitoBitOtherRevenueList
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
        /// Revenue items
        /// </summary>
        public IEnumerable<HitoBitOtherRevenueItem> OtherProfits { get; set; } = Array.Empty<HitoBitOtherRevenueItem>();
    }

    /// <summary>
    /// Revenue
    /// </summary>
    public class HitoBitOtherRevenueItem
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Coin
        /// </summary>
        [JsonProperty("coinName")]
        public string Coin { get; set; } = string.Empty;
        /// <summary>
        /// Earning type
        /// </summary>
        [JsonConverter(typeof(HitoBitEarningTypeConverter))]
        public HitoBitEarningType Type { get; set; }
        /// <summary>
        /// Profit quantity
        /// </summary>
        [JsonProperty("profitAmount")]
        public decimal ProfitQuantity { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public MinerStatus Status { get; set; }
    }
}
