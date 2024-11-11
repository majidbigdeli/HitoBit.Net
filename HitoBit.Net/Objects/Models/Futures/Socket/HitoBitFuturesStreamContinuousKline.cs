using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Objects.Models.Spot.Socket;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Wrapper for continuous kline information for a symbol
    /// </summary>
    public record HitoBitStreamContinuousKlineData: HitoBitStreamEvent, IHitoBitStreamKlineData
    {
        /// <summary>
        /// The symbol the data is for
        /// </summary>
        [JsonPropertyName("ps")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The contract type
        /// </summary>
        [JsonPropertyName("ct")]
        public ContractType ContractType { get; set; } = ContractType.Unknown;

        /// <summary>
        /// The data
        /// </summary>
        [JsonPropertyName("k")]
        [JsonConverter(typeof(InterfaceConverter<HitoBitStreamKline, IHitoBitStreamKline>))]
        public IHitoBitStreamKline Data { get; set; } = default!;
    }
}
