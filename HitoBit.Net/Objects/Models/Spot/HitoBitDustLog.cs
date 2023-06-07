using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Dust log response details
    /// </summary>
    public class HitoBitDustLogList
    {
        /// <summary>
        /// Total counts of exchange
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Rows
        /// </summary>
        public IEnumerable<HitoBitDustLog> UserAssetDribblets { get; set; } = Array.Empty<HitoBitDustLog>();
    }

    /// <summary>
    /// Dust log details
    /// </summary>
    public class HitoBitDustLog
    {
        /// <summary>
        /// Total transferred
        /// </summary>
        [JsonProperty("totalTransferedAmount")]
        public decimal TransferredTotal { get; set; }
        /// <summary>
        /// Total service charge
        /// </summary>
        [JsonProperty("totalServiceChargeAmount")]
        public decimal ServiceChargeTotal { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonProperty("transId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Detail logs
        /// </summary>
        [JsonProperty("userAssetDribbletDetails")]
        public IEnumerable<HitoBitDustLogDetails> Logs { get; set; } = Array.Empty<HitoBitDustLogDetails>();
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("operateTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime OperateTime { get; set; }
    }

    /// <summary>
    /// Dust log entry details
    /// </summary>
    public class HitoBitDustLogDetails
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonProperty("transId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Service charge
        /// </summary>
        [JsonProperty("serviceChargeAmount")]
        public decimal ServiceChargeQuantity { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("operateTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// Transferred quantity
        /// </summary>
        [JsonProperty("transferedAmount")]
        public decimal TransferredQuantity { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty("fromAsset")]
        public string FromAsset { get; set; } = string.Empty;
    }
}
