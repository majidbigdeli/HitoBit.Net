﻿using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// Futures Asset Info
    /// </summary>
    public class HitoBitBrokerageFuturesAssetInfo
    {
        /// <summary>
        /// Data
        /// </summary>
        public IEnumerable<HitoBitBrokerageSubAccountFuturesAssetInfo> Data { get; set; } = Array.Empty<HitoBitBrokerageSubAccountFuturesAssetInfo>();

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Account Futures Asset Info
    /// </summary>
    public class HitoBitBrokerageSubAccountFuturesAssetInfo
    {
        /// <summary>
        /// Sub Account Id
        /// </summary>
        public string SubAccountId { get; set; } = string.Empty;
        
        /// <summary>
        /// Futures enable
        /// </summary>
        [JsonProperty("futuresEnable")]
        public bool IsFuturesEnable { get; set; }
        
        /// <summary>
        /// Total Initial Margin Of Usdt
        /// </summary>
        public decimal TotalInitialMarginOfUsdt { get; set; }
        
        /// <summary>
        /// Total Maintenance Margin Of Usdt
        /// </summary>
        public decimal TotalMaintenanceMarginOfUsdt { get; set; }
        
        /// <summary>
        /// Total Wallet Balance Of Usdt
        /// </summary>
        public decimal TotalWalletBalanceOfUsdt { get; set; }
        
        /// <summary>
        /// Total Unrealized Profit Of Usdt
        /// </summary>
        public decimal TotalUnrealizedProfitOfUsdt { get; set; }
        
        /// <summary>
        /// Total Margin Balance Of Usdt
        /// </summary>
        public decimal TotalMarginBalanceOfUsdt { get; set; }
        
        /// <summary>
        /// Total Position Initial Margin Of Usdt
        /// </summary>
        public decimal TotalPositionInitialMarginOfUsdt { get; set; }
        
        /// <summary>
        /// Total Open Order Initial Margin Of Usdt
        /// </summary>
        public decimal TotalOpenOrderInitialMarginOfUsdt { get; set; }
    }
}