using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Add/Remove liquidity type
    /// </summary>
    public enum LiquidityType
    {
        /// <summary>
        /// Add/Remove single asset
        /// </summary>
        [Map("SINGLE")]
        Single,
        /// <summary>
        /// Add/Remove combination of all coins
        /// </summary>
        [Map("COMBINATION")]
        Combined
    }
}
