using CryptoExchange.Net.Objects.Options;

namespace HitoBit.Net.Objects.Options
{
    /// <summary>
    /// Options for the HitoBitRestClient
    /// </summary>
    public class HitoBitRestOptions : RestExchangeOptions<HitoBitEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static HitoBitRestOptions Default { get; set; } = new HitoBitRestOptions()
        {
            Environment = HitoBitEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// The default receive window for requests
        /// </summary>
        public TimeSpan ReceiveWindow { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Spot API options
        /// </summary>
        public HitoBitRestApiOptions SpotOptions { get; private set; } = new HitoBitRestApiOptions();

        /// <summary>
        /// Usd futures API options
        /// </summary>
        public HitoBitRestApiOptions UsdFuturesOptions { get; private set; } = new HitoBitRestApiOptions();

        /// <summary>
        /// Coin futures API options
        /// </summary>
        public HitoBitRestApiOptions CoinFuturesOptions { get; private set; } = new HitoBitRestApiOptions();

        internal HitoBitRestOptions Copy()
        {
            var options = Copy<HitoBitRestOptions>();
            options.ReceiveWindow = ReceiveWindow;
            options.SpotOptions = SpotOptions.Copy();
            options.UsdFuturesOptions = UsdFuturesOptions.Copy();
            options.CoinFuturesOptions = CoinFuturesOptions.Copy();
            return options;
        }
    }
}
