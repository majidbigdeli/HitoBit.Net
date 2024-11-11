namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// Margin dust transfer info
    /// </summary>
    public record HitoBitMarginDustTransfer
    {
        /// <summary>
        /// Total service charge
        /// </summary>
        [JsonPropertyName("totalServiceCharge")]
        public decimal TotalServiceCharge { get; set; }
        /// <summary>
        /// Total transfered
        /// </summary>
        [JsonPropertyName("totalTransfered")]
        public decimal TotalTransfered { get; set; }
        /// <summary>
        /// Transfer results
        /// </summary>
        [JsonPropertyName("transferResult")]
        public IEnumerable<HitoBitMargingDustTransferResult> TransferResults { get; set; } = Array.Empty<HitoBitMargingDustTransferResult>();
    }

    /// <summary>
    /// Transfer results
    /// </summary>
    public record HitoBitMargingDustTransferResult
    {
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Source asset
        /// </summary>
        [JsonPropertyName("fromAsset")]
        public string FromAsset { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("operateTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// Service charge quantity
        /// </summary>
        [JsonPropertyName("serviceChargeAmount")]
        public decimal ServiceChargeQuantity { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("tranId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Transfered quantity
        /// </summary>
        [JsonPropertyName("transferedAmount")]
        public decimal TransferedQuantity { get; set; }
    }
}
