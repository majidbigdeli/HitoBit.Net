using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    /// <summary>
    /// Sub accounts margin summary
    /// </summary>
    public class HitoBitSubAccountsMarginSummary
    {
        /// <summary>
        /// Total btc asset
        /// </summary>
        public decimal TotalAssetOfBtc { get; set; }
        /// <summary>
        /// Total liability
        /// </summary>
        public decimal TotalLiabilityOfBtc { get; set; }
        /// <summary>
        /// Total net btc
        /// </summary>
        public decimal TotalNetAssetOfBtc { get; set; }
        /// <summary>
        /// Sub account details
        /// </summary>
        [JsonProperty("subAccountList")]
        public IEnumerable<HitoBitSubAccountMarginInfo> SubAccounts { get; set; } = Array.Empty<HitoBitSubAccountMarginInfo>();
    }

    /// <summary>
    /// Sub account margin info
    /// </summary>
    public class HitoBitSubAccountMarginInfo
    {
        /// <summary>
        /// Sub account email
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Total btc asset
        /// </summary>
        public decimal TotalAssetOfBtc { get; set; }
        /// <summary>
        /// Total liability
        /// </summary>
        public decimal TotalLiabilityOfBtc { get; set; }
        /// <summary>
        /// Total net btc
        /// </summary>
        public decimal TotalNetAssetOfBtc { get; set; }
    }
}
