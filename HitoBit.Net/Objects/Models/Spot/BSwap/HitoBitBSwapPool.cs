﻿using System;
using System.Collections.Generic;

namespace HitoBit.Net.Objects.Models.Spot.BSwap
{
    /// <summary>
    /// Swap pool info
    /// </summary>
    public class HitoBitBSwapPool
    {
        /// <summary>
        /// Id
        /// </summary>
        public int PoolId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string PoolName { get; set; } = string.Empty;
        /// <summary>
        /// Assets in the pool
        /// </summary>
        public IEnumerable<string> Assets { get; set; } = Array.Empty<string>();
    }
}