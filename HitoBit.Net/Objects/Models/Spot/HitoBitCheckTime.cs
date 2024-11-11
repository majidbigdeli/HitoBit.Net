namespace HitoBit.Net.Objects.Models.Spot
{
    internal record HitoBitCheckTime
    {
        [JsonPropertyName("serverTime"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ServerTime { get; set; }
    }
}
