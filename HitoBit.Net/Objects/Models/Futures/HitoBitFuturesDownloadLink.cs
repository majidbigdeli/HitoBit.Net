using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// Transaction download info
    /// </summary>
    public class HitoBitFuturesDownloadLink
    {
        /// <summary>
        /// Download id
        /// </summary>
        public string DownloadId { get; set; } = string.Empty;
        /// <summary>
        /// Is ready to download
        /// </summary>
        public DownloadStatus Status { get; set; }
        /// <summary>
        /// Download url
        /// </summary>
        public string Url { get; set; } = string.Empty;
        /// <summary>
        /// Link expiration time
        /// </summary>
        [JsonProperty("expirationTimestamp")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ExpirationTime { get; set; }
        /// <summary>
        /// Is expired
        /// </summary>
        public bool? IsExpired { get; set; }
    }
}
