﻿using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Download status
    /// </summary>
    public enum DownloadStatus
    {
        /// <summary>
        /// Processing
        /// </summary>
        [Map("processing")]
        Processing,
        /// <summary>
        /// Ready for download
        /// </summary>
        [Map("completed")]
        Completed
    }
}
