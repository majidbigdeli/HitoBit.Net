﻿namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Convert result
    /// </summary>
    public record HitoBitBusdConvertResult
    {
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("tranId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
