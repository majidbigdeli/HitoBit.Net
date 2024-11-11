using HitoBit.Net.Interfaces;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Stream order book
    /// </summary>
    public record HitoBitEventOrderBook: IHitoBitEventOrderBook
    {
        /// <summary>
        /// The id of this update, can be synced with HitoBitClient.Spot.GetOrderBook to update the order book
        /// </summary>
        [JsonPropertyName("U")]
        public long? FirstUpdateId { get; set; }

        /// <summary>
        /// Setter for last update id, need for Json.Net
        /// </summary>
        [JsonPropertyName("u")]
        public long LastUpdateId { get; set; }

        /// <summary>
        /// The symbol of the order book 
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Event type
        /// </summary>
        [JsonPropertyName("e")]
        internal string EventType { get; set; } = string.Empty;

        /// <summary>
        /// Event time of the update
        /// </summary>
        [JsonPropertyName("E"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime EventTime { get; set; }

        /// <summary>
        /// The list of bids
        /// </summary>
        [JsonPropertyName("b")]
        public IEnumerable<HitoBitOrderBookEntry> Bids { get; set; } = Array.Empty<HitoBitOrderBookEntry>();
        /// <summary>
        /// The list of asks
        /// </summary>
        [JsonPropertyName("a")]
        public IEnumerable<HitoBitOrderBookEntry> Asks { get; set; } = Array.Empty<HitoBitOrderBookEntry>();
    }
}
