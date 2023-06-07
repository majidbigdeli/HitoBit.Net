using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models
{
    /// <summary>
    /// Query result
    /// </summary>
    public class HitoBitResult
    {
        /// <summary>
        /// Result code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Query result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class HitoBitResult<T>: HitoBitResult
    {
        /// <summary>
        /// The data
        /// </summary>
        public T Data { get; set; } = default!;
    }
}
