using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Book price
    /// </summary>
    public class HitoBitFuturesBookPrice: HitoBitBookPrice
    {
        /// <summary>
        /// Pair
        /// </summary>
        public string Pair { get; set; } = string.Empty;
    }
}
