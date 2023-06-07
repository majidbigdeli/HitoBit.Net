using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Account info
    /// </summary>
    public class HitoBitFuturesCoinAccountInfo
    {
        /// <summary>
        /// Can deposit
        /// </summary>
        public bool CanDeposit { get; set; }
        /// <summary>
        /// Can trade
        /// </summary>
        public bool CanTrade { get; set; }
        /// <summary>
        /// Can withdraw
        /// </summary>
        public bool CanWithdraw { get; set; }
        /// <summary>
        /// Fee tier
        /// </summary>
        public int FeeTier { get; set; }
        /// <summary>
        /// Update tier
        /// </summary>
        public int UpdateTier { get; set; }

        /// <summary>
        /// Account assets
        /// </summary>
        public IEnumerable<HitoBitFuturesAccountAsset> Assets { get; set; } = Array.Empty<HitoBitFuturesAccountAsset>();
        /// <summary>
        /// Account positions
        /// </summary>
        public IEnumerable<HitoBitPositionInfoCoin> Positions { get; set; } = Array.Empty<HitoBitPositionInfoCoin>();
        /// <summary>
        /// Update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime UpdateTime { get; set; }
    }
}
