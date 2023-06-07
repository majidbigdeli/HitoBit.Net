using System;
using System.Collections.Generic;
using HitoBit.Net.Objects.Models;

namespace HitoBit.Net.Interfaces
{
    /// <summary>
    /// The order book for a asset
    /// </summary>
    public interface IHitoBitOrderBook
    {
        /// <summary>
        /// The symbol of the order book (only filled from stream updates)
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// The ID of the last update
        /// </summary>
        long LastUpdateId { get; set; }

        /// <summary>
        /// The list of bids
        /// </summary>
        IEnumerable<HitoBitOrderBookEntry> Bids { get; set; }

        /// <summary>
        /// The list of asks
        /// </summary>
        IEnumerable<HitoBitOrderBookEntry> Asks { get; set; }
    }

    /// <summary>
    /// Order book update event
    /// </summary>
    public interface IHitoBitEventOrderBook : IHitoBitOrderBook
    {
        /// <summary>
        /// The ID of the first update
        /// </summary>
        long? FirstUpdateId { get; set; }
        /// <summary>
        /// Timestamp of the event
        /// </summary>
        DateTime EventTime { get; set; }
    }

    /// <summary>
    /// Futures order book update event
    /// </summary>
    public interface IHitoBitFuturesEventOrderBook : IHitoBitEventOrderBook
    {
        /// <summary>
        /// Transaction time
        /// </summary>
        DateTime TransactionTime { get; set; }
        /// <summary>
        /// Last update id of the previous update
        /// </summary>
        public long LastUpdateIdStream { get; set; }
    }
}
