namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Test order commission info
    /// </summary>
    public record HitoBitTestOrderCommission
    {
        /// <summary>
        /// Standard fee rates on trades from the order
        /// </summary>
        [JsonPropertyName("standardCommissionForOrder")]
        public HitoBitFee StandardFeeForOrder { get; set; } = null!;
        /// <summary>
        /// Tax fee rates on trades from the order
        /// </summary>
        [JsonPropertyName("taxCommissionForOrder")]
        public HitoBitFee TaxFeeForOrder { get; set; } = null!;
        /// <summary>
        /// Discount info
        /// </summary>
        [JsonPropertyName("discount")]
        public HitoBitDiscount Discount { get; set; } = null!;
    }

    /// <summary>
    /// Fee rates
    /// </summary>
    public record HitoBitFee
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
    }

    /// <summary>
    /// Discount info
    /// </summary>
    public record HitoBitDiscount
    {
        /// <summary>
        /// Is discount enabled for the account
        /// </summary>
        [JsonPropertyName("enabledForAccount")]
        public bool EnabledForAccount { get; set; }
        /// <summary>
        /// Is discount enabled for the symbol
        /// </summary>
        [JsonPropertyName("enabledForSymbol")]
        public bool EnabledForSymbol { get; set; }
        /// <summary>
        /// The discount asset
        /// </summary>
        [JsonPropertyName("discountAsset")]
        public string DiscountAsset { get; set; } = string.Empty;
        /// <summary>
        /// Discount rate
        /// </summary>
        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }
    }
}
