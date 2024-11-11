namespace HitoBit.Net.Objects.Models.Futures
{
    /// <summary>
    /// The result of cancel all orders
    /// </summary>
    public record HitoBitFuturesCancelAllOrders
    {
        /// <summary>
        /// The execution code
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// The execution message
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; } = string.Empty;
    }
}
