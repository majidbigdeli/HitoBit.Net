using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Objects.Models.Spot.Socket;
using Newtonsoft.Json;

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
}
