using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;

namespace HitoBit.Net
{
    /// <summary>
    /// HitoBit exchange information and configuration
    /// </summary>
    public static class HitoBitExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "HitoBit";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.hitobit.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://hitobit-docs.github.io/apidocs/spot/en/#change-log"
            };

        /// <summary>
        /// Format a base and quote asset to a HitoBit recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            if (tradingMode == TradingMode.Spot)
                return baseAsset.ToUpperInvariant() + quoteAsset.ToUpperInvariant();

            if (tradingMode.IsLinear())
                return baseAsset.ToUpperInvariant() + quoteAsset.ToUpperInvariant() + (deliverTime == null ? string.Empty : "_" + deliverTime.Value.ToString("yyMMdd"));

            return baseAsset.ToUpperInvariant() + quoteAsset.ToUpperInvariant() + (deliverTime == null ? "_PERP" : "_" + deliverTime.Value.ToString("yyMMdd"));
        }

        /// <summary>
        /// Rate limiter configuration for the HitoBit API
        /// </summary>
        public static HitoBitRateLimiters RateLimiter { get; } = new HitoBitRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the HitoBit API
    /// </summary>
    public class HitoBitRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal HitoBitRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            EndpointLimit = new RateLimitGate("Endpoint Limit");
            SpotRestIp = new RateLimitGate("Spot Rest")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new PathStartFilter("api/"), 6000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed)) // IP limit of 6000 request weight per minute to /api endpoints
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerEndpoint, new PathStartFilter("sapi/"), 12000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed)); // IP limit of 12000 request weight per endpoint per minute to /sapi endpoints
            SpotRestUid = new RateLimitGate("Spot Rest")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new PathStartFilter("api/"), 6000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed)) // Uid limit of 6000 request weight per minute to /api endpoints
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKeyPerEndpoint, new PathStartFilter("sapi/"), 180000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed)); // Uid limit of 180000 request weight per minute to /sapi endpoints
            SpotSocket = new RateLimitGate("Spot Socket")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new IGuardFilter[] { new LimitItemTypeFilter(RateLimitItemType.Connection) }, 300, TimeSpan.FromMinutes(5), RateLimitWindowType.Fixed)) // 300 connections per 5 minutes per host
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerConnection, new IGuardFilter[] { new HostFilter("wss://stream.hitobit.com"), new LimitItemTypeFilter(RateLimitItemType.Request) }, 4, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding)) // 5 requests per second per path (connection)
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new IGuardFilter[] { new HostFilter("wss://ws-api.hitobit.com") }, 6000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed, connectionWeight: 2)); // 6000 request weight per minute in total
            FuturesRest = new RateLimitGate("Futures Rest")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new IGuardFilter[] { new HostFilter("https://fapi.hitobit.com") }, 2400, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed)) // IP limit of 2400 request weight per minute to fapi.hitobit.com host
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new IGuardFilter[] { new HostFilter("https://dapi.hitobit.com") }, 2400, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed)); // IP limit of 2400 request weight per minute to dapi.hitobit.com host
            FuturesSocket = new RateLimitGate("Futures Socket")
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerConnection, new IGuardFilter[] { new LimitItemTypeFilter(RateLimitItemType.Request), new HostFilter("wss://dstream.hitobit.com") }, 10, TimeSpan.FromSeconds(1), RateLimitWindowType.Fixed)) // 10 requests per second per path (connection)
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerConnection, new IGuardFilter[] { new LimitItemTypeFilter(RateLimitItemType.Request), new HostFilter("wss://fstream.hitobit.com") }, 10, TimeSpan.FromSeconds(1), RateLimitWindowType.Fixed)) // 10 requests per second per path (connection)
                                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new IGuardFilter[] { new HostFilter("wss://ws-fapi.hitobit.com") }, 2400, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed, connectionWeight: 5));

            EndpointLimit.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotRestIp.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotRestUid.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            FuturesRest.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            FuturesSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
        }

        internal IRateLimitGate EndpointLimit { get; private set; }

        internal IRateLimitGate SpotRestIp { get; private set; } 

        internal IRateLimitGate SpotRestUid { get; private set; } 

        internal IRateLimitGate SpotSocket { get; private set; } 

        internal IRateLimitGate FuturesRest { get; private set; } 

        internal IRateLimitGate FuturesSocket { get; private set; } 

    }
}
