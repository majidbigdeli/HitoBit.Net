using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Objects.Options;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;

namespace HitoBit.Net.SymbolOrderBooks
{
    /// <summary>
    /// HitoBit order book factory
    /// </summary>
    public class HitoBitOrderBookFactory : IHitoBitOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public HitoBitOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Spot = new OrderBookFactory<HitoBitOrderBookOptions>(
                CreateSpot,
                (sharedSymbol, options) => CreateSpot(HitoBitExchange.FormatSymbol(sharedSymbol.BaseAsset, sharedSymbol.QuoteAsset, sharedSymbol.TradingMode, sharedSymbol.DeliverTime), options));
            UsdFutures = new OrderBookFactory<HitoBitOrderBookOptions>(
                CreateUsdtFutures,
                (sharedSymbol, options) => CreateUsdtFutures(HitoBitExchange.FormatSymbol(sharedSymbol.BaseAsset, sharedSymbol.QuoteAsset, sharedSymbol.TradingMode, sharedSymbol.DeliverTime), options));
            CoinFutures = new OrderBookFactory<HitoBitOrderBookOptions>(
                CreateCoinFutures,
                (sharedSymbol, options) => CreateCoinFutures(HitoBitExchange.FormatSymbol(sharedSymbol.BaseAsset, sharedSymbol.QuoteAsset, sharedSymbol.TradingMode, sharedSymbol.DeliverTime), options));
        }

        /// <inheritdoc />
        public IOrderBookFactory<HitoBitOrderBookOptions> Spot { get; }
        /// <inheritdoc />
        public IOrderBookFactory<HitoBitOrderBookOptions> UsdFutures { get; }
        /// <inheritdoc />
        public IOrderBookFactory<HitoBitOrderBookOptions> CoinFutures { get; }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<HitoBitOrderBookOptions>? options = null)
        {
            var symbolName = HitoBitExchange.FormatSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.TradingMode, symbol.DeliverTime);
            if (symbol.TradingMode == TradingMode.Spot)
                return CreateSpot(symbolName, options);
            if (symbol.TradingMode.IsLinear())
                return CreateUsdtFutures(symbolName, options);
            
            return CreateCoinFutures(symbolName, options);
        }

        /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<HitoBitOrderBookOptions>? options = null)
            => new HitoBitSpotSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IHitoBitRestClient>(),
                                             _serviceProvider.GetRequiredService<IHitoBitSocketClient>());

        
        /// <inheritdoc />
        public ISymbolOrderBook CreateUsdtFutures(string symbol, Action<HitoBitOrderBookOptions>? options = null)
            => new HitoBitFuturesUsdtSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IHitoBitRestClient>(),
                                             _serviceProvider.GetRequiredService<IHitoBitSocketClient>());

        
        /// <inheritdoc />
        public ISymbolOrderBook CreateCoinFutures(string symbol, Action<HitoBitOrderBookOptions>? options = null)
            => new HitoBitFuturesCoinSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IHitoBitRestClient>(),
                                             _serviceProvider.GetRequiredService<IHitoBitSocketClient>());
    }
}
