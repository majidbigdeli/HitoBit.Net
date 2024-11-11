using HitoBit.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Spot.AutoInvest
{
    /// <summary>
    /// Auto invest index info
    /// </summary>
    public record HitoBitAutoInvestIndex
    {
        /// <summary>
        /// Index id
        /// </summary>
        [JsonPropertyName("indexId")]
        public long IndexId { get; set; }
        /// <summary>
        /// Index name
        /// </summary>
        [JsonPropertyName("indexName")]
        public string IndexName { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public AutoInvestIndexStatus? Status { get; set; }
        /// <summary>
        /// Asset allocation
        /// </summary>
        [JsonPropertyName("assetAllocation")]
        public IEnumerable<HitoBitAutoInvestIndexSUB> AssetAllocation { get; set; } = Array.Empty<HitoBitAutoInvestIndexSUB>();
    }

    /// <summary>
    /// 
    /// </summary>
    public record HitoBitAutoInvestIndexSUB
    {
        /// <summary>
        /// Target asset
        /// </summary>
        [JsonPropertyName("targetAsset")]
        public string TargetAsset { get; set; } = string.Empty;
        /// <summary>
        /// Allocation
        /// </summary>
        [JsonPropertyName("allocation")]
        public decimal Allocation { get; set; }
    }


}
