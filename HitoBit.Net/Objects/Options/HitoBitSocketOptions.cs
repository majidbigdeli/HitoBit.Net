using CryptoExchange.Net.Objects.Options;

namespace HitoBit.Net.Objects.Options
{
    /// <summary>
    /// Options for the HitoBitSocketClient
    /// </summary>
    public class HitoBitSocketOptions : SocketExchangeOptions<HitoBitEnvironment>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static HitoBitSocketOptions Default { get; set; } = new HitoBitSocketOptions()
        {
            Environment = HitoBitEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Options for the Spot API
        /// </summary>
        public HitoBitSocketApiOptions SpotOptions { get; private set; } = new HitoBitSocketApiOptions();

        /// <summary>
        /// Options for the Usd Futures API
        /// </summary>
        public HitoBitSocketApiOptions UsdFuturesOptions { get; private set; } = new HitoBitSocketApiOptions();

        /// <summary>
        /// Options for the Coin Futures API
        /// </summary>
        public HitoBitSocketApiOptions CoinFuturesOptions { get; private set; } = new HitoBitSocketApiOptions(); 

        internal HitoBitSocketOptions Copy()
        {
            var options = Copy<HitoBitSocketOptions>();
            options.SpotOptions = SpotOptions.Copy();
            options.UsdFuturesOptions = UsdFuturesOptions.Copy();
            options.CoinFuturesOptions = CoinFuturesOptions.Copy();
            return options;
        }
    }
}
