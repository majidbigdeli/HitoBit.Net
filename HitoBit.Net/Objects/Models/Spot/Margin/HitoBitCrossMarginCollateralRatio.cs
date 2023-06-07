using System;
using System.Collections.Generic;

namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Cross margin collateral info
    /// </summary>
    public class HitoBitCrossMarginCollateralRatio
    {
        /// <summary>
        /// Collaterals
        /// </summary>
        public IEnumerable<HitoBitCrossMarginCollateral> Collaterals { get; set; } = Array.Empty<HitoBitCrossMarginCollateral>();
        /// <summary>
        /// Asset names
        /// </summary>
        public IEnumerable<string> AssetNames { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Collateral info
    /// </summary>
    public class HitoBitCrossMarginCollateral
    {
        /// <summary>
        /// Min usd value
        /// </summary>
        public decimal MinUsdValue { get; set; }
        /// <summary>
        /// Max usd value
        /// </summary>
        public decimal? MaxUsdValue { get; set; }
        /// <summary>
        /// Discount rate
        /// </summary>
        public decimal DiscountRate { get; set; }
    }
}
