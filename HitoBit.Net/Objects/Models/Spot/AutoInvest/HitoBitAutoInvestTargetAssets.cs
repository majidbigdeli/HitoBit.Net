using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Spot.AutoInvest
{
    /// <summary>
    /// Auto invest source asset info
    /// </summary>
    public record HitoBitAutoInvestTargetAssets
    {
        /// <summary>
        /// Target assets
        /// </summary>
        [JsonPropertyName("targetAssets")]
        public IEnumerable<string> TargetAssets { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Target asset list
        /// </summary>
        [JsonPropertyName("autoInvestAssetList")]
        public IEnumerable<HitoBitAutoInvestTargetAsset> Assets { get; set; } = Array.Empty<HitoBitAutoInvestTargetAsset>();
    }

    /// <summary>
    /// Auto invest target asset
    /// </summary>
    public record HitoBitAutoInvestTargetAsset
    {
        /// <summary>
        /// Target asset
        /// </summary>
        [JsonPropertyName("targetAsset")]
        public string TargetAsset { get; set; } = string.Empty;
        /// <summary>
        /// Target asset list
        /// </summary>
        [JsonPropertyName("roiAndDimensionTypeList")]
        public IEnumerable<HitoBitAutoInvestTargetAssetRoi> Assets { get; set; } = Array.Empty<HitoBitAutoInvestTargetAssetRoi>();
    }

    /// <summary>
    /// Auto invest target asset roi
    /// </summary>
    public record HitoBitAutoInvestTargetAssetRoi
    {
        /// <summary>
        /// Simulate ROI
        /// </summary>
        [JsonPropertyName("simulateRoi")]
        public decimal SimulateRoi { get; set; }
        /// <summary>
        /// The dimension
        /// </summary>
        [JsonPropertyName("dimensionValue")]
        public decimal DimensionValue { get; set; }
        /// <summary>
        /// The dimension unit
        /// </summary>
        [JsonPropertyName("dimensionUnit")]
        public string DimensionUnit { get; set; } = string.Empty;
    }
}
