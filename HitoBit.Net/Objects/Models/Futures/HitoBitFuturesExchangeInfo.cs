using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Exchange info
    /// </summary>
    public class HitoBitFuturesExchangeInfo
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
        /// Filters
        /// </summary>
        public IEnumerable<object> ExchangeFilters { get; set; } = Array.Empty<object>();
    }

    /// <summary>
    /// Exchange info
    /// </summary>
    public class HitoBitFuturesUsdtExchangeInfo: HitoBitFuturesExchangeInfo
    {
        /// <summary>
        /// All symbols supported
        /// </summary>
        public IEnumerable<HitoBitFuturesUsdtSymbol> Symbols { get; set; } = Array.Empty<HitoBitFuturesUsdtSymbol>();

        /// <summary>
        /// All assets
        /// </summary>
        public IEnumerable<HitoBitFuturesUsdtAsset> Assets { get; set; } = Array.Empty<HitoBitFuturesUsdtAsset>();
    }

    /// <summary>
    /// Exchange info
    /// </summary>
    public class HitoBitFuturesCoinExchangeInfo : HitoBitFuturesExchangeInfo
    {
        /// <summary>
        /// All symbols supported
        /// </summary>
        public IEnumerable<HitoBitFuturesCoinSymbol> Symbols { get; set; } = Array.Empty<HitoBitFuturesCoinSymbol>();
    }
}
