using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// HitoBit pay trade
    /// </summary>
    public record HitoBitPayTrade
    {
        /// <summary>
        /// Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public long? Uid { get; set; }
        /// <summary>
        /// Counter party id
        /// </summary>
        [JsonPropertyName("counterpartyId")]
        public long? CounterPartyId { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Note
        /// </summary>
        [JsonPropertyName("note")]
        public string? Note { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("orderType")]
        public PayOrderType OrderType { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("transactionTime")]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Total fee
        /// </summary>
        [JsonPropertyName("totalPaymentFee")]
        public decimal TotalPaymentFee { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Main wallet type
        /// </summary>
        [JsonPropertyName("walletType"), JsonConverter(typeof(EnumConverter))]
        public PayWalletType WalletType { get; set; }
        /// <summary>
        /// Fund details
        /// </summary>
        [JsonPropertyName("fundsDetail")]
        public IEnumerable<HitoBitPayTradeDetails> Details { get; set; } = Array.Empty<HitoBitPayTradeDetails>();
        /// <summary>
        /// Payer info
        /// </summary>
        [JsonPropertyName("payerInfo")]
        public HitoBitPayTradeParticipantInfo PayerInfo { get; set; } = new HitoBitPayTradeParticipantInfo();
        /// <summary>
        /// Receiver info
        /// </summary>
        [JsonPropertyName("receiverInfo")]
        public HitoBitPayTradeParticipantInfo ReceiverInfo { get; set; } = new HitoBitPayTradeParticipantInfo();
    }

    /// <summary>
    /// Pay trade funds details
    /// </summary>
    public record HitoBitPayTradeDetails
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// Pay trade receiver info
    /// </summary>
    public record HitoBitPayTradeParticipantInfo
    {
        /// <summary>
        /// Nickname or merchant name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Account type，USER for personal，MERCHANT for merchant
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// HitoBit uid
        /// </summary>
        [JsonPropertyName("hitobitId")]
        public long HitoBitId { get; set; }
        /// <summary>
        /// HitoBit pay id
        /// </summary>
        [JsonPropertyName("accountId")]
        public long AccountId { get; set; }
        /// <summary>
        /// International area code
        /// </summary>
        [JsonPropertyName("countryCode")]
        public long CountryCode { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// Country code
        /// </summary>
        [JsonPropertyName("mobileCode")]
        public string MobileCode { get; set; } = string.Empty;
        /// <summary>
        /// Unmask data
        /// </summary>
        [JsonPropertyName("unmaskData")]
        public bool? UnmaskData { get; set; }
    }
}
