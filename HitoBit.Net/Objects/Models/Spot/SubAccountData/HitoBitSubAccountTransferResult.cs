namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    /// <summary>
    /// Sub account transfer result
    /// </summary>
    public record HitoBitSubAccountTransferResult
    {
        /// <summary>
        /// Whether the transfer was successful
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        /// <summary>
        /// The transaction id of the transfer
        /// </summary>
        [JsonPropertyName("txnId")]
        public string? TransactionId { get; set; }
    }
}
