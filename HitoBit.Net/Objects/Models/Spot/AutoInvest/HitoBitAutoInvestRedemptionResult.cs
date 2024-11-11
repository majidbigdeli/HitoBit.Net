using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Spot.AutoInvest
{
    /// <summary>
    /// Redemption result
    /// </summary>
    public record HitoBitAutoInvestRedemptionResult
    {
        /// <summary>
        /// Redemption id
        /// </summary>
        [JsonPropertyName("redemptionId")]
        public long RedemptionId { get; set; }
    }

}
