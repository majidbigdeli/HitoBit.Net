using System;
using HitoBit.Net.Interfaces;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Mark price update
    /// </summary>
    public class HitoBitFuturesStreamMarkPrice: HitoBitStreamEvent, IHitoBitFuturesMarkPrice
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonProperty("p")]
        public decimal MarkPrice { get; set; }

        /// <summary>
        /// Estimated Settle Price, only useful in the last hour before the settlement starts
        /// </summary>
        [JsonProperty("P")]
        public decimal EstimatedSettlePrice { get; set; }

        /// <summary>
        /// Next Funding Rate
        /// </summary>
        [JsonProperty("r")]
        public decimal? FundingRate { get; set; }
        
        /// <summary>
        /// Next Funding Time
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime NextFundingTime { get; set; }
    }

    /// <summary>
    /// Mark price update
    /// </summary>
    public class HitoBitFuturesUsdtStreamMarkPrice : HitoBitFuturesStreamMarkPrice
    {
        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonProperty("i")]
        public decimal IndexPrice { get; set; }
    }

    /// <summary>
    /// Mark price update
    /// </summary>
    public class HitoBitFuturesCoinStreamMarkPrice : HitoBitFuturesStreamMarkPrice
    {
        /// <summary>
        /// Mark Price
        /// </summary>
        [JsonProperty("P")]
        public new decimal EstimatedSettlePrice { get; set; }
    }
}
