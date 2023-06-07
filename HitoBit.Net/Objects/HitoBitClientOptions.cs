using System;
using System.Collections.Generic;
using System.Net.Http;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace HitoBit.Net.Objects
{
    /// <summary>
    /// Options for the HitoBit client
    /// </summary>
    public class HitoBitClientOptions : ClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static HitoBitClientOptions Default { get; set; } = new HitoBitClientOptions();

        /// <summary>
        /// The default receive window for requests
        /// </summary>
        public TimeSpan ReceiveWindow { get; set; } = TimeSpan.FromSeconds(5);

        private HitoBitApiClientOptions _spotApiOptions = new HitoBitApiClientOptions(HitoBitApiAddresses.Default.RestClientAddress)
        {
            AutoTimestamp = true,
            RateLimiters = new List<IRateLimiter>
                {
                    new RateLimiter()
                        .AddPartialEndpointLimit("/api/", 1200, TimeSpan.FromMinutes(1))
                        .AddPartialEndpointLimit("/sapi/", 180000, TimeSpan.FromMinutes(1))
                        .AddEndpointLimit("/api/v3/order", 50, TimeSpan.FromSeconds(10), HttpMethod.Post, true)
                }
        };
        /// <summary>
        /// Spot API options
        /// </summary>
        public HitoBitApiClientOptions SpotApiOptions
        {
            get => _spotApiOptions;
            set => _spotApiOptions = new HitoBitApiClientOptions(_spotApiOptions, value);
        }

        /// <inheritdoc />
        public new HitoBitApiCredentials? ApiCredentials
        {
            get => (HitoBitApiCredentials?)base.ApiCredentials;
            set => base.ApiCredentials = value;
        }

        private HitoBitApiClientOptions _usdFuturesApiOptions = new HitoBitApiClientOptions(HitoBitApiAddresses.Default.UsdFuturesRestClientAddress!)
        {
            AutoTimestamp = true
        };
        /// <summary>
        /// Usd futures API options
        /// </summary>
        public HitoBitApiClientOptions UsdFuturesApiOptions
        {
            get => _usdFuturesApiOptions;
            set => _usdFuturesApiOptions = new HitoBitApiClientOptions(_usdFuturesApiOptions, value);
        }

        private HitoBitApiClientOptions _coinFuturesApiOptions = new HitoBitApiClientOptions(HitoBitApiAddresses.Default.CoinFuturesRestClientAddress!)
        {
            AutoTimestamp = true
        };
        /// <summary>
        /// Coin futures API options
        /// </summary>
        public HitoBitApiClientOptions CoinFuturesApiOptions
        {
            get => _coinFuturesApiOptions;
            set => _coinFuturesApiOptions = new HitoBitApiClientOptions(_coinFuturesApiOptions, value);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public HitoBitClientOptions() : this(Default)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOn">Base the new options on other options</param>
        internal HitoBitClientOptions(HitoBitClientOptions baseOn) : base(baseOn)
        {
            if (baseOn == null)
                return;

            ReceiveWindow = baseOn.ReceiveWindow;

            ApiCredentials = (HitoBitApiCredentials?)baseOn.ApiCredentials?.Copy();
            _spotApiOptions = new HitoBitApiClientOptions(baseOn.SpotApiOptions, null);
            _usdFuturesApiOptions = new HitoBitApiClientOptions(baseOn.UsdFuturesApiOptions, null);
            _coinFuturesApiOptions = new HitoBitApiClientOptions(baseOn.CoinFuturesApiOptions, null);
        }
    }

    /// <summary>
    /// HitoBit socket client options
    /// </summary>
    public class HitoBitSocketClientOptions : ClientOptions
    {
        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static HitoBitSocketClientOptions Default { get; set; } = new HitoBitSocketClientOptions();

        private HitoBitSocketApiClientOptions _spotStreamsOptions = new HitoBitSocketApiClientOptions(HitoBitApiAddresses.Default.SocketClientAddress)
        {
            SocketSubscriptionsCombineTarget = 10
        };


        /// <inheritdoc />
        public new HitoBitApiCredentials? ApiCredentials
        {
            get => (HitoBitApiCredentials?)base.ApiCredentials;
            set => base.ApiCredentials = value;
        }

        /// <summary>
        /// Spot streams options
        /// </summary>
        public HitoBitSocketApiClientOptions SpotStreamsOptions
        {
            get => _spotStreamsOptions;
            set => _spotStreamsOptions = new HitoBitSocketApiClientOptions(_spotStreamsOptions, value);
        }

        private HitoBitSocketApiClientOptions _usdFuturesStreamsOptions = new HitoBitSocketApiClientOptions(HitoBitApiAddresses.Default.UsdFuturesSocketClientAddress!)
        {
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Usd futures streams options
        /// </summary>
        public HitoBitSocketApiClientOptions UsdFuturesStreamsOptions
        {
            get => _usdFuturesStreamsOptions;
            set => _usdFuturesStreamsOptions = new HitoBitSocketApiClientOptions(_usdFuturesStreamsOptions, value);
        }

        private HitoBitSocketApiClientOptions _coinFuturesStreamsOptions = new HitoBitSocketApiClientOptions(HitoBitApiAddresses.Default.CoinFuturesSocketClientAddress!)
        {
            SocketSubscriptionsCombineTarget = 10
        };

        /// <summary>
        /// Coin futures streams options
        /// </summary>
        public HitoBitSocketApiClientOptions CoinFuturesStreamsOptions
        {
            get => _coinFuturesStreamsOptions;
            set => _coinFuturesStreamsOptions = new HitoBitSocketApiClientOptions(_coinFuturesStreamsOptions, value);
        }

        /// <summary>
        /// Address for conencting the BLVT streams
        /// </summary>
        public string? BlvtStreamAddress { get; set; } = HitoBitApiAddresses.Default.BlvtSocketClientAddress;

        /// <summary>
        /// ctor
        /// </summary>
        public HitoBitSocketClientOptions() : this(Default)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOn">Base the new options on other options</param>
        internal HitoBitSocketClientOptions(HitoBitSocketClientOptions baseOn) : base(baseOn)
        {
            if (baseOn == null)
                return;

            BlvtStreamAddress = baseOn.BlvtStreamAddress;

            ApiCredentials = (HitoBitApiCredentials?)baseOn.ApiCredentials?.Copy();
            _spotStreamsOptions = new HitoBitSocketApiClientOptions(baseOn.SpotStreamsOptions, null);
            _usdFuturesStreamsOptions = new HitoBitSocketApiClientOptions(baseOn.UsdFuturesStreamsOptions, null);
            _coinFuturesStreamsOptions = new HitoBitSocketApiClientOptions(baseOn.CoinFuturesStreamsOptions, null);
        }
    }

    /// <summary>
    /// HitoBit API client options
    /// </summary>
    public class HitoBitApiClientOptions : RestApiClientOptions
    {
        /// <inheritdoc />
        public new HitoBitApiCredentials? ApiCredentials
        {
            get => (HitoBitApiCredentials?)base.ApiCredentials;
            set => base.ApiCredentials = value;
        }

        /// <summary>
        /// A manual offset for the timestamp. Should only be used if AutoTimestamp and regular time synchronization on the OS is not reliable enough
        /// </summary>
        public TimeSpan TimestampOffset { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Whether to check the trade rules when placing new orders and what to do if the trade isn't valid
        /// </summary>
        public TradeRulesBehaviour TradeRulesBehaviour { get; set; } = TradeRulesBehaviour.None;
        /// <summary>
        /// How often the trade rules should be updated. Only used when TradeRulesBehaviour is not None
        /// </summary>
        public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);

        /// <summary>
        /// ctor
        /// </summary>
        public HitoBitApiClientOptions()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseAddress"></param>
        internal HitoBitApiClientOptions(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOn"></param>
        /// <param name="newValues"></param>
        internal HitoBitApiClientOptions(HitoBitApiClientOptions baseOn, HitoBitApiClientOptions? newValues) : base(baseOn, newValues)
        {
            ApiCredentials = (HitoBitApiCredentials?)newValues?.ApiCredentials?.Copy() ?? (HitoBitApiCredentials?)baseOn.ApiCredentials?.Copy();
            TimestampOffset = newValues?.TimestampOffset ?? baseOn.TimestampOffset;
            TradeRulesBehaviour = newValues?.TradeRulesBehaviour ?? baseOn.TradeRulesBehaviour;
            TradeRulesUpdateInterval = newValues?.TradeRulesUpdateInterval ?? baseOn.TradeRulesUpdateInterval;
        }
    }

    /// <inheritdoc />
    public class HitoBitSocketApiClientOptions : SocketApiClientOptions
    {
        /// <inheritdoc />
        public new HitoBitApiCredentials? ApiCredentials
        {
            get => (HitoBitApiCredentials?)base.ApiCredentials;
            set => base.ApiCredentials = value;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public HitoBitSocketApiClientOptions()
        {
        }
        
        /// <summary>
        /// ctor
        /// </summary>
        public HitoBitSocketApiClientOptions(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="baseOn"></param>
        /// <param name="newValues"></param>
        internal HitoBitSocketApiClientOptions(HitoBitSocketApiClientOptions baseOn, HitoBitSocketApiClientOptions? newValues) : base(baseOn, newValues)
        {
            ApiCredentials = (HitoBitApiCredentials?)newValues?.ApiCredentials?.Copy() ?? (HitoBitApiCredentials?)baseOn.ApiCredentials?.Copy();
        }
    }

    /// <summary>
    /// HitoBit symbol order book options
    /// </summary>
    public class HitoBitOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks. Leaving this null will sync the full order book
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Update interval in milliseconds, either 100 or 1000. Defaults to 1000
        /// </summary>
        public int? UpdateInterval { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        /// <summary>
        /// The rest client to use for requesting the initial order book
        /// </summary>
        public IHitoBitClient? RestClient { get; set; }

        /// <summary>
        /// The client to use for the socket connection. When using the same client for multiple order books the connection can be shared.
        /// </summary>
        public IHitoBitSocketClient? SocketClient { get; set; }
    }
}
