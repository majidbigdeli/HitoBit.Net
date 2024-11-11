using HitoBit.Net.Converters;
using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Types of indicators
    /// </summary>
    public enum IndicatorType
    {
        /// <summary>
        /// Unfilled ratio
        /// </summary>
        [Map("UFR")]
        UnfilledRatio,
        /// <summary>
        /// Expired orders ratio
        /// </summary>
        [Map("IFER")]
        ExpirationRatio,
        /// <summary>
        /// Canceled orders ratio
        /// </summary>
        [Map("GCR")]
        CancelationRatio
    }
}
