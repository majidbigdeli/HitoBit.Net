using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Multi asset mode info
    /// </summary>
    public class HitoBitFuturesMultiAssetMode
    {
        /// <summary>
        /// Is multi assets mode enabled
        /// </summary>
        [JsonProperty("multiAssetsMargin")]
        public bool MultiAssetMode { get; set; }
    }
}
