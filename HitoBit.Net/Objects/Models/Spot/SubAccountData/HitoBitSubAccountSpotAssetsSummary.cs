using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    /// <summary>
    /// Sub accounts btc value summary
    /// </summary>
    public class HitoBitSubAccountSpotAssetsSummary
    {
        /// <summary>
        /// Total records
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Master account total asset value
        /// </summary>
        public decimal MasterAccountTotalAsset { get; set; }
        /// <summary>
        /// Sub account values
        /// </summary>
        [JsonProperty("spotSubUserAssetBtcVoList")]
        public IEnumerable<HitoBitSubAccountBtcValue> SubAccountsBtcValues { get; set; } = Array.Empty<HitoBitSubAccountBtcValue>();
    }

    /// <summary>
    /// Sub account btc value
    /// </summary>
    public class HitoBitSubAccountBtcValue
    {
        /// <summary>
        /// Sub account email
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Sub account total asset 
        /// </summary>
        public decimal TotalAsset { get; set; }
    }
}
