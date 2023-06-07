using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// User auto conversion settings
    /// </summary>
    public class HitoBitAutoConversionSettings
    {
        /// <summary>
        /// Is auto convert enabled
        /// </summary>
        public bool ConvertEnabled { get; set; }
        /// <summary>
        /// Assets
        /// </summary>
        [JsonProperty("coins")]
        public IEnumerable<string> Assets { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Exchange rates
        /// </summary>
        public Dictionary<string, decimal> ExchangeRates { get; set; } = new Dictionary<string, decimal>();
    }
}
