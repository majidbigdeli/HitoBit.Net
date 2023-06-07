using HitoBit.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// Types of indicators
    /// </summary>
    [JsonConverter(typeof(IndicatorTypeConverter))]
    public enum IndicatorType
    {
        /// <summary>
        /// Unfilled ratio
        /// </summary>
        UnfilledRatio,
        /// <summary>
        /// Expired orders ratio
        /// </summary>
        ExpirationRatio,
        /// <summary>
        /// Canceled orders ratio
        /// </summary>
        CancelationRatio
    }
}
