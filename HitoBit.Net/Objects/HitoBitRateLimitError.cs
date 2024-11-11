namespace HitoBit.Net.Objects
{
    /// <summary>
    /// HitoBit rate limit error
    /// </summary>
    public class HitoBitRateLimitError : ServerRateLimitError
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public HitoBitRateLimitError(string message) : base(message)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public HitoBitRateLimitError(int? code, string message, object? data) : base(code, message, data)
        {
        }
    }
}
