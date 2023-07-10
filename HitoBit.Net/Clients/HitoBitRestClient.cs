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
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using HitoBit.Net.Objects.Options;
using CryptoExchange.Net.Authentication;

namespace HitoBit.Net.Clients
{
    /// <inheritdoc cref="IHitoBitRestClient" />
    public class HitoBitRestClient : BaseRestClient, IHitoBitRestClient
    {
        #region Api clients

        /// <inheritdoc />
        public IHitoBitRestClientGeneralApi GeneralApi { get; }
        /// <inheritdoc />
        public IHitoBitRestClientSpotApi SpotApi { get; }
        /// <inheritdoc />
        public IHitoBitRestClientUsdFuturesApi UsdFuturesApi { get; }
        /// <inheritdoc />
        public IHitoBitRestClientCoinFuturesApi CoinFuturesApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the HitoBitRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public HitoBitRestClient(Action<HitoBitRestOptions> optionsDelegate) : this(null, null, optionsDelegate)
        {
        }

        /// <summary>
        /// Create a new instance of the HitoBitRestClient using provided options
        /// </summary>
        public HitoBitRestClient(ILoggerFactory? loggerFactory = null, HttpClient? httpClient = null) : this(httpClient, loggerFactory, null)
        {
        }

        /// <summary>
        /// Create a new instance of the HitoBitRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public HitoBitRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, Action<HitoBitRestOptions>? optionsDelegate = null) : base(loggerFactory, "HitoBit")
        {
            var options = HitoBitRestOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            GeneralApi = AddApiClient(new HitoBitRestClientGeneralApi(_logger, httpClient, this, options));
            SpotApi = AddApiClient(new HitoBitRestClientSpotApi(_logger, httpClient, options));
            UsdFuturesApi = AddApiClient(new HitoBitRestClientUsdFuturesApi(_logger, httpClient, options));
            CoinFuturesApi = AddApiClient(new HitoBitRestClientCoinFuturesApi(_logger, httpClient, options));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<HitoBitRestOptions> optionsDelegate)
        {
            var options = HitoBitRestOptions.Default.Copy();
            optionsDelegate(options);
            HitoBitRestOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            GeneralApi.SetApiCredentials(credentials);
            SpotApi.SetApiCredentials(credentials);
            UsdFuturesApi.SetApiCredentials(credentials);
            CoinFuturesApi.SetApiCredentials(credentials);
        }
    }
}
