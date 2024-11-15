﻿using HitoBit.Net.Objects;

namespace HitoBit.Net
{
    /// <summary>
    /// HitoBit environments
    /// </summary>
    public class HitoBitEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Spot Rest API address
        /// </summary>
        public string SpotRestAddress { get; }

        /// <summary>
        /// Spot Socket Streams address
        /// </summary>
        public string SpotSocketStreamAddress { get; }

        /// <summary>
        /// Spot Socket API address
        /// </summary>
        public string SpotSocketApiAddress { get; }

        /// <summary>
        /// Blvt Socket API address
        /// </summary>
        public string? BlvtSocketAddress { get; }

        /// <summary>
        /// Usd futures Rest address
        /// </summary>
        public string? UsdFuturesRestAddress { get; }

        /// <summary>
        /// Usd futures Socket address
        /// </summary>
        public string? UsdFuturesSocketAddress { get; }

        /// <summary>
        /// Usd futures Socket address for the request API
        /// </summary>
        public string? UsdFuturesSocketApiAddress { get; }

        /// <summary>
        /// Coin futures Rest address
        /// </summary>
        public string? CoinFuturesRestAddress { get; }

        /// <summary>
        /// Coin futures Socket address
        /// </summary>
        public string? CoinFuturesSocketAddress { get; }

        internal HitoBitEnvironment(
            string name, 
            string spotRestAddress, 
            string spotSocketStreamAddress, 
            string spotSocketApiAddress,
            string? blvtSocketAddress, 
            string? usdFuturesRestAddress, 
            string? usdFuturesSocketAddress,
            string? usdFuturesSocketApiAddress,
            string? coinFuturesRestAddress,
            string? coinFuturesSocketAddress) :
            base(name)
        {
            SpotRestAddress = spotRestAddress;
            SpotSocketStreamAddress = spotSocketStreamAddress;
            SpotSocketApiAddress = spotSocketApiAddress;
            BlvtSocketAddress = blvtSocketAddress;
            UsdFuturesRestAddress = usdFuturesRestAddress;
            UsdFuturesSocketAddress = usdFuturesSocketAddress;
            UsdFuturesSocketApiAddress = usdFuturesSocketApiAddress;
            CoinFuturesRestAddress = coinFuturesRestAddress;
            CoinFuturesSocketAddress = coinFuturesSocketAddress;
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static HitoBitEnvironment Live { get; } 
            = new HitoBitEnvironment(TradeEnvironmentNames.Live, 
                                     HitoBitApiAddresses.Default.RestClientAddress,
                                     HitoBitApiAddresses.Default.SocketClientStreamAddress,
                                     HitoBitApiAddresses.Default.SocketClientApiAddress,
                                     HitoBitApiAddresses.Default.BlvtSocketClientAddress,
                                     HitoBitApiAddresses.Default.UsdFuturesRestClientAddress,
                                     HitoBitApiAddresses.Default.UsdFuturesSocketClientAddress,
                                     HitoBitApiAddresses.Default.UsdFuturesSocketApiClientAddress,
                                     HitoBitApiAddresses.Default.CoinFuturesRestClientAddress,
                                     HitoBitApiAddresses.Default.CoinFuturesSocketClientAddress);

        /// <summary>
        /// Testnet environment
        /// </summary>
        public static HitoBitEnvironment Testnet { get; }
            = new HitoBitEnvironment(TradeEnvironmentNames.Testnet,
                                     HitoBitApiAddresses.TestNet.RestClientAddress,
                                     HitoBitApiAddresses.TestNet.SocketClientStreamAddress,
                                     HitoBitApiAddresses.TestNet.SocketClientApiAddress,
                                     HitoBitApiAddresses.TestNet.BlvtSocketClientAddress,
                                     HitoBitApiAddresses.TestNet.UsdFuturesRestClientAddress,
                                     HitoBitApiAddresses.TestNet.UsdFuturesSocketClientAddress,
                                     HitoBitApiAddresses.TestNet.UsdFuturesSocketApiClientAddress,
                                     HitoBitApiAddresses.TestNet.CoinFuturesRestClientAddress,
                                     HitoBitApiAddresses.TestNet.CoinFuturesSocketClientAddress);

        /// <summary>
        /// HitoBit.us environment
        /// </summary>
        public static HitoBitEnvironment Us { get; }
            = new HitoBitEnvironment("Us",
                                     HitoBitApiAddresses.Us.RestClientAddress,
                                     HitoBitApiAddresses.Us.SocketClientStreamAddress,
                                     HitoBitApiAddresses.Us.SocketClientApiAddress,
                                     null,
                                     null,
                                     null,
                                     null,
                                     null,
                                     null);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        public static HitoBitEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketStreamsAddress,
                        string spotSocketApiAddress,
                        string? blvtSocketAddress,
                        string? usdFuturesRestAddress,
                        string? usdFuturesSocketAddress,
                        string? usdFuturesSocketApiAddress,
                        string? coinFuturesRestAddress,
                        string? coinFuturesSocketAddress)
            => new HitoBitEnvironment(name, spotRestAddress, spotSocketStreamsAddress, spotSocketApiAddress, blvtSocketAddress, usdFuturesRestAddress, usdFuturesSocketAddress, usdFuturesSocketApiAddress, coinFuturesRestAddress, coinFuturesSocketAddress);
    }
}
