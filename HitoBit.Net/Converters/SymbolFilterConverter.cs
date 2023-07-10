using HitoBit.Net.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.Converters
{
    internal class SymbolFilterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
#pragma warning disable 8604, 8602
            var obj = JObject.Load(reader);
            var type = new SymbolFilterTypeConverter(false).ReadString(obj["filterType"].ToString());
            HitoBitSymbolFilter result;
            switch (type)
            {
                case SymbolFilterType.LotSize:
                    result = new HitoBitSymbolLotSizeFilter
                    {
                        MaxQuantity = (decimal)obj["maxQty"],
                        MinQuantity = (decimal)obj["minQty"],
                        StepSize = (decimal)obj["stepSize"]
                    };
                    break;
                case SymbolFilterType.MarketLotSize:
                    result = new HitoBitSymbolMarketLotSizeFilter
                    {
                        MaxQuantity = (decimal)obj["maxQty"],
                        MinQuantity = (decimal)obj["minQty"],
                        StepSize = (decimal)obj["stepSize"]
                    };
                    break;
                case SymbolFilterType.MinNotional:
                    result = new HitoBitSymbolMinNotionalFilter
                    {
                        MinNotional = (decimal)obj["minNotional"],
                        ApplyToMarketOrders = (bool)obj["applyToMarket"],
                        AveragePriceMinutes = (int)obj["avgPriceMins"]
                    };
                    break;
                case SymbolFilterType.Notional:
                    result = new HitoBitSymbolNotionalFilter
                    {
                        MinNotional = (decimal)obj["minNotional"],
                        MaxNotional = (decimal)obj["maxNotional"],
                        ApplyMinToMarketOrders = (bool)obj["applyMinToMarket"],
                        ApplyMaxToMarketOrders = (bool)obj["applyMaxToMarket"],
                        AveragePriceMinutes = (int)obj["avgPriceMins"]
                    };
                    break;
                case SymbolFilterType.Price:
                    result = new HitoBitSymbolPriceFilter
                    {
                        MaxPrice = (decimal)obj["maxPrice"],
                        MinPrice = (decimal)obj["minPrice"],
                        TickSize = (decimal)obj["tickSize"]
                    };
                    break;
                case SymbolFilterType.MaxNumberAlgorithmicOrders:
                    result = new HitoBitSymbolMaxAlgorithmicOrdersFilter
                    {
                        MaxNumberAlgorithmicOrders = (int)obj["maxNumAlgoOrders"]
                    };
                    break;
                case SymbolFilterType.MaxNumberOrders:
                    result = new HitoBitSymbolMaxOrdersFilter
                    {
                        MaxNumberOrders = (int)obj["maxNumOrders"]
                    };
                    break;

                case SymbolFilterType.IcebergParts:
                    result = new HitoBitSymbolIcebergPartsFilter
                    {
                        Limit = (int)obj["limit"]
                    };
                    break;
                case SymbolFilterType.PricePercent:
                    result = new HitoBitSymbolPercentPriceFilter
                    {
                        MultiplierUp = (decimal)obj["multiplierUp"],
                        MultiplierDown = (decimal)obj["multiplierDown"],
                        AveragePriceMinutes = (int)obj["avgPriceMins"]
                    };
                    break;
                case SymbolFilterType.MaxPosition:
                    result = new HitoBitSymbolMaxPositionFilter
                    {
                        MaxPosition = obj.ContainsKey("maxPosition") ? (decimal)obj["maxPosition"] : 0
                    };
                    break;
                case SymbolFilterType.PercentagePriceBySide:
                    result = new HitoBitSymbolPercentPriceBySideFilter
                    {
                        AskMultiplierUp = (decimal)obj["askMultiplierUp"],
                        AskMultiplierDown = (decimal)obj["askMultiplierDown"],
                        BidMultiplierUp = (decimal)obj["bidMultiplierUp"],
                        BidMultiplierDown = (decimal)obj["bidMultiplierDown"],
                        AveragePriceMinutes = (int)obj["avgPriceMins"]
                    };
                    break;
                case SymbolFilterType.TrailingDelta:
                    result = new HitoBitSymbolTrailingDeltaFilter
                    {
                        MaxTrailingAboveDelta = (int)obj["maxTrailingAboveDelta"],
                        MaxTrailingBelowDelta = (int)obj["maxTrailingBelowDelta"],
                        MinTrailingAboveDelta = (int)obj["minTrailingAboveDelta"],
                        MinTrailingBelowDelta = (int)obj["minTrailingBelowDelta"],
                    };
                    break;
                case SymbolFilterType.IcebergOrders:
                    result = new HitoBitMaxNumberOfIcebergOrdersFilter
                    {
                        MaxNumIcebergOrders = obj.ContainsKey("maxNumIcebergOrders") ? (int)obj["maxNumIcebergOrders"] : 0
                    };
                    break;
                default:
                    Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Can't parse symbol filter of type: " + obj["filterType"]);
                    result = new HitoBitSymbolFilter();
                    break;
            }
#pragma warning restore 8604
            result.FilterType = type;
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var filter = (HitoBitSymbolFilter)value!;
            writer.WriteStartObject();

            writer.WritePropertyName("filterType");
            writer.WriteValue(JsonConvert.SerializeObject(filter.FilterType, new SymbolFilterTypeConverter(false)));

            switch (filter.FilterType)
            {
                case SymbolFilterType.LotSize:
                    var lotSizeFilter = (HitoBitSymbolLotSizeFilter)filter;
                    writer.WritePropertyName("maxQty");
                    writer.WriteValue(lotSizeFilter.MaxQuantity);
                    writer.WritePropertyName("minQty");
                    writer.WriteValue(lotSizeFilter.MinQuantity);
                    writer.WritePropertyName("stepSize");
                    writer.WriteValue(lotSizeFilter.StepSize);
                    break;
                case SymbolFilterType.MarketLotSize:
                    var marketLotSizeFilter = (HitoBitSymbolMarketLotSizeFilter)filter;
                    writer.WritePropertyName("maxQty");
                    writer.WriteValue(marketLotSizeFilter.MaxQuantity);
                    writer.WritePropertyName("minQty");
                    writer.WriteValue(marketLotSizeFilter.MinQuantity);
                    writer.WritePropertyName("stepSize");
                    writer.WriteValue(marketLotSizeFilter.StepSize);
                    break;
                case SymbolFilterType.MinNotional:
                    var minNotionalFilter = (HitoBitSymbolMinNotionalFilter)filter;
                    writer.WritePropertyName("minNotional");
                    writer.WriteValue(minNotionalFilter.MinNotional);
                    writer.WritePropertyName("applyToMarket");
                    writer.WriteValue(minNotionalFilter.ApplyToMarketOrders);
                    writer.WritePropertyName("avgPriceMins");
                    writer.WriteValue(minNotionalFilter.AveragePriceMinutes);
                    break;
                case SymbolFilterType.Price:
                    var priceFilter = (HitoBitSymbolPriceFilter)filter;
                    writer.WritePropertyName("maxPrice");
                    writer.WriteValue(priceFilter.MaxPrice);
                    writer.WritePropertyName("minPrice");
                    writer.WriteValue(priceFilter.MinPrice);
                    writer.WritePropertyName("tickSize");
                    writer.WriteValue(priceFilter.TickSize);
                    break;
                case SymbolFilterType.MaxNumberAlgorithmicOrders:
                    var algoFilter = (HitoBitSymbolMaxAlgorithmicOrdersFilter)filter;
                    writer.WritePropertyName("maxNumAlgoOrders");
                    writer.WriteValue(algoFilter.MaxNumberAlgorithmicOrders);
                    break;
                case SymbolFilterType.MaxPosition:
                    var maxPositionFilter = (HitoBitSymbolMaxPositionFilter)filter;
                    writer.WritePropertyName("maxPosition");
                    writer.WriteValue(maxPositionFilter.MaxPosition);
                    break;
                case SymbolFilterType.MaxNumberOrders:
                    var orderFilter = (HitoBitSymbolMaxOrdersFilter)filter;
                    writer.WritePropertyName("maxNumOrders");
                    writer.WriteValue(orderFilter.MaxNumberOrders);
                    break;
                case SymbolFilterType.IcebergParts:
                    var icebergPartsFilter = (HitoBitSymbolIcebergPartsFilter)filter;
                    writer.WritePropertyName("limit");
                    writer.WriteValue(icebergPartsFilter.Limit);
                    break;
                case SymbolFilterType.PricePercent:
                    var pricePercentFilter = (HitoBitSymbolPercentPriceFilter)filter;
                    writer.WritePropertyName("multiplierUp");
                    writer.WriteValue(pricePercentFilter.MultiplierUp);
                    writer.WritePropertyName("multiplierDown");
                    writer.WriteValue(pricePercentFilter.MultiplierDown);
                    writer.WritePropertyName("avgPriceMins");
                    writer.WriteValue(pricePercentFilter.AveragePriceMinutes);
                    break;
                case SymbolFilterType.TrailingDelta:
                    var TrailingDelta = (HitoBitSymbolTrailingDeltaFilter)filter;
                    writer.WritePropertyName("maxTrailingAboveDelta");
                    writer.WriteValue(TrailingDelta.MaxTrailingAboveDelta);
                    writer.WritePropertyName("maxTrailingBelowDelta");
                    writer.WriteValue(TrailingDelta.MaxTrailingBelowDelta);
                    writer.WritePropertyName("minTrailingAboveDelta");
                    writer.WriteValue(TrailingDelta.MinTrailingAboveDelta);
                    writer.WritePropertyName("minTrailingBelowDelta");
                    writer.WriteValue(TrailingDelta.MinTrailingBelowDelta);
                    break;
                case SymbolFilterType.IcebergOrders:
                    var MaxNumIcebergOrders = (HitoBitMaxNumberOfIcebergOrdersFilter)filter;
                    writer.WritePropertyName("maxNumIcebergOrders");
                    writer.WriteValue(MaxNumIcebergOrders.MaxNumIcebergOrders);                   
                    break;
                case SymbolFilterType.PercentagePriceBySide:
                    var pricePercentSideBySideFilter = (HitoBitSymbolPercentPriceBySideFilter)filter;
                    writer.WritePropertyName("askMultiplierUp");
                    writer.WriteValue(pricePercentSideBySideFilter.AskMultiplierUp);
                    writer.WritePropertyName("askMultiplierDown");
                    writer.WriteValue(pricePercentSideBySideFilter.AskMultiplierDown);
                    writer.WritePropertyName("bidMultiplierUp");
                    writer.WriteValue(pricePercentSideBySideFilter.BidMultiplierUp);
                    writer.WritePropertyName("bidMultiplierDown");
                    writer.WriteValue(pricePercentSideBySideFilter.BidMultiplierDown);
                    writer.WritePropertyName("avgPriceMins");
                    writer.WriteValue(pricePercentSideBySideFilter.AveragePriceMinutes);
                    break;
                case SymbolFilterType.Notional:
                    var notionalFilter = (HitoBitSymbolNotionalFilter)filter;
                    writer.WritePropertyName("minNotional");
                    writer.WriteValue(notionalFilter.MinNotional);
                    writer.WritePropertyName("maxNotional");
                    writer.WriteValue(notionalFilter.MaxNotional);
                    writer.WritePropertyName("applyMinToMarketOrders");
                    writer.WriteValue(notionalFilter.ApplyMinToMarketOrders);
                    writer.WritePropertyName("applyMaxToMarketOrders");
                    writer.WriteValue(notionalFilter.ApplyMaxToMarketOrders);
                    writer.WritePropertyName("avgPriceMins");
                    writer.WriteValue(notionalFilter.AveragePriceMinutes);
                    break;
                default:
                    Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Can't write symbol filter of type: " + filter.FilterType);
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
