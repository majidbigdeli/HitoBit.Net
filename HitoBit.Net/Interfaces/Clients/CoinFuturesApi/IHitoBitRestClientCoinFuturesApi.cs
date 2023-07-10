using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.CommonClients;
using System;

namespace HitoBit.Net.Interfaces.Clients.CoinFuturesApi
{
    /// <summary>
    /// HitoBit Coin futures API endpoints
    /// </summary>
    public interface IHitoBitRestClientCoinFuturesApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        public IHitoBitRestClientCoinFuturesApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market data
        /// </summary>
        public IHitoBitRestClientCoinFuturesApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        public IHitoBitRestClientCoinFuturesApiTrading Trading { get; }

        /// <summary>
        /// Get the IFuturesClient for this client. This is a common interface which allows for some basic operations without knowing any details of the exchange.
        /// </summary>
        /// <returns></returns>
        public IFuturesClient CommonFuturesClient { get; }
    }
}
