namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// The result of transferring
    /// </summary>
    public record HitoBitTransaction
    {
        /// <summary>
        /// The Transaction id as assigned by HitoBit
        /// </summary>
        [JsonPropertyName("tranId")]
        public long TransactionId { get; set; }
    }
}
