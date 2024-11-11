using HitoBit.Net.Interfaces;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// The order book for a asset
    /// </summary>
    public record HitoBitFuturesStreamOrderBookDepth : HitoBitStreamEvent, IHitoBitFuturesEventOrderBook
    {
        /// <summary>
        /// The symbol of the order book (only filled from stream updates)
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonPropertyName("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// The ID of the first update
        /// </summary>
        [JsonPropertyName("U")]
        public long? FirstUpdateId { get; set; }

        /// <summary>
        /// The ID of the last update
        /// </summary>
        [JsonPropertyName("u")]
        public long LastUpdateId { get; set; }


        /// <summary>
        /// The ID of the last update Id in last stream
        /// </summary>
        [JsonPropertyName("pu")]
        public long LastUpdateIdStream { get; set; }


        /// <summary>
        /// The list of diff bids
        /// </summary>
        [JsonPropertyName("b")]
        public IEnumerable<HitoBitOrderBookEntry> Bids { get; set; } = Array.Empty<HitoBitOrderBookEntry>();

        /// <summary>
        /// The list of diff asks
        /// </summary>
        [JsonPropertyName("a")]
        public IEnumerable<HitoBitOrderBookEntry> Asks { get; set; } = Array.Empty<HitoBitOrderBookEntry>();
    }
}
