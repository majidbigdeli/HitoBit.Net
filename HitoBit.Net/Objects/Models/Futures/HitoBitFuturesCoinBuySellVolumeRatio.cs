﻿using System;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Buy/sell volume ratio
    /// </summary>
    public class HitoBitFuturesCoinBuySellVolumeRatio
    {
        /// <summary>
        /// The pair
        /// </summary>
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonConverter(typeof(ContractTypeConverter))]
        public ContractType ContractType { get; set; }
        /// <summary>
        /// The taker buy volume
        /// </summary>
        [JsonProperty("takerBuyVol")]
        public decimal TakerBuyVolume { get; set; }
        /// <summary>
        /// The taker sell volume
        /// </summary>
        [JsonProperty("takerSellVol")]
        public decimal TakerSellVolume { get; set; }
        /// <summary>
        /// The taker buy value
        /// </summary>
        [JsonProperty("takerBuyVolValue")]
        public decimal TakerBuyVolumeValue { get; set; }
        /// <summary>
        /// The taker sell value
        /// </summary>
        [JsonProperty("takerSellVolValue")]
        public decimal TakerSellVolumeValue { get; set; }
        /// <summary>
        /// Data timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}



