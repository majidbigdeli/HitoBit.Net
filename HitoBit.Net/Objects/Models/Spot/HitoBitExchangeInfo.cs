using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Exchange info
    /// </summary>
    public class HitoBitExchangeInfo
    {
        /// <summary>
        /// The timezone the server uses
        /// </summary>
        public string TimeZone { get; set; } = string.Empty;
        /// <summary>
        /// The current server time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// The rate limits used
        /// </summary>
        public IEnumerable<HitoBitRateLimit> RateLimits { get; set; } = Array.Empty<HitoBitRateLimit>();
        /// <summary>
        /// All symbols supported
        /// </summary>
        public IEnumerable<HitoBitSymbol> Symbols { get; set; } = Array.Empty<HitoBitSymbol>();
        /// <summary>
        /// Filters
        /// </summary>
        public IEnumerable<object> ExchangeFilters { get; set; } = Array.Empty<object>();
    }
}
