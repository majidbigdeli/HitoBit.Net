using CryptoExchange.Net.Objects.Options;
using System.Collections.Generic;
using System.Net.Http;
using System;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace HitoBit.Net.Objects.Options
{
    /// <summary>
    /// Options for the HitoBitSocketClient
    /// </summary>
    public class HitoBitSocketOptions : SocketExchangeOptions<HitoBitEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static HitoBitSocketOptions Default { get; set; } = new HitoBitSocketOptions()
        {
            Environment = HitoBitEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public HitoBitSocketApiOptions SpotOptions { get; private set; } = new HitoBitSocketApiOptions()
        {
            RateLimiters = new List<IRateLimiter>
            {
                new RateLimiter()
                    .AddConnectionRateLimit("stream.binance.com", 5, TimeSpan.FromSeconds(1))
                    .AddConnectionRateLimit("ws-api.binance.com", 1200, TimeSpan.FromSeconds(60))
            }
        };

        /// <summary>
        /// Options for the Usd Futures API
        /// </summary>
        public HitoBitSocketApiOptions UsdFuturesOptions { get; private set; } = new HitoBitSocketApiOptions();

        /// <summary>
        /// Options for the Coin Futures API
        /// </summary>
        public HitoBitSocketApiOptions CoinFuturesOptions { get; private set; } = new HitoBitSocketApiOptions();

        internal HitoBitSocketOptions Copy()
        {
            var options = Copy<HitoBitSocketOptions>();
            options.SpotOptions = SpotOptions.Copy();
            options.UsdFuturesOptions = UsdFuturesOptions.Copy();
            options.CoinFuturesOptions = CoinFuturesOptions.Copy();
            return options;
        }
    }
}