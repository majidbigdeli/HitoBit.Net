using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    internal class HitoBitSubAccountAsset
    {
        public bool Success { get; set; } = true;
        [JsonProperty("msg")]
        public string Message { get; set; } = string.Empty;
        public IEnumerable<HitoBitBalance> Balances { get; set; } = Array.Empty<HitoBitBalance>();
    }
}
