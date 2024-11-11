using HitoBit.Net.Interfaces;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// The order book for a asset
    /// </summary>
    public record HitoBitOrderBook : IHitoBitOrderBook
    {
        /// <summary>
        /// The symbol of the order book 
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the last update
        /// </summary>
        [JsonPropertyName("lastUpdateId")]
        public long LastUpdateId { get; set; }
        
        /// <summary>
        /// The list of bids
        /// </summary>
        [JsonPropertyName("bids")]
        public IEnumerable<HitoBitOrderBookEntry> Bids { get; set; } = Array.Empty<HitoBitOrderBookEntry>();

        /// <summary>
        /// The list of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public IEnumerable<HitoBitOrderBookEntry> Asks { get; set; } = Array.Empty<HitoBitOrderBookEntry>();
    }
}
