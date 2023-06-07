using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Margin account snapshot
    /// </summary>
    public class HitoBitMarginAccountSnapshot
    {
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter)), JsonProperty("updateTime")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Account type the data is for
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public AccountType Type { get; set; }
        /// <summary>
        /// Snapshot data
        /// </summary>
        [JsonProperty("data")]
        public HitoBitMarginAccountSnapshotData Data { get; set; } = default!;
    }

    /// <summary>
    /// Margin snapshot data
    /// </summary>
    public class HitoBitMarginAccountSnapshotData
    {
        /// <summary>
        /// The margin level
        /// </summary>
        public decimal MarginLevel { get; set; }
        /// <summary>
        /// Total BTC asset
        /// </summary>
        public decimal TotalAssetOfBtc { get; set; }
        /// <summary>
        /// Total BTC liability
        /// </summary>
        public decimal TotalLiabilityOfBtc { get; set; }
        /// <summary>
        /// Total net BTC asset
        /// </summary>
        public decimal TotalNetAssetOfBtc { get; set; }

        /// <summary>
        /// Assets
        /// </summary>
        public IEnumerable<HitoBitMarginBalance> UserAssets { get; set; } = Array.Empty<HitoBitMarginBalance>();
    }
}
