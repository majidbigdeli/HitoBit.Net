namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// User commission rate
    /// </summary>
    public class HitoBitFuturesAccountUserCommissionRate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Maker commission rate
        /// </summary>
        public decimal MakerCommissionRate { get; set; }
        /// <summary>
        /// Taker commission rate
        /// </summary>
        public decimal TakerCommissionRate { get; set; }
    }
}
