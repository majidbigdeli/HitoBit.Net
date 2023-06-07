using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.IsolatedMargin
{
    /// <summary>
    /// Isolated margin account info
    /// </summary>
    public class HitoBitIsolatedMarginAccount
    {
        /// <summary>
        /// Account assets
        /// </summary>
        public IEnumerable<HitoBitIsolatedMarginAccountSymbol> Assets { get; set; } = Array.Empty<HitoBitIsolatedMarginAccountSymbol>();
        /// <summary>
        /// Total btc asset
        /// </summary>
        public decimal TotalAssetOfBtc { get; set; }
        /// <summary>
        /// Total liability
        /// </summary>
        public decimal TotalLiabilityOfBtc { get; set; }
        /// <summary>
        /// Total net asset
        /// </summary>
        public decimal TotalNetAssetOfBtc { get; set; }
    }

    /// <summary>
    /// Isolated margin account symbol
    /// </summary>
    public class HitoBitIsolatedMarginAccountSymbol
    {
        /// <summary>
        /// Base asset
        /// </summary>
        public HitoBitIsolatedMarginAccountAsset BaseAsset { get; set; } = default!;

        /// <summary>
        /// Quote asset
        /// </summary>
        public HitoBitIsolatedMarginAccountAsset QuoteAsset { get; set; } = default!;

        /// <summary>
        /// Symbol name
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Isolated created
        /// </summary>
        public bool IsolatedCreated { get; set; }
        /// <summary>
        /// The margin level
        /// </summary>
        public decimal MarginLevel { get; set; }
        /// <summary>
        /// Margin level status
        /// </summary>
        [JsonConverter(typeof(MarginLevelStatusConverter))]
        public MarginLevelStatus MarginLevelStatus { get; set; }
        /// <summary>
        /// Margin ratio
        /// </summary>
        public decimal MarginRatio { get; set; }
        /// <summary>
        /// Index price
        /// </summary>
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Liquidate price
        /// </summary>
        public decimal LiquidatePrice { get; set; }
        /// <summary>
        /// Liquidate rate
        /// </summary>
        public decimal LiquidateRate { get; set; }
        /// <summary>
        /// If trading is enabled
        /// </summary>
        public bool TradeEnabled { get; set; }
        /// <summary>
        /// Account is enabled
        /// </summary>
        public bool Enabled { get; set; }
    }

    /// <summary>
    /// Isolated margin account asset
    /// </summary>
    public class HitoBitIsolatedMarginAccountAsset
    {
        /// <summary>
        /// Asset name
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// If borrow is enabled
        /// </summary>
        public bool BorrowEnabled { get; set; }
        /// <summary>
        /// Borrowed
        /// </summary>
        public decimal Borrowed { get; set; }
        /// <summary>
        /// Free
        /// </summary>
        public decimal Free { get; set; }
        /// <summary>
        /// Interest
        /// </summary>
        public decimal Interest { get; set; }
        /// <summary>
        /// Locked
        /// </summary>
        public decimal Locked { get; set; }
        /// <summary>
        /// Net asset
        /// </summary>
        public decimal NetAsset { get; set; }
        /// <summary>
        /// Net asset in btc
        /// </summary>
        public decimal NetAssetOfBtc { get; set; }
        /// <summary>
        /// Is repay enabled
        /// </summary>
        public bool RepayEnabled { get; set; }
        /// <summary>
        /// Total asset
        /// </summary>
        public decimal TotalAsset { get; set; }
    }
}
