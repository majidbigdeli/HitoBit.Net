﻿using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot
{
    // NOTE this is a bit of a weird place for this, however it is a request on the normal client since it uses
    // the /sapi/ route instead of /fapi/. For lack of a better place keep it here

    /// <summary>
    /// Snapshot data of a futures account
    /// </summary>
    public record HitoBitFuturesAccountSnapshot
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
        public HitoBitFuturesAccountSnapshotData Data { get; set; } = default!;
    }

    /// <summary>
    /// Data of the snapshot
    /// </summary>
    public record HitoBitFuturesAccountSnapshotData
    {
        /// <summary>
        /// List of assets
        /// </summary>
        [JsonPropertyName("assets")]
        public IEnumerable<HitoBitFuturesAsset> Assets { get; set; } = Array.Empty<HitoBitFuturesAsset>();
        /// <summary>
        /// List of positions
        /// </summary>
        [JsonPropertyName("position")]
        public IEnumerable<HitoBitFuturesSnapshotPosition> Position { get; set; } = Array.Empty<HitoBitFuturesSnapshotPosition>();
    }

    /// <summary>
    /// Asset
    /// </summary>
    public record HitoBitFuturesAsset
    {
        /// <summary>
        /// Name of the asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string? Asset { get; set; }
        /// <summary>
        /// Margin balance
        /// </summary>
        [JsonPropertyName("marginBalance")]
        public decimal MarginBalance { get; set; }
        /// <summary>
        /// Wallet balance
        /// </summary>
        [JsonPropertyName("walletBalance")]
        public decimal? WalletBalance { get; set; }
    }

    /// <summary>
    /// Position
    /// </summary>
    public record HitoBitFuturesSnapshotPosition
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        /// <summary>
        /// Entry price
        /// </summary>
        [JsonPropertyName("entryPrice")]
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// Position amount
        /// </summary>
        [JsonPropertyName("positionAmt")]
        public decimal? PositionAmt { get; set; }
        /// <summary>
        /// Unrealized profit
        /// </summary>
        [JsonPropertyName("unRealizedProfit")]
        public decimal? UnrealizedProfit { get; set; }
    }
}
