using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// HitoBit commissions
    /// </summary>
    public record HitoBitCommissions
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Standard commission rates on trades from the order.
        /// </summary>
        [JsonPropertyName("standardCommission")]
        public HitoBitCommissionInfo StandardCommissions { get; set; } = null!;
        /// <summary>
        /// Tax commission rates for trades from the order.
        /// </summary>
        [JsonPropertyName("taxCommission")]
        public HitoBitCommissionInfo TaxCommissions { get; set; } = null!;
        /// <summary>
        /// Discount commission when paying in BNB
        /// </summary>
        [JsonPropertyName("discount")]
        public HitoBitDiscountInfo Discount { get; set; } = null!;

    }

    /// <summary>
    /// Commission info
    /// </summary>
    public record HitoBitDiscountInfo
    {
        /// <summary>
        /// Standard commission is reduced by this rate when paying commission in BNB.
        /// </summary>
        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }
        /// <summary>
        /// Enabled for account
        /// </summary>
        [JsonPropertyName("enabledForAccount")]
        public bool EnabledForAccount { get; set; }
        /// <summary>
        /// Enabled for symbol
        /// </summary>
        [JsonPropertyName("enabledForSymbol")]
        public bool EnabledForSymbol { get; set; }
        /// <summary>
        /// Discount asset
        /// </summary>
        [JsonPropertyName("discountAsset")]
        public string DiscountAsset { get; set; } = string.Empty;
    }

    /// <summary>
    /// Commission info
    /// </summary>
    public record HitoBitCommissionInfo
    {
        /// <summary>
        /// Maker fee
        /// </summary>
        [JsonPropertyName("maker")]
        public decimal Maker { get; set; }
        /// <summary>
        /// Taker fee
        /// </summary>
        [JsonPropertyName("taker")]
        public decimal Taker { get; set; }
        /// <summary>
        /// Buyer fee
        /// </summary>
        [JsonPropertyName("buyer")]
        public decimal Buyer { get; set; }
        /// <summary>
        /// Seller fee
        /// </summary>
        [JsonPropertyName("seller")]
        public decimal Sell { get; set; }
    }
}
