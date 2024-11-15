﻿using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Simple earn redemption type
    /// </summary>
    public enum RedemptionType
    {
        /// <summary>
        /// Redeem to spot account
        /// </summary>
        [Map("MATURE")]
        ToSpot,
        /// <summary>
        /// Redeem to flexible product
        /// </summary>
        [Map("NEW_TRANSFERRED")]
        ToFlexibleProduct,
        /// <summary>
        /// Early redemption
        /// </summary>
        [Map("AHEAD")]
        EarlyRedemption
    }
}
