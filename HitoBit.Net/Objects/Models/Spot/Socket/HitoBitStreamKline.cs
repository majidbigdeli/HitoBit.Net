using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;

namespace HitoBit.Net.Objects.Models.Spot.Socket
{
    /// <summary>
    /// Wrapper for kline information for a symbol
    /// </summary>
    public record HitoBitStreamKlineData: HitoBitStreamEvent, IHitoBitStreamKlineData
    {
        /// <summary>
        /// The symbol the data is for
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The data
        /// </summary>
        [JsonPropertyName("k")]
        [JsonConverter(typeof(InterfaceConverter<HitoBitStreamKline, IHitoBitStreamKline>))]
        public IHitoBitStreamKline Data { get; set; } = default!;
    }

    /// <summary>
    /// The kline data
    /// </summary>
    public record HitoBitStreamKline: IHitoBitStreamKline
    {
        /// <summary>
        /// The open time of this candlestick
        /// </summary>
        [JsonPropertyName("t"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime OpenTime { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }

        /// <summary>
        /// The close time of this candlestick
        /// </summary>
        [JsonPropertyName("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime CloseTime { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("q")]
        public decimal QuoteVolume { get; set; }

        /// <summary>
        /// The symbol this candlestick is for
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The interval of this candlestick
        /// </summary>
        [JsonPropertyName("i"), JsonConverter(typeof(EnumConverter))]
        public KlineInterval Interval { get; set; }
        /// <summary>
        /// The first trade id in this candlestick
        /// </summary>
        [JsonPropertyName("f")]
        public long FirstTrade { get; set; }
        /// <summary>
        /// The last trade id in this candlestick
        /// </summary>
        [JsonPropertyName("L")]
        public long LastTrade { get; set; }
        /// <summary>
        /// The open price of this candlestick
        /// </summary>
        [JsonPropertyName("o")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// The close price of this candlestick
        /// </summary>
        [JsonPropertyName("c")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// The highest price of this candlestick
        /// </summary>
        [JsonPropertyName("h")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// The lowest price of this candlestick
        /// </summary>
        [JsonPropertyName("l")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// The amount of trades in this candlestick
        /// </summary>
        [JsonPropertyName("n")]
        public int TradeCount { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("V")]
        public decimal TakerBuyBaseVolume { get; set; }
        /// <inheritdoc />
        [JsonPropertyName("Q")]
        public decimal TakerBuyQuoteVolume { get; set; }

        /// <summary>
        /// Boolean indicating whether this candlestick is closed
        /// </summary>
        [JsonPropertyName("x")]
        public bool Final { get; set; }

        /// <summary>
        /// Casts this object to a <see cref="HitoBitSpotKline"/> object
        /// </summary>
        /// <returns></returns>
        public HitoBitSpotKline ToKline()
        {
            return new HitoBitSpotKline
            {
                OpenPrice = OpenPrice,
                ClosePrice = ClosePrice,
                Volume = Volume,
                CloseTime = CloseTime,
                HighPrice = HighPrice,
                LowPrice = LowPrice,
                OpenTime = OpenTime,
                QuoteVolume = QuoteVolume,
                TakerBuyBaseVolume = TakerBuyBaseVolume,
                TakerBuyQuoteVolume = TakerBuyQuoteVolume,
                TradeCount = TradeCount
            };
        }
    }
}
