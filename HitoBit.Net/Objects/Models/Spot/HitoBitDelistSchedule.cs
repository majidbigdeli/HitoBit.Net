namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Spot symbol delist info
    /// </summary>
    public record HitoBitDelistSchedule
    {
        /// <summary>
        /// Delist time
        /// </summary>
        [JsonPropertyName("delistTime"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime DelistTime { get; set; }

        /// <summary>
        /// Symbols being delisted
        /// </summary>
        [JsonPropertyName("symbols")]
        public IEnumerable<string> Symbols { get; set; } = new List<string>();
    }
}
