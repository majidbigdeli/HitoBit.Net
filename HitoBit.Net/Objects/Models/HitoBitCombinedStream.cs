namespace HitoBit.Net.Objects.Models
{
    /// <summary>
    /// Represents the hitobit result for combined data on a single socket connection
    /// See on https://github.com/hitobit-exchange/hitobit-official-api-docs/blob/master/web-socket-streams.md
    /// Combined streams
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record HitoBitCombinedStream<T>
    {
        /// <summary>
        /// The stream combined
        /// </summary>
        [JsonPropertyName("stream")]
        public string Stream { get; set; } = string.Empty;

        /// <summary>
        /// The data of stream
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
