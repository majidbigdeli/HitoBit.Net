namespace HitoBit.Net.Objects.Internal
{
    internal class HitoBitSocketMessage
    {
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    internal class HitoBitSocketRequest : HitoBitSocketMessage
    {
        [JsonPropertyName("params")]
        public string[] Params { get; set; } = Array.Empty<string>();
    }

    internal class HitoBitSocketQuery : HitoBitSocketMessage
    {
        [JsonPropertyName("params")]
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}
