using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    internal class HitoBitSubAccountWrapper
    {
        public IEnumerable<HitoBitSubAccount>? SubAccounts { get; set; }
    }

    /// <summary>
    /// Sub account details
    /// </summary>
    public class HitoBitSubAccount
    {
        /// <summary>
        /// The email associated with the sub account
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Is account frozen
        /// </summary>
        public bool IsFreeze { get; set; } = false;
        /// <summary>
        /// The time the sub account was created
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreateTime { get; set; }
    }
}
