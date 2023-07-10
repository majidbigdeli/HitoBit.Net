using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.CommonClients;
using System;

namespace HitoBit.Net.Interfaces.Clients.UsdFuturesApi
{
    /// <summary>
    /// HitoBit USD futures API endpoints
    /// </summary>
    public interface IHitoBitRestClientUsdFuturesApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        public IHitoBitRestClientUsdFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market data
        /// </summary>
        public IHitoBitRestClientUsdFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        public IHitoBitRestClientUsdFuturesApiTrading Trading { get; }

        /// <summary>
        /// Get the IFuturesClient for this client. This is a common interface which allows for some basic operations without knowing any details of the exchange.
        /// </summary>
        /// <returns></returns>
        public IFuturesClient CommonFuturesClient { get; }
    }
}
