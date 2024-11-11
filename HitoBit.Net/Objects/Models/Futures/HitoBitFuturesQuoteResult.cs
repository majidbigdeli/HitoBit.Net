using HitoBit.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Convert quote result
    /// </summary>
    public record HitoBitFuturesQuoteResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("orderStatus")]
        public ConvertOrderStatus Status { get; set; }
    }


}
