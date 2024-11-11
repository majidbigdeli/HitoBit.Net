using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Book price
    /// </summary>
    public record HitoBitFuturesBookPrice: HitoBitBookPrice
    {
        /// <summary>
        /// Pair
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;
    }
}
