using HitoBit.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Spot.AutoInvest
{
    /// <summary>
    /// Transaction status
    /// </summary>
    public record HitoBitAutoInvestOneTimeTransactionStatus
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("transactionId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public AutoInvestOneTimeTransactionStatus Status { get; set; }
    }
}
