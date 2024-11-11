using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Spot.AutoInvest
{
    /// <summary>
    /// Auto invest assets
    /// </summary>
    public record HitoBitAutoInvestAssets
    {
        /// <summary>
        /// Target assets
        /// </summary>
        [JsonPropertyName("targetAssets")]
        public IEnumerable<string> TargetAssets { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Source assets
        /// </summary>
        [JsonPropertyName("sourceAssets")]
        public IEnumerable<string> SourceAssets { get; set; } = Array.Empty<string>();
    }

}
