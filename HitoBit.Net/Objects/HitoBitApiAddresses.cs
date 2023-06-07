namespace HitoBit.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class HitoBitApiAddresses
    {
        /// <summary>
        /// The address used by the HitoBitClient for the Spot API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the HitoBitSocketClient for the Spot API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the HitoBitSocketClient for connecting to the BLVT streams
        /// </summary>
        public string? BlvtSocketClientAddress { get; set; }
        /// <summary>
        /// The address used by the HitoBitClient for the USD futures API
        /// </summary>
        public string? UsdFuturesRestClientAddress { get; set; }
        /// <summary>
        /// The address used by the HitoBitSocketClient for the USD futures API
        /// </summary>
        public string? UsdFuturesSocketClientAddress { get; set; }

        /// <summary>
        /// The address used by the HitoBitClient for the COIN futures API
        /// </summary>
        public string? CoinFuturesRestClientAddress { get; set; }
        /// <summary>
        /// The address used by the HitoBitSocketClient for the Coin futures API
        /// </summary>
        public string? CoinFuturesSocketClientAddress { get; set; }

        /// <summary>
        /// The default addresses to connect to the HitoBit.com API
        /// </summary>
        public static HitoBitApiAddresses Default = new HitoBitApiAddresses
        {
            RestClientAddress = "https://api.HitoBit.com",
            SocketClientAddress = "wss://stream.HitoBit.com:9443/",
            BlvtSocketClientAddress = "wss://nbstream.HitoBit.com/lvt-p",
            UsdFuturesRestClientAddress = "https://fapi.HitoBit.com",
            UsdFuturesSocketClientAddress = "wss://fstream.HitoBit.com/",
            CoinFuturesRestClientAddress = "https://dapi.HitoBit.com",
            CoinFuturesSocketClientAddress = "wss://dstream.HitoBit.com/",
        };

        /// <summary>
        /// The addresses to connect to the HitoBit testnet
        /// </summary>
        public static HitoBitApiAddresses TestNet = new HitoBitApiAddresses
        {
            RestClientAddress = "https://testnet.HitoBit.vision",
            SocketClientAddress = "wss://testnet.HitoBit.vision",
            BlvtSocketClientAddress = "wss://fstream.HitoBitfuture.com",
            UsdFuturesRestClientAddress = "https://testnet.HitoBitfuture.com",
            UsdFuturesSocketClientAddress = "wss://fstream.HitoBitfuture.com",
            CoinFuturesRestClientAddress = "https://testnet.HitoBitfuture.com",
            CoinFuturesSocketClientAddress = "wss://dstream.HitoBitfuture.com",
        };

        /// <summary>
        /// The addresses to connect to HitoBit.us. (HitoBit.us futures not are not available)
        /// </summary>
        public static HitoBitApiAddresses Us = new HitoBitApiAddresses
        {
            RestClientAddress = "https://api.HitoBit.us",
            SocketClientAddress = "wss://stream.HitoBit.us:9443",
        };
    }
}
