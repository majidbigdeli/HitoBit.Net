﻿using System;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Objects.Models.Spot;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Wrapper for kline information for a symbol
    /// </summary>
    public class HitoBitFuturesStreamCoinKlineData : HitoBitStreamEvent, IHitoBitStreamKlineData
    {
        /// <summary>
        /// The symbol the data is for
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The data
        /// </summary>
        [JsonProperty("k")]
        [JsonConverter(typeof(InterfaceConverter<HitoBitFuturesStreamCoinKline>))]
        public IHitoBitStreamKline Data { get; set; } = default!;
    }

    /// <summary>
    /// The kline data
    /// </summary>
    public class HitoBitFuturesStreamCoinKline : HitoBitKlineBase, IHitoBitStreamKline
    {
        /// <summary>
        /// The open time of this candlestick
        /// </summary>
        [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
        public new DateTime OpenTime { get; set; }

        /// <inheritdoc />
        [JsonProperty("q")]
        public override decimal Volume { get; set; }

        /// <summary>
        /// The close time of this candlestick
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public new DateTime CloseTime { get; set; }

        /// <inheritdoc />
        [JsonProperty("v")]
        public override decimal QuoteVolume { get; set; }

        /// <summary>
        /// The symbol this candlestick is for
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The interval of this candlestick
        /// </summary>
        [JsonProperty("i"), JsonConverter(typeof(KlineIntervalConverter))]
        public KlineInterval Interval { get; set; }
        /// <summary>
        /// The first trade id in this candlestick
        /// </summary>
        [JsonProperty("f")]
        public long FirstTrade { get; set; }
        /// <summary>
        /// The last trade id in this candlestick
        /// </summary>
        [JsonProperty("L")]
        public long LastTrade { get; set; }
        /// <summary>
        /// The open price of this candlestick
        /// </summary>
        [JsonProperty("o")]
        public new decimal OpenPrice { get; set; }
        /// <summary>
        /// The close price of this candlestick
        /// </summary>
        [JsonProperty("c")]
        public new decimal ClosePrice { get; set; }
        /// <summary>
        /// The highest price of this candlestick
        /// </summary>
        [JsonProperty("h")]
        public new decimal HighPrice { get; set; }
        /// <summary>
        /// The lowest price of this candlestick
        /// </summary>
        [JsonProperty("l")]
        public new decimal LowPrice { get; set; }
        /// <summary>
        /// The amount of trades in this candlestick
        /// </summary>
        [JsonProperty("n")]
        public new int TradeCount { get; set; }

        /// <inheritdoc />
        [JsonProperty("Q")]
        public override decimal TakerBuyBaseVolume { get; set; }
        /// <inheritdoc />
        [JsonProperty("V")]
        public override decimal TakerBuyQuoteVolume { get; set; }

        /// <summary>
        /// Boolean indicating whether this candlestick is closed
        /// </summary>
        [JsonProperty("x")]
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

    public class HitoBitFuturesStreamAssetIndexUpdate : HitoBitStreamEvent
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Index price
        /// </summary>
        [JsonProperty("i")]
        public decimal IndexPrice { get; set; }
        /// <summary>
        /// Bid buffer
        /// </summary>
        [JsonProperty("b")]
        public decimal BidBuffer { get; set; }
        /// <summary>
        /// Ask buffer
        /// </summary>
        [JsonProperty("a")]
        public decimal AskBuffer { get; set; }
        /// <summary>
        /// Bid rate
        /// </summary>
        [JsonProperty("B")]
        public decimal BidRate { get; set; }
        /// <summary>
        /// Ask rate
        /// </summary>
        [JsonProperty("A")]
        public decimal AskRate { get; set; }
        /// <summary>
        /// Auto exchange bid buffer
        /// </summary>
        [JsonProperty("q")]
        public decimal AutoExchangeBidBuffer { get; set; }
        /// <summary>
        /// Auto exchange ask buffer
        /// </summary>
        [JsonProperty("g")]
        public decimal AutoExchangeAskBuffer { get; set; }
        /// <summary>
        /// Auto exchange bid rate
        /// </summary>
        [JsonProperty("Q")]
        public decimal AutoExchangeBidRate { get; set; }
        /// <summary>
        /// Auto exchange ask rate
        /// </summary>
        [JsonProperty("G")]
        public decimal AutoExchangeAskRate { get; set; }
    }

}
