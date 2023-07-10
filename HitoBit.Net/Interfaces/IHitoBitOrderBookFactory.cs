using HitoBit.Net.Objects.Options;
using CryptoExchange.Net.Interfaces;
using System;

namespace HitoBit.Net.Interfaces
{
    /// <summary>
    /// HitoBit order book factory
    /// </summary>
    public interface IHitoBitOrderBookFactory
    {
        /// <summary>
        /// Create a Spot SymbolOrderBook
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook CreateSpot(string symbol, Action<HitoBitOrderBookOptions>? options = null);

        /// <summary>
        /// Create a Usdt Futures SymbolOrderBook
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook CreateUsdtFutures(string symbol, Action<HitoBitOrderBookOptions>? options = null);

        /// <summary>
        /// Create a Coin Futures SymbolOrderBook
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook CreateCoinFutures(string symbol, Action<HitoBitOrderBookOptions>? options = null);
    }
}