using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Status of the HitoBit system
    /// </summary>
    public enum SystemStatus
    {
        /// <summary>
        /// Operational
        /// </summary>
        [Map("0")]
        Normal,
        /// <summary>
        /// In maintenance
        /// </summary>
        [Map("1")]
        Maintenance
    }
}
