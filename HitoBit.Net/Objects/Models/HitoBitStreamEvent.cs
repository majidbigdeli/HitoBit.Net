namespace HitoBit.Net.Objects.Models
{
    /// <summary>
    /// A event received by a HitoBit websocket
    /// </summary>
    public record HitoBitStreamEvent
    {
        /// <summary>
        /// The type of the event
        /// </summary>
        [JsonPropertyName("e")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonPropertyName("E"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime EventTime { get; set; }
    }
}
