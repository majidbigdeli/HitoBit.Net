﻿using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Transfer type
    /// </summary>
    public enum WithdrawDepositTransferType
    {
        /// <summary>
        /// Internal transfer
        /// </summary>
        [Map("1")]
        Internal,
        /// <summary>
        /// External transfer
        /// </summary>
        [Map("0")]
        External
    }
}
