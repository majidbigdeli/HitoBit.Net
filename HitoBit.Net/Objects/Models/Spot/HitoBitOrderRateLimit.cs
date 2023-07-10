namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Rate limit info
    /// </summary>
    public class HitoBitCurrentRateLimit: HitoBitRateLimit
    {
        /// <summary>
        /// The current used amount
        /// </summary>
        public int Count { get; set; }
    }
}
