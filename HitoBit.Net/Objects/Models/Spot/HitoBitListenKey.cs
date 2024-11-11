namespace HitoBit.Net.Objects.Models.Spot
{
    internal record HitoBitListenKey
    {
        [JsonPropertyName("listenKey")]
        public string ListenKey { get; set; } = string.Empty;
    }
}
