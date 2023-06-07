using HitoBit.Net.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using HitoBit.Net.Objects.Models.Futures;

namespace HitoBit.Net.Converters
{
    internal class SymbolFuturesFilterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
#pragma warning disable 8604, 8602
            var type = new SymbolFilterTypeConverter(false).ReadString(obj["filterType"].ToString());
            HitoBitFuturesSymbolFilter result;
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
                case SymbolFilterType.Price:
                    result = new HitoBitSymbolPriceFilter
                    {
                        MaxPrice = (decimal)obj["maxPrice"],
                        MinPrice = (decimal)obj["minPrice"],
                        TickSize = (decimal)obj["tickSize"]
                    };
                    break;
                case SymbolFilterType.MaxNumberOrders:
                    result = new HitoBitSymbolMaxOrdersFilter
                    {
                        MaxNumberOrders = (int)obj["limit"]
                    };
                    break;
                case SymbolFilterType.PricePercent:
                    result = new HitoBitSymbolPercentPriceFilter
                    {
                        MultiplierUp = (decimal)obj["multiplierUp"],
                        MultiplierDown = (decimal)obj["multiplierDown"],
                        MultiplierDecimal = (int)obj["multiplierDecimal"]
                    };
                    break;
                case SymbolFilterType.MaxNumberAlgorithmicOrders:
                    result = new HitoBitSymbolMaxAlgorithmicOrdersFilter
                    {
                        MaxNumberAlgorithmicOrders = (int)obj["limit"]
                    };
                    break;
                case SymbolFilterType.MinNotional:
                    result = new HitoBitSymbolMinNotionalFilter
                    {
                        MinNotional = obj.ContainsKey("notional") ? (decimal)obj["notional"] : 0
                    };
                    break;
                default:
                    Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Can't parse symbol filter of type: " + obj["filterType"]);
                    result = new HitoBitFuturesSymbolFilter();
                    break;
            }
#pragma warning restore 8604
            result.FilterType = type;
            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var filter = (HitoBitFuturesSymbolFilter?)value;
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
                    writer.WritePropertyName("limit");
                    writer.WriteValue(algoFilter.MaxNumberAlgorithmicOrders);
                    break;
                case SymbolFilterType.MaxNumberOrders:
                    var orderFilter = (HitoBitSymbolMaxOrdersFilter)filter;
                    writer.WritePropertyName("limit");
                    writer.WriteValue(orderFilter.MaxNumberOrders);
                    break;
                case SymbolFilterType.PricePercent:
                    var pricePercentFilter = (HitoBitSymbolPercentPriceFilter)filter;
                    writer.WritePropertyName("multiplierUp");
                    writer.WriteValue(pricePercentFilter.MultiplierUp);
                    writer.WritePropertyName("multiplierDown");
                    writer.WriteValue(pricePercentFilter.MultiplierDown);
                    writer.WritePropertyName("multiplierDecimal");
                    writer.WriteValue(pricePercentFilter.MultiplierDecimal);
                    break;
                case SymbolFilterType.MinNotional:
                    var minNotional = (HitoBitSymbolMinNotionalFilter)filter;
                    writer.WritePropertyName("notional");
                    writer.WriteValue(minNotional.MinNotional);
                    break;
                default:
                    Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Can't write symbol filter of type: " + filter.FilterType);
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
