using CryptoExchange.Net.Objects.Options;
using System;

namespace HitoBit.Net.Objects.Options
{
    /// <summary>
    /// Options for the HitoBit SymbolOrderBook
    /// </summary>
    public class HitoBitOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static HitoBitOrderBookOptions Default { get; set; } = new HitoBitOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Update interval in milliseconds, either 100 or 1000. Defaults to 1000
        /// </summary>
        public int? UpdateInterval { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal HitoBitOrderBookOptions Copy()
        {
            var result = Copy<HitoBitOrderBookOptions>();
            result.Limit = Limit;
            result.UpdateInterval = UpdateInterval;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
