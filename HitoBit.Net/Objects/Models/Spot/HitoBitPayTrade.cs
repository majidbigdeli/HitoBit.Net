using System;
using System.Collections.Generic;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// HitoBit pay trade
    /// </summary>
    public class HitoBitPayTrade
    {
        /// <summary>
        /// Order type
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public PayOrderType OrderType { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Fund details
        /// </summary>
        [JsonProperty("fundsDetail")]
        public IEnumerable<HitoBitPayTradeDetails> Details { get; set; } = Array.Empty<HitoBitPayTradeDetails>();
        /// <summary>
        /// Payer info
        /// </summary>
        [JsonProperty("payerInfo")]
        public HitoBitPayTradePayerInfo PayerInfo { get; set; } = new HitoBitPayTradePayerInfo();
        /// <summary>
        /// Receiver info
        /// </summary>
        [JsonProperty("receiverInfo")]
        public HitoBitPayTradeReceiverInfo ReceiverInfo { get; set; } = new HitoBitPayTradeReceiverInfo();
    }

    /// <summary>
    /// Pay trade funds details
    /// </summary>
    public class HitoBitPayTradeDetails
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
    }

    /// <summary>
    /// Pay trade payer info
    /// </summary>
    public class HitoBitPayTradePayerInfo
    {
        /// <summary>
        /// Nickname or merchant name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Account type，USER for personal，MERCHANT for merchant
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// HitoBit uid
        /// </summary>
        [JsonProperty("HitoBitId")]
        public string HitoBitId { get; set; } = string.Empty;
        /// <summary>
        /// HitoBit pay id
        /// </summary>
        [JsonProperty("accountId")]
        public string AccountId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Pay trade receiver info
    /// </summary>
    public class HitoBitPayTradeReceiverInfo
    {
        /// <summary>
        /// Nickname or merchant name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Account type，USER for personal，MERCHANT for merchant
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// HitoBit uid
        /// </summary>
        [JsonProperty("HitoBitId")]
        public string HitoBitId { get; set; } = string.Empty;
        /// <summary>
        /// HitoBit pay id
        /// </summary>
        [JsonProperty("accountId")]
        public string AccountId { get; set; } = string.Empty;
        /// <summary>
        /// International area code
        /// </summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; } = string.Empty;
        /// <summary>
        /// Phone number
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// Country code
        /// </summary>
        [JsonProperty("mobileCode")]
        public string MobileCode { get; set; } = string.Empty;
    }
}
