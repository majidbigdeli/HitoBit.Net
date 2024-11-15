﻿using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Account status
    /// </summary>
    public enum PortfolioMarginAccountStatus
    {
        /// <summary>
        /// Normal
        /// </summary>
        [Map("NORMAL")]
        Normal,
        /// <summary>
        /// Margin call
        /// </summary>
        [Map("MARGIN_CALL")]
        MarginCall,
        /// <summary>
        /// Supply margin
        /// </summary>
        [Map("SUPPLY_MARGIN")]
        SupplyMargin,
        /// <summary>
        /// Reduce only
        /// </summary>
        [Map("REDUCE_ONLY")]
        ReduceOnly,
        /// <summary>
        /// Active liquidation
        /// </summary>
        [Map("ACTIVE_LIQUIDATION")]
        ActiveLiquidation,
        /// <summary>
        /// Force liquidation
        /// </summary>
        [Map("FORCE_LIQUIDATION")]
        ForceLiquidation,
        /// <summary>
        /// Bankruped
        /// </summary>
        [Map("BANKRUPTED")]
        Bankrupted
    }
}
