using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// User's position mode
    /// </summary>
    public class HitoBitFuturesPositionMode
    {
        /// <summary>
        /// true": Hedge Mode mode; "false": One-way Mode
        /// </summary>
        [JsonProperty("dualSidePosition"), JsonConverter(typeof(PositionModeConverter))]
        public PositionMode PositionMode { get; set; }
    }
}
