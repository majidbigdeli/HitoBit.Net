using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;

namespace HitoBit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Spot API socket subscriptions and requests
    /// </summary>
    public interface IHitoBitSocketClientSpotApi
    {
        /// <summary>
        /// Factory for websockets
        /// </summary>
        IWebsocketFactory SocketFactory { get; set; }

        /// <summary>
        /// Account streams and queries
        /// </summary>
        IHitoBitSocketClientSpotApiAccount Account { get; }
        /// <summary>
        /// Exchange data streams and queries
        /// </summary>
        IHitoBitSocketClientSpotApiExchangeData ExchangeData { get; }
        /// <summary>
        /// Trading data and queries
        /// </summary>
        IHitoBitSocketClientSpotApiTrading Trading { get; }
        /// <summary>
        /// Set the API credentials for this API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="credentials"></param>
        void SetApiCredentials<T>(T credentials) where T : ApiCredentials;
    }
}