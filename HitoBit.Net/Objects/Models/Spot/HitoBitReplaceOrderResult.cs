using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// The result of replacing an order
    /// </summary>
    public record HitoBitReplaceOrderResult: HitoBitReplaceResult
    {
        /// <summary>
        /// Cancel result
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("cancelResult")]
        public OrderOperationResult CancelResult { get; set; }
        /// <summary>
        /// New order result
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("newOrderResult")]
        public OrderOperationResult NewOrderResult { get; set; }
        /// <summary>
        /// Cancel order response. Make sure to check that the CancelResult is Success, else the CancelResponse.Message will contain more info
        /// </summary>
        [JsonPropertyName("cancelResponse")]
        public HitoBitReplaceCancelOrder? CancelResponse { get; set; }
        /// <summary>
        /// New order response. Make sure to check that the NewOrderResult is Success, else the NewOrderResponse.Message will contain more info
        /// </summary>
        [JsonPropertyName("newOrderResponse")]
        public HitoBitReplaceOrder? NewOrderResponse { get; set; }
    }

    /// <summary>
    /// Replace order
    /// </summary>
    public record HitoBitReplaceOrder: HitoBitPlacedOrder
    {
        /// <summary>
        /// Failure message
        /// </summary>
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
        /// <summary>
        /// Error code if not successful
        /// </summary>
        [JsonPropertyName("code")]
        public int? Code { get; set; }
    }

    /// <summary>
    /// Replace cancel order info
    /// </summary>
    public record HitoBitReplaceCancelOrder: HitoBitOrderBase
    {
        /// <summary>
        /// Failure message
        /// </summary>
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
        /// <summary>
        /// Error code if not successful
        /// </summary>
        [JsonPropertyName("code")]
        public int? Code { get; set; }
    }

    /// <summary>
    /// Replace result
    /// </summary>
    public record HitoBitReplaceResult
    {
        /// <summary>
        /// Failure message
        /// </summary>
        [JsonPropertyName("msg")]
        public string? Message { get; set; }
        /// <summary>
        /// Error code if not successful
        /// </summary>
        [JsonPropertyName("code")]
        public int? Code { get; set; }
    }
}
