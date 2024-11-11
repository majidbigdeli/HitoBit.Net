namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    /// <summary>
    /// Transaction
    /// </summary>
    public record HitoBitSubAccountTransaction
    {
        /// <summary>
        /// The transaction id
        /// </summary>
        [JsonPropertyName("txnId"), JsonConverter(typeof(NumberStringConverter))]
        public string TransactionId { get; set; } = string.Empty;
    }
}
