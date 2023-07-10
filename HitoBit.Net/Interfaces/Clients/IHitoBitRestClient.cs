using HitoBit.Net.Interfaces.Clients.CoinFuturesApi;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;

namespace HitoBit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the HitoBit Rest API. 
    /// </summary>
    public interface IHitoBitRestClient: IRestClient
    {
        /// <summary>
        /// General API endpoints
        /// </summary>
        IHitoBitRestClientGeneralApi GeneralApi { get; }
        /// <summary>
        /// Coin futures API endpoints
        /// </summary>
        IHitoBitRestClientCoinFuturesApi CoinFuturesApi { get; }
        /// <summary>
        /// Spot API endpoints
        /// </summary>
        IHitoBitRestClientSpotApi SpotApi { get; }
        /// <summary>
        /// Usd futures API endpoints
        /// </summary>
        IHitoBitRestClientUsdFuturesApi UsdFuturesApi { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(ApiCredentials credentials);
    }
}
