namespace HitoBit.Net.Objects.Models
{
    /// <summary>
    /// Query results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record HitoBitQueryRecords<T>
    {
        /// <summary>
        /// The list records
        /// </summary>
        [JsonPropertyName("rows")]
        public IEnumerable<T> Rows { get; set; } = Array.Empty<T>();
        /// <summary>
        /// The total count of the records
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
