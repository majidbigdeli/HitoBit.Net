﻿namespace HitoBit.Net.Objects.Models.Spot.Brokerage.SubAccountData
{
    /// <summary>
    /// Transfer Result
    /// </summary>
    public record HitoBitBrokerageTransferResult
    {
        /// <summary>
        /// Transaction Id
        /// </summary>
        [JsonPropertyName("txnId")]
        public long Id { get; set; }
        
        /// <summary>
        /// Client Transfer Id
        /// </summary>
        [JsonPropertyName("clientTranId")]
        public string ClientTransferId { get; set; } = string.Empty;
    }
}