using System;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Information about leverage of symbol changed
    /// </summary>
    public class HitoBitFuturesStreamConfigUpdate : HitoBitStreamEvent
    {
        /// <summary>
        /// Leverage Update data
        /// </summary>
        [JsonProperty("ac")]
        public HitoBitFuturesStreamLeverageUpdateData LeverageUpdateData { get; set; } = new HitoBitFuturesStreamLeverageUpdateData();

        /// <summary>
        /// Position mode Update data
        /// </summary>
        [JsonProperty("ai")]
        public HitoBitFuturesStreamConfigUpdateData ConfigUpdateData { get; set; } = new HitoBitFuturesStreamConfigUpdateData();

        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// The listen key the update was for
        /// </summary>
        public string ListenKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// Config update data
    /// </summary>
    public class HitoBitFuturesStreamLeverageUpdateData
    {
        /// <summary>
        /// The symbol this balance is for
        /// </summary>
        [JsonProperty("s")]
        public string? Symbol { get; set; }

        /// <summary>
        /// The symbol this leverage is for
        /// </summary>
        [JsonProperty("l")]
        public int Leverage { get; set; }
    }

    /// <summary>
    /// Position mode update data
    /// </summary>
    public class HitoBitFuturesStreamConfigUpdateData
    {
        /// <summary>
        /// Multi-Assets Mode
        /// </summary>
        [JsonProperty("j")]
        public bool MultiAssetMode { get; set; }
    }
}
