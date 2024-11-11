namespace HitoBit.Net.Objects.Models.Futures.AlgoOrders
{
    /// <summary>
    /// Algo order result
    /// </summary>
    public record HitoBitAlgoOrderResult: HitoBitResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("clientAlgoId")]
        public string ClientAlgoId { get; set; } = string.Empty;
        /// <summary>
        /// Successful
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
