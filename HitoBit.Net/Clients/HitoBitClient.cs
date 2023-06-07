using HitoBit.Net.Objects;
using CryptoExchange.Net;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Interfaces.Clients.CoinFuturesApi;
using HitoBit.Net.Clients.GeneralApi;
using HitoBit.Net.Clients.SpotApi;
using HitoBit.Net.Clients.UsdFuturesApi;
using HitoBit.Net.Clients.CoinFuturesApi;

namespace HitoBit.Net.Clients
{
    /// <inheritdoc cref="IHitoBitClient" />
    public class HitoBitClient : BaseRestClient, IHitoBitClient
    {
        #region Api clients

        /// <inheritdoc />
        public IHitoBitClientGeneralApi GeneralApi { get; }
        /// <inheritdoc />
        public IHitoBitClientSpotApi SpotApi { get; }
        /// <inheritdoc />
        public IHitoBitClientUsdFuturesApi UsdFuturesApi { get; }
        /// <inheritdoc />
        public IHitoBitClientCoinFuturesApi CoinFuturesApi { get; }

        #endregion

        #region constructor/destructor
        /// <summary>
        /// Create a new instance of HitoBitClient using the default options
        /// </summary>
        public HitoBitClient() : this(HitoBitClientOptions.Default)
        {
        }

        /// <summary>
        /// Create a new instance of HitoBitClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public HitoBitClient(HitoBitClientOptions options) : base("HitoBit", options)
        {
            GeneralApi = AddApiClient(new HitoBitClientGeneralApi(log, this, options));
            SpotApi = AddApiClient(new HitoBitClientSpotApi(log, options));
            UsdFuturesApi = AddApiClient(new HitoBitClientUsdFuturesApi(log, options));
            CoinFuturesApi = AddApiClient(new HitoBitClientCoinFuturesApi(log, options));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options">Options to use as default</param>
        public static void SetDefaultOptions(HitoBitClientOptions options)
        {
            HitoBitClientOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(HitoBitApiCredentials credentials)
        {
            GeneralApi.SetApiCredentials(credentials);
            SpotApi.SetApiCredentials(credentials);
            UsdFuturesApi.SetApiCredentials(credentials);
            CoinFuturesApi.SetApiCredentials(credentials);
        }
    }
}
