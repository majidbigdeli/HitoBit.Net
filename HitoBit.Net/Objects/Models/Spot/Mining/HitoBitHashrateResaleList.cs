using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.Mining
{
    /// <summary>
    /// Resale list
    /// </summary>
    public class HitoBitHashrateResaleList
    {
        /// <summary>
        /// Total number of results
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Details
        /// </summary>
        [JsonProperty("configDetails")]
        public IEnumerable<HitoBitHashrateResaleItem> ResaleItmes { get; set; } = Array.Empty<HitoBitHashrateResaleItem>();
    }

    /// <summary>
    /// Resale item
    /// </summary>
    public class HitoBitHashrateResaleItem
    {
        /// <summary>
        /// Mining id
        /// </summary>
        public int ConfigId { get; set; }
        /// <summary>
        /// From user
        /// </summary>
        public string PoolUserName { get; set; } = string.Empty;
        /// <summary>
        /// To user
        /// </summary>
        public string ToPoolUserName { get; set; } = string.Empty;
        /// <summary>
        /// Algorithm
        /// </summary>
        public string AlgoName { get; set; } = string.Empty;
        /// <summary>
        /// Hash rate
        /// </summary>
        public decimal Hashrate { get; set; }
        /// <summary>
        /// Start day
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartDay { get; set; }
        /// <summary>
        /// End day
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime EndDay { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [JsonConverter(typeof(HashrateResaleStatusConverter))]
        public HashrateResaleStatus Status { get; set; }
    }
}
