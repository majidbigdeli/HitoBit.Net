using System;
using System.Collections.Generic;
using System.Linq;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public class HitoBitSymbol
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonConverter(typeof(SymbolStatusConverter))]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// The base asset
        /// </summary>
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// The precision of the base asset
        /// </summary>
        public int BaseAssetPrecision { get; set; }
        /// <summary>
        /// The quote asset
        /// </summary>
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// The precision of the quote asset
        /// </summary>
        [JsonProperty("quotePrecision")]
        public int QuoteAssetPrecision { get; set; }

        /// <summary>
        /// Allowed order types
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(SpotOrderTypeConverter))]
        public IEnumerable<SpotOrderType> OrderTypes { get; set; } = Array.Empty<SpotOrderType>();
        /// <summary>
        /// Ice berg orders allowed
        /// </summary>
        public bool IceBergAllowed { get; set; }
        /// <summary>
        /// Cancel replace allowed
        /// </summary>
        public bool CancelReplaceAllowed { get; set; }
        /// <summary>
        /// Spot trading orders allowed
        /// </summary>
        public bool IsSpotTradingAllowed { get; set; }
        /// <summary>
        /// Trailling stop orders are allowed
        /// </summary>
        public bool AllowTrailingStop { get; set; }
        /// <summary>
        /// Margin trading orders allowed
        /// </summary>
        public bool IsMarginTradingAllowed { get; set; }
        /// <summary>
        /// If OCO(One Cancels Other) orders are allowed
        /// </summary>
        public bool OCOAllowed { get; set; }
        /// <summary>
        /// Whether or not it is allowed to specify the quantity of a market order in the quote asset
        /// </summary>
        [JsonProperty("quoteOrderQtyMarketAllowed")]
        public bool QuoteOrderQuantityMarketAllowed { get; set; }
        /// <summary>
        /// The precision of the base asset fee
        /// </summary>
        [JsonProperty("baseCommissionPrecision")]
        public int BaseFeePrecision { get; set; }
        /// <summary>
        /// The precision of the quote asset fee
        /// </summary>
        [JsonProperty("quoteCommissionPrecision")]
        public int QuoteFeePrecision { get; set; }
        /// <summary>
        /// Permissions types
        /// </summary>
        [JsonProperty(ItemConverterType = typeof(EnumConverter))]
        public IEnumerable<AccountType> Permissions { get; set; } = Array.Empty<AccountType>();
        /// <summary>
        /// Filters for order on this symbol
        /// </summary>
        public IEnumerable<HitoBitSymbolFilter> Filters { get; set; } = Array.Empty<HitoBitSymbolFilter>();
        /// <summary>
        /// Default self trade prevention
        /// </summary>
        [JsonProperty("defaultSelfTradePreventionMode")]
        [JsonConverter(typeof(EnumConverter))]
        public SelfTradePreventionMode DefaultSelfTradePreventionMode { get; set; }
        /// <summary>
        /// Allowed self trade prevention modes
        /// </summary>
        [JsonProperty("allowedSelfTradePreventionModes", ItemConverterType = typeof(EnumConverter))]
        public IEnumerable<SelfTradePreventionMode> AllowedSelfTradePreventionModes { get; set; } = Array.Empty<SelfTradePreventionMode>();
        /// <summary>
        /// Filter for max amount of iceberg parts for this symbol
        /// </summary>
        [JsonIgnore]        
        public HitoBitSymbolIcebergPartsFilter? IceBergPartsFilter => Filters.OfType<HitoBitSymbolIcebergPartsFilter>().FirstOrDefault();
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
        /// Filter for max algorithmic orders for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolMaxAlgorithmicOrdersFilter? MaxAlgorithmicOrdersFilter => Filters.OfType<HitoBitSymbolMaxAlgorithmicOrdersFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for the minimal quote quantity of an order for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolMinNotionalFilter? MinNotionalFilter => Filters.OfType<HitoBitSymbolMinNotionalFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for the minimal quote quantity of an order for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolNotionalFilter? NotionalFilter => Filters.OfType<HitoBitSymbolNotionalFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for the max accuracy of the price for this symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolPriceFilter? PriceFilter => Filters.OfType<HitoBitSymbolPriceFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for the maximum deviation of the price
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolPercentPriceFilter? PricePercentFilter => Filters.OfType<HitoBitSymbolPercentPriceFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for the maximum deviation of the price per side
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolPercentPriceBySideFilter? PricePercentByPriceFilter => Filters.OfType<HitoBitSymbolPercentPriceBySideFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for the maximum position on a symbol
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolMaxPositionFilter? MaxPositionFilter => Filters.OfType<HitoBitSymbolMaxPositionFilter>().FirstOrDefault();
        /// <summary>
        /// Filter for the trailing delta values
        /// </summary>
        [JsonIgnore]
        public HitoBitSymbolTrailingDeltaFilter? TrailingDeltaFilter => Filters.OfType<HitoBitSymbolTrailingDeltaFilter>().FirstOrDefault();
    }
}
