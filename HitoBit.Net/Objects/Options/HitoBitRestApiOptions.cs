﻿using HitoBit.Net.Enums;
using CryptoExchange.Net.Objects.Options;

namespace HitoBit.Net.Objects.Options
{
    /// <summary>
    /// Options for HitoBit Rest API
    /// </summary>
    public class HitoBitRestApiOptions : RestApiOptions
    {
        /// <summary>
        /// A manual offset for the timestamp. Should only be used if AutoTimestamp and regular time synchronization on the OS is not reliable enough
        /// </summary>
        public TimeSpan TimestampOffset { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Whether to check the trade rules when placing new orders and what to do if the trade isn't valid
        /// </summary>
        public TradeRulesBehaviour TradeRulesBehaviour { get; set; } = TradeRulesBehaviour.None;
        /// <summary>
        /// How often the trade rules should be updated. Only used when TradeRulesBehaviour is not None
        /// </summary>
        public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);

        /// <summary>
        /// The broker reference id to use
        /// </summary>
        public string? BrokerId { get; set; }

        internal HitoBitRestApiOptions Copy()
        {
            var result = base.Copy<HitoBitRestApiOptions>();
            result.TimestampOffset = TimestampOffset;
            result.TradeRulesBehaviour = TradeRulesBehaviour;
            result.TradeRulesUpdateInterval = TradeRulesUpdateInterval;
            return result;
        }
    }
}
