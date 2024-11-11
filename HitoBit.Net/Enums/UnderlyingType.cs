using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Underlying Type
    /// </summary>
    public enum UnderlyingType
    {
        /// <summary>
        /// Coin
        /// </summary>
        [Map("COIN")]
        Coin,
        /// <summary>
        /// Index
        /// </summary>
        [Map("INDEX")]
        Index
    }
}
