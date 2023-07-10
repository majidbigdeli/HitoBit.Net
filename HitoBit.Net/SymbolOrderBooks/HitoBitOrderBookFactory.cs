using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Objects.Options;
using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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
        }

        /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<HitoBitOrderBookOptions>? options = null)
            => new HitoBitSpotSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILogger<HitoBitSpotSymbolOrderBook>>(),
                                             _serviceProvider.GetRequiredService<IHitoBitRestClient>(),
                                             _serviceProvider.GetRequiredService<IHitoBitSocketClient>());

        
        /// <inheritdoc />
        public ISymbolOrderBook CreateUsdtFutures(string symbol, Action<HitoBitOrderBookOptions>? options = null)
            => new HitoBitFuturesUsdtSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILogger<HitoBitFuturesUsdtSymbolOrderBook>>(),
                                             _serviceProvider.GetRequiredService<IHitoBitRestClient>(),
                                             _serviceProvider.GetRequiredService<IHitoBitSocketClient>());

        
        /// <inheritdoc />
        public ISymbolOrderBook CreateCoinFutures(string symbol, Action<HitoBitOrderBookOptions>? options = null)
            => new HitoBitFuturesCoinSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILogger<HitoBitFuturesCoinSymbolOrderBook>>(),
                                             _serviceProvider.GetRequiredService<IHitoBitRestClient>(),
                                             _serviceProvider.GetRequiredService<IHitoBitSocketClient>());
    }
}
