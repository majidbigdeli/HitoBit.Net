using HitoBit.Net.Interfaces.Clients.CoinFuturesApi;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using CryptoExchange.Net.Interfaces;

namespace HitoBit.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the HitoBit websocket API
    /// </summary>
    public interface IHitoBitSocketClient: ISocketClient
    {
        /// <summary>
        /// Coin futures streams
        /// </summary>
        IHitoBitSocketClientCoinFuturesStreams CoinFuturesStreams { get; }
        /// <summary>
        /// Spot streams
        /// </summary>
        IHitoBitSocketClientSpotStreams SpotStreams { get; }
        /// <summary>
        /// Usd futures streams
        /// </summary>
        IHitoBitSocketClientUsdFuturesStreams UsdFuturesStreams { get; }

        /// <summary>
        /// Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.
        /// </summary>
        /// <param name="credentials">The credentials to set</param>
        void SetApiCredentials(HitoBitApiCredentials credentials);
    }
}
