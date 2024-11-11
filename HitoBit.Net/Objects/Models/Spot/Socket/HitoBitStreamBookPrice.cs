using HitoBit.Net.Interfaces;

namespace HitoBit.Net.Objects.Models.Spot.Socket
{
    /// <summary>
    /// Book tick
    /// </summary>
    public record HitoBitStreamBookPrice: IHitoBitBookPrice
    {
        /// <summary>
        /// Update id
        /// </summary>
        [JsonPropertyName("u")]
        public long UpdateId { get; set; }
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Price of the best bid
        /// </summary>
        [JsonPropertyName("b")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Quantity of the best bid
        /// </summary>
        [JsonPropertyName("B")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Price of the best ask
        /// </summary>
        [JsonPropertyName("a")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Quantity of the best ask
        /// </summary>
        [JsonPropertyName("A")]
        public decimal BestAskQuantity { get; set; }
    }
}
