using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// BNB burn for fee reduction status
    /// </summary>
    public record HitoBitBnbBurnStatus
    {
        /// <summary>
        /// Fee burn status
        /// </summary>
        [JsonPropertyName("feeBurn")]
        public bool FeeBurn { get; set; }
    }
}
