using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    internal record HitoBitSubAccountAssetTransferHistoryList
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("futuresType")]
        [JsonConverter(typeof(EnumConverter))]
        public FuturesAccountType AccountType { get; set; }

        /// <summary>
        /// Transfers
        /// </summary>
        [JsonPropertyName("transfers")]
        public IEnumerable<HitoBitSubAccountAssetTransferHistory> Transfers { get; set; } =
            new List<HitoBitSubAccountAssetTransferHistory>();

    }

    /// <summary>
    /// HitoBit sub account transfer
    /// </summary>
    public record HitoBitSubAccountAssetTransferHistory
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("tranId")]
        public long TransactionId { get; set; }

        /// <summary>
        /// From email
        /// </summary>
        [JsonPropertyName("from")]
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// To email
        /// </summary>
        [JsonPropertyName("to")]
        public string To { get; set; } = string.Empty;

        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The time transaction was created
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
    }
}
