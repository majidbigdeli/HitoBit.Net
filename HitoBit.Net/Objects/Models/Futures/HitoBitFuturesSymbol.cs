using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Information about a futures symbol
    /// </summary>
    public record HitoBitFuturesSymbol
    {
        /// <summary>
        /// Filters for order on this symbol
        /// </summary>
        [JsonPropertyName("filters")]
        public IEnumerable<HitoBitSymbolFilter> Filters { get; set; } = Array.Empty<HitoBitSymbolFilter>();
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonPropertyName("contractType")]
        public ContractType? ContractType { get; set; }
        /// <summary>
        /// The maintenance margin percent
        /// </summary>
        [JsonPropertyName("maintMarginPercent")]
        public decimal MaintMarginPercent { get; set; }
        /// <summary>
        /// The price Precision
        /// </summary>
        [JsonPropertyName("pricePrecision")]
        public int PricePrecision { get; set; }
        /// <summary>
        /// The quantity precision
        /// </summary>
        [JsonPropertyName("quantityPrecision")]
        public int QuantityPrecision { get; set; }
        /// <summary>
        /// The required margin percentage
        /// </summary>
        [JsonPropertyName("requiredMarginPercent")]
        public decimal RequiredMarginPercent { get; set; }
        /// <summary>
        /// The base asset
        /// </summary>
        [JsonPropertyName("baseAsset")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Margin asset
        /// </summary>
        [JsonPropertyName("marginAsset")]
        public string MarginAsset { get; set; } = string.Empty;
        /// <summary>
        /// The quote asset
        /// </summary>
        [JsonPropertyName("quoteAsset")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// The precision of the base asset
        /// </summary>
        [JsonPropertyName("baseAssetPrecision")]
        public int BaseAssetPrecision { get; set; }
        /// <summary>
        /// The precision of the quote asset
        /// </summary>
        [JsonPropertyName("quotePrecision")]
        public int QuoteAssetPrecision { get; set; }
        /// <summary>
        /// Allowed order types
        /// </summary>
        [JsonPropertyName("orderTypes")]
        public IEnumerable<FuturesOrderType> OrderTypes { get; set; } = Array.Empty<FuturesOrderType>();
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Pair
        /// </summary>
        [JsonPropertyName("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Delivery Date
        /// </summary>
        [JsonPropertyName("deliveryDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// Delivery Date
        /// </summary>
        [JsonPropertyName("onboardDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ListingDate { get; set; }
        /// <summary>
        /// Trigger protect
        /// </summary>
        [JsonPropertyName("triggerProtect")]
        public decimal TriggerProtect { get; set; }
        /// <summary>
        /// Currently Empty
        /// </summary>
        [JsonPropertyName("underlyingType")]
        public UnderlyingType UnderlyingType { get; set; }
        /// <summary>
        /// Sub types
        /// </summary>
        [JsonPropertyName("underlyingSubType")]
        public IEnumerable<string> UnderlyingSubType { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Liquidation fee
        /// </summary>
        [JsonPropertyName("liquidationFee")]
        public decimal LiquidationFee { get; set; }
        /// <summary>
        /// The max price difference rate (from mark price) a market order can make
        /// </summary>
        [JsonPropertyName("marketTakeBound")]
        public decimal MarketTakeBound { get; set; }

        /// <summary>
        /// Allowed order time in force
        /// </summary>
        [JsonPropertyName("timeInForce")]
        public IEnumerable<TimeInForce> TimeInForce { get; set; } = Array.Empty<TimeInForce>();
        /// <summary>
        /// Filter for the max accuracy of the price for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolPriceFilter? PriceFilter => Filters.OfType<HitoBitSymbolPriceFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for max accuracy of the quantity for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolLotSizeFilter? LotSizeFilter => Filters.OfType<HitoBitSymbolLotSizeFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for max accuracy of the quantity for this symbol, specifically for market orders
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolMarketLotSizeFilter? MarketLotSizeFilter => Filters.OfType<HitoBitSymbolMarketLotSizeFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for max number of orders for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolMaxOrdersFilter? MaxOrdersFilter => Filters.OfType<HitoBitSymbolMaxOrdersFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for max number of orders for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolMaxAlgorithmicOrdersFilter? MaxAlgoOrdersFilter => Filters.OfType<HitoBitSymbolMaxAlgorithmicOrdersFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for the maximum deviation of the price
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolPercentPriceFilter? PricePercentFilter => Filters.OfType<HitoBitSymbolPercentPriceFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for the maximum deviation of the price
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolMinNotionalFilter? MinNotionalFilter => Filters.OfType<HitoBitSymbolMinNotionalFilter>().FirstOrDefault();
    }

    /// <summary>
    /// Information about a futures symbol
    /// </summary>
    public record HitoBitFuturesUsdtSymbol: HitoBitFuturesSymbol
    {
        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }

        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonPropertyName("settlePlan")]
        public decimal SettlePlan { get; set; }
    }

    /// <summary>
    /// Information about a futures symbol
    /// </summary>
    public record HitoBitFuturesCoinSymbol: HitoBitFuturesSymbol
    {

        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonPropertyName("contractStatus")]
        public SymbolStatus Status { get; set; }

        /// <summary>
        /// Contract size
        /// </summary>
        [JsonPropertyName("contractSize")]
        public int ContractSize { get; set; }

        /// <summary>
        /// Equal quantity precision
        /// </summary>
        [JsonPropertyName("equalQtyPrecision")]
        public int EqualQuantityPrecision { get; set; }
       
    }
}
