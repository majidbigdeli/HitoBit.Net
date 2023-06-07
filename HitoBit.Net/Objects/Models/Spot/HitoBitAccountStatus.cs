using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Account status info
    /// </summary>
    public class HitoBitAccountStatus
    {
        /// <summary>
        /// The result status
        /// </summary>
        [JsonProperty("data")]
        public string? Data { get; set; }
    }
}
