using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.Objects
{
    /// <summary>
    /// HitoBit response
    /// </summary>
    public class HitoBitResponse
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }
        /// <summary>
        /// Result status
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }
        /// <summary>
        /// Error info
        /// </summary>
        [JsonPropertyName("error")]
        public HitoBitResponseError? Error { get; set; }

        /// <summary>
        /// Rate limit info
        /// </summary>
        [JsonPropertyName("rateLimits")]
        public IEnumerable<HitoBitCurrentRateLimit> Ratelimits { get; set; } = new List<HitoBitCurrentRateLimit>();

    }

    /// <summary>
    /// HitoBit response
    /// </summary>
    /// <typeparam name="T">Type of the data</typeparam>
    public class HitoBitResponse<T> : HitoBitResponse
    {
        /// <summary>
        /// Data result
        /// </summary>
        [JsonPropertyName("result")]
        public T Result { get; set; } = default!;
    }

    /// <summary>
    /// HitoBit error response
    /// </summary>
    public class HitoBitResponseError
    {
        /// <summary>
        /// Error code
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// Error data
        /// </summary>
        [JsonPropertyName("data")]
        public HitoBitResponseErrorData? Data { get; set; }
    }

    /// <summary>
    /// Error data
    /// </summary>
    public class HitoBitResponseErrorData 
    {
        /// <summary>
        /// Server time
        /// </summary>
        [JsonPropertyName("serverTime")]
        public DateTime? ServerTime { get; set; }
        /// <summary>
        /// Retry after time
        /// </summary>
        [JsonPropertyName("retryAfter")]
        public DateTime? RetryAfter { get; set; }
    }
}
