﻿using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Margin account snapshot
    /// </summary>
    public record HitoBitMarginAccountSnapshot
    {
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter)), JsonPropertyName("updateTime")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Account type the data is for
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("type")]
        public AccountType Type { get; set; }
        /// <summary>
        /// Snapshot data
        /// </summary>
        [JsonPropertyName("data")]
        public HitoBitMarginAccountSnapshotData Data { get; set; } = default!;
    }

    /// <summary>
    /// Margin snapshot data
    /// </summary>
    public record HitoBitMarginAccountSnapshotData
    {
        /// <summary>
        /// The margin level
        /// </summary>
        [JsonPropertyName("marginLevel")]
        public decimal MarginLevel { get; set; }
        /// <summary>
        /// Total BTC asset
        /// </summary>
        [JsonPropertyName("totalAssetOfBtc")]
        public decimal TotalAssetOfBtc { get; set; }
        /// <summary>
        /// Total BTC liability
        /// </summary>
        [JsonPropertyName("totalLiabilityOfBtc")]
        public decimal TotalLiabilityOfBtc { get; set; }
        /// <summary>
        /// Total net BTC asset
        /// </summary>
        [JsonPropertyName("totalNetAssetOfBtc")]
        public decimal TotalNetAssetOfBtc { get; set; }

        /// <summary>
        /// Assets
        /// </summary>
        [JsonPropertyName("userAssets")]
        public IEnumerable<HitoBitMarginBalance> UserAssets { get; set; } = Array.Empty<HitoBitMarginBalance>();
    }
}
