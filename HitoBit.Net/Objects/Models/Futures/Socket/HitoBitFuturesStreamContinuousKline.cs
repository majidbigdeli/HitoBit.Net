using CryptoExchange.Net.Converters;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Objects.Models.Spot.Socket;
using Newtonsoft.Json;
using System;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Wrapper for continuous kline information for a symbol
    /// </summary>
    public class HitoBitStreamContinuousKlineData: HitoBitStreamEvent, IHitoBitStreamKlineData
    {
        /// <summary>
        /// The symbol the data is for
        /// </summary>
        [JsonProperty("ps")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The contract type
        /// </summary>
        [JsonProperty("ct")]
        public ContractType ContractType { get; set; } = ContractType.Unknown;

        /// <summary>
        /// The data
        /// </summary>
        [JsonProperty("k")]
        [JsonConverter(typeof(InterfaceConverter<HitoBitStreamKline>))]
        public IHitoBitStreamKline Data { get; set; } = default!;
    }

    /// <summary>
    /// 
    /// </summary>
    public class HitoBitConditionOrderTriggerRejectUpdate : HitoBitStreamEvent
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("T")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Reject info
        /// </summary>
        [JsonProperty("or")]
        public HitoBitConditionOrderTriggerReject RejectInfo { get; set; } = null!;
    }

    /// <summary>
    /// Reject info
    /// </summary>
    public class HitoBitConditionOrderTriggerReject
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("i")]
        public long OrderId { get; set; }
        /// <summary>
        /// Reject reason
        /// </summary>
        [JsonProperty("r")]
        public string Reason { get; set; } = string.Empty;
    }
}
