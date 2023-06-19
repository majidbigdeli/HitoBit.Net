﻿using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Information about an account
    /// </summary>
    public class HitoBitFuturesAccountBalance
    {
        /// <summary>
        /// Account alias
        /// </summary>
        public string AccountAlias { get; set; } = string.Empty;

        /// <summary>
        /// The asset this balance is for
        /// </summary>
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// The total balance of this asset
        /// </summary>
        [JsonProperty("balance")]
        public decimal WalletBalance { get; set; }

        /// <summary>
        /// Crossed wallet balance
        /// </summary>
        public decimal CrossWalletBalance { get; set; }

        /// <summary>
        /// Unrealized profit of crossed positions
        /// </summary>
        [JsonProperty("crossUnPnl")]
        public decimal CrossUnrealizedPnl { get; set; }

        /// <summary>
        /// Available balance
        /// </summary>
        public decimal AvailableBalance { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// Usd futures account balance
    /// </summary>
    public class HitoBitUsdFuturesAccountBalance : HitoBitFuturesAccountBalance
    {
        /// <summary>
        /// Maximum quantity for transfer out
        /// </summary>
        [JsonProperty("maxWithdrawAmount")]
        public decimal MaxWithdrawQuantity { get; set; }

        /// <summary>
        /// Whether the asset can be used as margin in Multi-Assets mode
        /// </summary>
        public bool? MarginAvailable { get; set; }
    }

    /// <summary>
    /// Coin futures account balance
    /// </summary>
    public class HitoBitCoinFuturesAccountBalance : HitoBitFuturesAccountBalance
    {
        /// <summary>
        /// Available for withdraw
        /// </summary>
        public decimal WithdrawAvailable { get; set; }
    }
}