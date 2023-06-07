using Newtonsoft.Json;
using System;

namespace HitoBit.Net.Objects.Models.Spot.PortfolioMargin
{
    /// <summary>
    /// Bankruptcy loan info
    /// </summary>
    public class HitoBitPortfolioMarginLoan
    {
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Loan amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
    }
}
