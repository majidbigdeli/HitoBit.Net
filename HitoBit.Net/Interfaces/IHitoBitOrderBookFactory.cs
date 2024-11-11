using HitoBit.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;

namespace HitoBit.Net.Interfaces
{
    /// <summary>
    /// HitoBit order book factory
    /// </summary>
    public interface IHitoBitOrderBookFactory
    {
        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        public IOrderBookFactory<HitoBitOrderBookOptions> Spot { get; }
        /// <summary>
        /// USD Futures order book factory methods
        /// </summary>
        public IOrderBookFactory<HitoBitOrderBookOptions> UsdFutures { get; }
        /// <summary>
        /// Coin Futures order book factory methods
        /// </summary>
        public IOrderBookFactory<HitoBitOrderBookOptions> CoinFutures { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<HitoBitOrderBookOptions>? options = null);

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