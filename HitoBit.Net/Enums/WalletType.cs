﻿using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Wallet type
    /// </summary>
    public enum WalletType
    {
        /// <summary>
        /// Spot wallet
        /// </summary>
        [Map("0")]
        Spot,
        /// <summary>
        /// Funding wallet
        /// </summary>
        [Map("1")]
        Funding
    }
}
