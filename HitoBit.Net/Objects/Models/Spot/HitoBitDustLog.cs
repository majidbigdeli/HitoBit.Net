namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Dust log response details
    /// </summary>
    public record HitoBitDustLogList
    {
        /// <summary>
        /// Total counts of exchange
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// Rows
        /// </summary>
        [JsonPropertyName("userAssetDribblets")]
        public IEnumerable<HitoBitDustLog> UserAssetDribblets { get; set; } = Array.Empty<HitoBitDustLog>();
    }

    /// <summary>
    /// Dust log details
    /// </summary>
    public record HitoBitDustLog
    {
        /// <summary>
        /// Total transferred
        /// </summary>
        [JsonPropertyName("totalTransferedAmount")]
        public decimal TransferredTotal { get; set; }
        /// <summary>
        /// Total service charge
        /// </summary>
        [JsonPropertyName("totalServiceChargeAmount")]
        public decimal ServiceChargeTotal { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("transId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Detail logs
        /// </summary>
        [JsonPropertyName("userAssetDribbletDetails")]
        public IEnumerable<HitoBitDustLogDetails> Logs { get; set; } = Array.Empty<HitoBitDustLogDetails>();
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("operateTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime OperateTime { get; set; }
    }

    /// <summary>
    /// Dust log entry details
    /// </summary>
    public record HitoBitDustLogDetails
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("transId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Service charge
        /// </summary>
        [JsonPropertyName("serviceChargeAmount")]
        public decimal ServiceChargeQuantity { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("operateTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// Transferred quantity
        /// </summary>
        [JsonPropertyName("transferedAmount")]
        public decimal TransferredQuantity { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("fromAsset")]
        public string FromAsset { get; set; } = string.Empty;
    }
}
