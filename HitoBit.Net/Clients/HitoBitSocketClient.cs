using HitoBit.Net.Clients.CoinFuturesApi;
using HitoBit.Net.Clients.SpotApi;
using HitoBit.Net.Clients.UsdFuturesApi;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Interfaces.Clients.CoinFuturesApi;
using HitoBit.Net.Interfaces.Clients.SpotApi;
using HitoBit.Net.Interfaces.Clients.UsdFuturesApi;
using HitoBit.Net.Objects;
using HitoBit.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using System;

namespace HitoBit.Net.Clients
{
    /// <inheritdoc cref="IHitoBitSocketClient" />
    public class HitoBitSocketClient : BaseSocketClient, IHitoBitSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IHitoBitSocketClientSpotApi SpotApi { get; set; }

        /// <inheritdoc />
        public IHitoBitSocketClientUsdFuturesApi UsdFuturesApi { get; set; }

        /// <inheritdoc />
        public IHitoBitSocketClientCoinFuturesApi CoinFuturesApi { get; set; }

        #endregion

        #region constructor/destructor
        /// <summary>
        /// Create a new instance of HitoBitSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        public HitoBitSocketClient(ILoggerFactory? loggerFactory = null) : this((x) => { }, loggerFactory)
        {
        }

        /// <summary>
        /// Create a new instance of HitoBitSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public HitoBitSocketClient(Action<HitoBitSocketOptions> optionsDelegate) : this(optionsDelegate, null)
        {
        }

        /// <summary>
        /// Create a new instance of HitoBitSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public HitoBitSocketClient(Action<HitoBitSocketOptions> optionsDelegate, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "HitoBit")
        {
            var options = HitoBitSocketOptions.Default.Copy();
            optionsDelegate(options);
            Initialize(options);

            SpotApi = AddApiClient(new HitoBitSocketClientSpotApi(_logger, options));
            UsdFuturesApi = AddApiClient(new HitoBitSocketClientUsdFuturesApi(_logger, options));
            CoinFuturesApi = AddApiClient(new HitoBitSocketClientCoinFuturesApi(_logger, options));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<HitoBitSocketOptions> optionsDelegate)
        {
            var options = HitoBitSocketOptions.Default.Copy();
            optionsDelegate(options);
            HitoBitSocketOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
            UsdFuturesApi.SetApiCredentials(credentials);
            CoinFuturesApi.SetApiCredentials(credentials);
        }
    }
}
