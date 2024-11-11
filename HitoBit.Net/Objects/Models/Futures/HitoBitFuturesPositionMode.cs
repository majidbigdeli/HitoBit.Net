using HitoBit.Net.Converters;
using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// User's position mode
    /// </summary>
    public record HitoBitFuturesPositionMode
    {
        /// <summary>
        /// true": Hedge Mode mode; "false": One-way Mode
        /// </summary>
        [JsonPropertyName("dualSidePosition")]
        public bool IsHedgeMode { get; set; }
    }
}
