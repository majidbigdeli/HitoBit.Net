using HitoBit.Net.Objects.Models.Spot.Socket;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Futures book price
    /// </summary>
    public record HitoBitFuturesStreamBookPrice : HitoBitStreamBookPrice
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? TransactionTime { get; set; }
        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonPropertyName("E"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime EventTime { get; set; }

        /// <summary>
        /// The type of the event
        /// </summary>
        [JsonPropertyName("e")] 
        public string Event { get; set; } = string.Empty;
    }
}
