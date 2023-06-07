using System;
using System.Collections.Generic;
using HitoBit.Net.Interfaces;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// The order book for a asset
    /// </summary>
    public class HitoBitOrderBook : IHitoBitOrderBook
    {
        /// <summary>
        /// The symbol of the order book 
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the last update
        /// </summary>
        [JsonProperty("lastUpdateId")]
        public long LastUpdateId { get; set; }
        
        /// <summary>
        /// The list of bids
        /// </summary>
        public IEnumerable<HitoBitOrderBookEntry> Bids { get; set; } = Array.Empty<HitoBitOrderBookEntry>();

        /// <summary>
        /// The list of asks
        /// </summary>
        public IEnumerable<HitoBitOrderBookEntry> Asks { get; set; } = Array.Empty<HitoBitOrderBookEntry>();
    }
}
