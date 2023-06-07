﻿using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    // NOTE this is a bit of a weird place for this, however it is a request on the normal client since it uses
    // the /sapi/ route instead of /fapi/. For lack of a better place keep it here

    /// <summary>
    /// Snapshot data of a futures account
    /// </summary>
    public class HitoBitFuturesAccountSnapshot
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
        public HitoBitFuturesAccountSnapshotData Data { get; set; } = default!;
    }

    /// <summary>
    /// Data of the snapshot
    /// </summary>
    public class HitoBitFuturesAccountSnapshotData
    {
        /// <summary>
        /// List of assets
        /// </summary>
        public IEnumerable<HitoBitFuturesAsset> Assets { get; set; } = Array.Empty<HitoBitFuturesAsset>();
        /// <summary>
        /// List of positions
        /// </summary>
        public IEnumerable<HitoBitFuturesSnapshotPosition> Position { get; set; } = Array.Empty<HitoBitFuturesSnapshotPosition>();
    }

    /// <summary>
    /// Asset
    /// </summary>
    public class HitoBitFuturesAsset
    {
        /// <summary>
        /// Name of the asset
        /// </summary>
        public string? Asset { get; set; }
        /// <summary>
        /// Margin balance
        /// </summary>
        public decimal MarginBalance { get; set; }
        /// <summary>
        /// Wallet balance
        /// </summary>
        public decimal? WalletBalance { get; set; }
    }

    /// <summary>
    /// Position
    /// </summary>
    public class HitoBitFuturesSnapshotPosition
    {
        /// <summary>
        /// The symbol
        /// </summary>
        public string? Symbol { get; set; }
        /// <summary>
        /// Entry price
        /// </summary>
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// PositionAmt
        /// </summary>
        public decimal? PositionAmt { get; set; }
        /// <summary>
        /// Unrealized profit
        /// </summary>
        public decimal? UnrealizedProfit { get; set; }
    }
}
