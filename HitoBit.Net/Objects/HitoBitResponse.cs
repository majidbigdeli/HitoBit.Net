using System.Collections.Generic;
using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.Objects
{
    /// <summary>
    /// HitoBit response
    /// </summary>
    /// <typeparam name="T">Type of the data</typeparam>
    public class HitoBitResponse<T>
    {
        /// <summary>
        /// Data result
        /// </summary>
        public T Result { get; set; } = default!;
        /// <summary>
        /// Rate limit info
        /// </summary>
        public IEnumerable<HitoBitCurrentRateLimit> Ratelimits { get; set; } = new List<HitoBitCurrentRateLimit>();
    }
}
