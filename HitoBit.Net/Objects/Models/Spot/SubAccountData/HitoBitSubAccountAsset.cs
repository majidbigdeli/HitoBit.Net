namespace HitoBit.Net.Objects.Models.Spot.SubAccountData
{
    internal record HitoBitSubAccountAsset
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;
        [JsonPropertyName("msg")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("balances")]
        public IEnumerable<HitoBitBalance> Balances { get; set; } = Array.Empty<HitoBitBalance>();
    }
}
