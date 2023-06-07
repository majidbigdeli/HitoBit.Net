using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Price index for a symbol
    /// </summary>
    public class HitoBitMarginPriceIndex
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Time of calculation
        /// </summary>
        [JsonProperty("calcTime"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime CalculationTime { get; set; }
    }
}
