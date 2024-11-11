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
        public string RestClientAddress { get; set; } = string.Empty;
        /// <summary>
        /// The address used by the HitoBitSocketClient for the Spot streams
        /// </summary>
        public string SocketClientStreamAddress { get; set; } = string.Empty;
        /// <summary>
        /// The address used by the HitoBitSocketClient for the Spot API
        /// </summary>
        public string SocketClientApiAddress { get; set; } = string.Empty;
        /// <summary>
        /// The address used by the HitoBitSocketClient for connecting to the BLVT streams
        /// </summary>
        public string? BlvtSocketClientAddress { get; set; }
        /// <summary>
        /// The address used by the HitoBitClient for the USD futures API
        /// </summary>
        public string? UsdFuturesRestClientAddress { get; set; }
        /// <summary>
        /// The address used by the HitoBitSocketClient for the USD futures streams
        /// </summary>
        public string? UsdFuturesSocketClientAddress { get; set; }
        /// <summary>
        /// The address used by the HitoBitSocketClient for the USD futures API
        /// </summary>
        public string? UsdFuturesSocketApiClientAddress { get; set; }

        /// <summary>
        /// The address used by the HitoBitClient for the COIN futures API
        /// </summary>
        public string? CoinFuturesRestClientAddress { get; set; }
        /// <summary>
        /// The address used by the HitoBitSocketClient for the Coin futures API
        /// </summary>
        public string? CoinFuturesSocketClientAddress { get; set; }

        /// <summary>
        /// The default addresses to connect to the hitobit.com API
        /// </summary>
        public static HitoBitApiAddresses Default = new HitoBitApiAddresses
        {
            RestClientAddress = "https://api.hitobit.com",
            SocketClientStreamAddress = "wss://stream.hitobit.com",
            SocketClientApiAddress = "wss://stream.hitobit.com",
            BlvtSocketClientAddress = "wss://nbstream.hitobit.com/",
            UsdFuturesRestClientAddress = "https://fapi.hitobit.com",
            UsdFuturesSocketClientAddress = "wss://fstream.hitobit.com/",
            UsdFuturesSocketApiClientAddress = "wss://ws-fapi.hitobit.com/",
            CoinFuturesRestClientAddress = "https://dapi.hitobit.com",
            CoinFuturesSocketClientAddress = "wss://dstream.hitobit.com/",
        };

        /// <summary>
        /// The addresses to connect to the hitobit testnet
        /// </summary>
        public static HitoBitApiAddresses TestNet = new HitoBitApiAddresses
        {
            RestClientAddress = "https://testnet.hitobit.vision",
            SocketClientStreamAddress = "wss://testnet.hitobit.vision",
            SocketClientApiAddress = "wss://testnet.hitobit.vision",
            BlvtSocketClientAddress = "wss://fstream.hitobitfuture.com",
            UsdFuturesRestClientAddress = "https://testnet.hitobitfuture.com",
            UsdFuturesSocketClientAddress = "wss://fstream.hitobitfuture.com",
            UsdFuturesSocketApiClientAddress = "wss://testnet.hitobitfuture.com",
            CoinFuturesRestClientAddress = "https://testnet.hitobitfuture.com",
            CoinFuturesSocketClientAddress = "wss://dstream.hitobitfuture.com",
        };

        /// <summary>
        /// The addresses to connect to hitobit.us. (hitobit.us futures not are not available)
        /// </summary>
        public static HitoBitApiAddresses Us = new HitoBitApiAddresses
        {
            RestClientAddress = "https://api.hitobit.us",
            SocketClientApiAddress = "wss://ws-api.hitobit.us:443",
            SocketClientStreamAddress = "wss://stream.hitobit.us:9443",
        };
    }
}
