using HitoBit.Net.Interfaces;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Recent trade info
    /// </summary>
    public abstract record HitoBitRecentTrade: IHitoBitRecentTrade
    {
        /// <summary>
        /// The id of the trade
        /// </summary>
        [JsonPropertyName("id")]
        public long OrderId { get; set; }
        /// <summary>
        /// The price of the trade
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <inheritdoc />
        public abstract decimal BaseQuantity { get; set; }
        /// <inheritdoc />
        public abstract decimal QuoteQuantity { get; set; }
        /// <summary>
        /// The timestamp of the trade
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TradeTime { get; set; }
        /// <summary>
        /// Whether the buyer is maker
        /// </summary>
        [JsonPropertyName("isBuyerMaker")]
        public bool BuyerIsMaker { get; set; }
        /// <summary>
        /// Whether the trade was made at the best match
        /// </summary>
        [JsonPropertyName("isBestMatch")]
        public bool IsBestMatch { get; set; }
    }

    /// <summary>
    /// Recent trade with quote quantity
    /// </summary>
    public record HitoBitRecentTradeQuote : HitoBitRecentTrade
    {
        /// <inheritdoc />
        [JsonPropertyName("quoteQty")]
        public override decimal QuoteQuantity { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("qty")]
        public override decimal BaseQuantity { get; set; }
    }

    /// <summary>
    /// Recent trade with base quantity
    /// </summary>
    public record HitoBitRecentTradeBase : HitoBitRecentTrade
    {
        /// <inheritdoc />
        [JsonPropertyName("qty")]
        public override decimal QuoteQuantity { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("baseQty")]
        public override decimal BaseQuantity { get; set; }
    }
}
