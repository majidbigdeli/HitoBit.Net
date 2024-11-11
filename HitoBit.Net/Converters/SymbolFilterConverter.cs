using HitoBit.Net.Enums;
using System.Diagnostics;
using HitoBit.Net.Objects.Models.Spot;
using System.Text.Json;

namespace HitoBit.Net.Converters
{
    internal class SymbolFilterConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type converterType = typeof(SymbolFilterConverterImp<>).MakeGenericType(typeToConvert);
            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        private class SymbolFilterConverterImp<T> : JsonConverter<T>
        {
            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var obj = JsonDocument.ParseValue(ref reader).RootElement;
                var type = obj.GetProperty("filterType").Deserialize<SymbolFilterType>(SerializerOptions.WithConverters);
                HitoBitSymbolFilter result;
                switch (type)
                {
                    case SymbolFilterType.LotSize:
                        result = new HitoBitSymbolLotSizeFilter
                        {
                            MaxQuantity = obj.GetProperty("maxQty").GetDecimal(),
                            MinQuantity = obj.GetProperty("minQty").GetDecimal(),
                            StepSize = obj.GetProperty("stepSize").GetDecimal()
                        };
                        break;
                    case SymbolFilterType.MarketLotSize:
                        result = new HitoBitSymbolMarketLotSizeFilter
                        {
                            MaxQuantity = obj.GetProperty("maxQty").GetDecimal(),
                            MinQuantity = obj.GetProperty("minQty").GetDecimal(),
                            StepSize = obj.GetProperty("stepSize").GetDecimal(),
                        };
                        break;
                    case SymbolFilterType.MinNotional:
                        result = new HitoBitSymbolMinNotionalFilter
                        {
                            MinNotional = obj.GetProperty("minNotional").GetDecimal(),
                            ApplyToMarketOrders = obj.TryGetProperty("applyToMarket", out var applyToMarket) ? applyToMarket.GetBoolean() : null,
                            AveragePriceMinutes = obj.TryGetProperty("avgPriceMins", out var avgPrice) ? avgPrice.GetInt32() : null
                        };
                        break;
                    case SymbolFilterType.Notional:
                        result = new HitoBitSymbolNotionalFilter
                        {
                            MinNotional = obj.GetProperty("minNotional").GetDecimal(),
                            MaxNotional = obj.GetProperty("maxNotional").GetDecimal(),
                            ApplyMinToMarketOrders = obj.GetProperty("applyMinToMarket").GetBoolean(),
                            ApplyMaxToMarketOrders = obj.GetProperty("applyMaxToMarket").GetBoolean(),
                            AveragePriceMinutes = obj.GetProperty("avgPriceMins").GetInt32()
                        };
                        break;
                    case SymbolFilterType.Price:
                        result = new HitoBitSymbolPriceFilter
                        {
                            MaxPrice = obj.GetProperty("maxPrice").GetDecimal(),
                            MinPrice = obj.GetProperty("minPrice").GetDecimal(),
                            TickSize = obj.GetProperty("tickSize").GetDecimal(),
                        };
                        break;
                    case SymbolFilterType.MaxNumberAlgorithmicOrders:
                        result = new HitoBitSymbolMaxAlgorithmicOrdersFilter
                        {
                            MaxNumberAlgorithmicOrders = obj.TryGetProperty("maxNumAlgoOrders", out var algoOrderEl) ? algoOrderEl.GetInt32() : obj.GetProperty("limit").GetInt32()
                        };
                        break;
                    case SymbolFilterType.MaxNumberOrders:
                        result = new HitoBitSymbolMaxOrdersFilter
                        {
                            MaxNumberOrders = obj.TryGetProperty("maxNumOrders", out var orderEl) ? orderEl.GetInt32() : obj.GetProperty("limit").GetInt32()
                        };
                        break;

                    case SymbolFilterType.IcebergParts:
                        result = new HitoBitSymbolIcebergPartsFilter
                        {
                            Limit = obj.GetProperty("limit").GetInt32()
                        };
                        break;
                    case SymbolFilterType.PricePercent:
                        result = new HitoBitSymbolPercentPriceFilter
                        {
                            MultiplierUp = obj.GetProperty("multiplierUp").GetDecimal(),
                            MultiplierDown = obj.GetProperty("multiplierDown").GetDecimal(),
                            AveragePriceMinutes = obj.TryGetProperty("avgPriceMins", out var avgPriceMins) ? avgPriceMins.GetInt32() : null,
                            MultiplierDecimal = obj.TryGetProperty("multiplierDecimal", out var mulDec) ? JsonSerializer.Deserialize<int>(mulDec, options) : null
                        };
                        break;
                    case SymbolFilterType.MaxPosition:
                        result = new HitoBitSymbolMaxPositionFilter
                        {
                            MaxPosition = obj.TryGetProperty("maxPosition", out var el) ? el.GetDecimal(): 0
                        };
                        break;
                    case SymbolFilterType.PercentagePriceBySide:
                        result = new HitoBitSymbolPercentPriceBySideFilter
                        {
                            AskMultiplierUp = obj.GetProperty("askMultiplierUp").GetDecimal(),
                            AskMultiplierDown = obj.GetProperty("askMultiplierDown").GetDecimal(),
                            BidMultiplierUp = obj.GetProperty("bidMultiplierUp").GetDecimal(),
                            BidMultiplierDown = obj.GetProperty("bidMultiplierDown").GetDecimal(),
                            AveragePriceMinutes = obj.GetProperty("avgPriceMins").GetInt32()
                        };
                        break;
                    case SymbolFilterType.TrailingDelta:
                        result = new HitoBitSymbolTrailingDeltaFilter
                        {
                            MaxTrailingAboveDelta = obj.GetProperty("maxTrailingAboveDelta").GetInt32(),
                            MaxTrailingBelowDelta = obj.GetProperty("maxTrailingBelowDelta").GetInt32(),
                            MinTrailingAboveDelta = obj.GetProperty("minTrailingAboveDelta").GetInt32(),
                            MinTrailingBelowDelta = obj.GetProperty("minTrailingBelowDelta").GetInt32(),
                        };
                        break;
                    case SymbolFilterType.IcebergOrders:
                        result = new HitoBitMaxNumberOfIcebergOrdersFilter
                        {
                            MaxNumIcebergOrders = obj.TryGetProperty("maxNumIcebergOrders", out var ele) ? ele.GetInt32() : 0
                        };
                        break;
                    default:
                        Trace.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss:fff} | Warning | Can't parse symbol filter of type: " + obj.GetProperty("filterType").GetString());
                        result = new HitoBitSymbolFilter();
                        break;
                }
#pragma warning restore 8604
                result.FilterType = type;
                return (T)(object)result;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, value!.GetType());
            }
        }
    }
}
