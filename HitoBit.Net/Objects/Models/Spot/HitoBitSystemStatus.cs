using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// The status of HitoBit
    /// </summary>
    public class HitoBitSystemStatus
    {
        /// <summary>
        /// Status
        /// </summary>
        [JsonConverter(typeof(SystemStatusConverter))]
        public SystemStatus Status { get; set; }
        /// <summary>
        /// Additional info
        /// </summary>
        [JsonProperty("msg")]
        public string? Message { get; set; }
    }
}
