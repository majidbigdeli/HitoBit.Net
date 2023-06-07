using HitoBit.Net.Clients.CoinFuturesApi;
using HitoBit.Net.Clients.SpotApi;
using HitoBit.Net.Clients.UsdFuturesApi;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Interfaces.Clients.CoinFuturesApi;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using CryptoExchange.Net;

namespace HitoBit.Net.Clients
{
    /// <inheritdoc cref="IHitoBitSocketClient" />
    public class HitoBitSocketClient : BaseSocketClient, IHitoBitSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IHitoBitSocketClientSpotStreams SpotStreams { get; set; }
        /// <inheritdoc />
        public IHitoBitSocketClientUsdFuturesStreams UsdFuturesStreams { get; set; }
        /// <inheritdoc />
        public IHitoBitSocketClientCoinFuturesStreams CoinFuturesStreams { get; set; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of HitoBitSocketClientSpot with default options
        /// </summary>
        public HitoBitSocketClient() : this(HitoBitSocketClientOptions.Default)
        {
        }

        /// <summary>
        /// Create a new instance of HitoBitSocketClientSpot using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public HitoBitSocketClient(HitoBitSocketClientOptions options) : base("HitoBit", options)
        {
            SpotStreams = AddApiClient(new HitoBitSocketClientSpotStreams(log, options));
            UsdFuturesStreams = AddApiClient(new HitoBitSocketClientUsdFuturesStreams(log, options));
            CoinFuturesStreams = AddApiClient(new HitoBitSocketClientCoinFuturesStreams(log, options));
        }
        #endregion 

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options">Options to use as default</param>
        public static void SetDefaultOptions(HitoBitSocketClientOptions options)
        {
            HitoBitSocketClientOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(HitoBitApiCredentials credentials)
        {
            SpotStreams.SetApiCredentials(credentials);
            UsdFuturesStreams.SetApiCredentials(credentials);
            CoinFuturesStreams.SetApiCredentials(credentials);
        }
    }
}
