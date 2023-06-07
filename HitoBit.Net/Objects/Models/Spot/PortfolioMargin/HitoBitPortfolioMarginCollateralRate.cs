using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
namespace HitoBit.Net.Objects.Models.Spot.PortfolioMargin
{
    /// <summary>
    /// Portfolio margin collateral rate info
    /// </summary>
    public class HitoBitPortfolioMarginCollateralRate
    {
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// Collateral rate
        /// </summary>
        public decimal CollateralRate { get; set; }
    }
}
