using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Internal
{
    internal class HitoBitSocketMessage
    {
        [JsonProperty("method")]
        public string Method { get; set; } = "";

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    internal class HitoBitSocketRequest : HitoBitSocketMessage
    {
        [JsonProperty("params")]
        public string[] Params { get; set; } = Array.Empty<string>();
    }

    internal class HitoBitSocketQuery : HitoBitSocketMessage
    {
        [JsonProperty("params")]
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
    }
}
