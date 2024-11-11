using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Type of auto close
    /// </summary>
    public enum AutoCloseType
    {
        /// <summary>
        /// ADL
        /// </summary>
        [Map("ADL")]
        ADL,

        /// <summary>
        /// Liquidation
        /// </summary>
        [Map("LIQUIDATION")]
        Liquidation
    }
}
