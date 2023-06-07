using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// Spot Asset Info
    /// </summary>
    public class HitoBitBrokerageSpotAssetInfo
    {
        /// <summary>
        /// Data
        /// </summary>
        public IEnumerable<HitoBitBrokerageSubAccountSpotAssetInfo> Data { get; set; } = Array.Empty<HitoBitBrokerageSubAccountSpotAssetInfo>();

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Account Spot Asset Info
    /// </summary>
    public class HitoBitBrokerageSubAccountSpotAssetInfo
    {
        /// <summary>
        /// Sub Account Id
        /// </summary>
        public string SubAccountId { get; set; } = string.Empty;
        
        /// <summary>
        /// Total Balance Of Btc
        /// </summary>
        public decimal TotalBalanceOfBtc { get; set; }
    }
}