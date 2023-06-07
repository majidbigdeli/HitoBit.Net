using System;
using System.Collections.Generic;
using HitoBit.Net.Interfaces;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// The order book for a asset
    /// </summary>
    public class HitoBitFuturesStreamOrderBookDepth : HitoBitStreamEvent, IHitoBitFuturesEventOrderBook
    {
        /// <summary>
        /// The symbol of the order book (only filled from stream updates)
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// The ID of the first update
        /// </summary>
        [JsonProperty("U")]
        public long? FirstUpdateId { get; set; }

        /// <summary>
        /// The ID of the last update
        /// </summary>
        [JsonProperty("u")]
        public long LastUpdateId { get; set; }


        /// <summary>
        /// The ID of the last update Id in last stream
        /// </summary>
        [JsonProperty("pu")]
        public long LastUpdateIdStream { get; set; }


        /// <summary>
        /// The list of diff bids
        /// </summary>
        [JsonProperty("b")]
        public IEnumerable<HitoBitOrderBookEntry> Bids { get; set; } = Array.Empty<HitoBitOrderBookEntry>();

        /// <summary>
        /// The list of diff asks
        /// </summary>
        [JsonProperty("a")]
        public IEnumerable<HitoBitOrderBookEntry> Asks { get; set; } = Array.Empty<HitoBitOrderBookEntry>();
    }
}
