using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// The result of placing a new OCO order
    /// </summary>
    public class HitoBitOrderOcoList
    {
        /// <summary>
        /// The id of the order list
        /// </summary>
        [JsonProperty("orderListId")]
        public long Id { get; set; }
        /// <summary>
        /// The contingency type
        /// </summary>
        public string ContingencyType { get; set; } = string.Empty;
        /// <summary>
        /// The order list status
        /// </summary>
        [JsonConverter(typeof(ListStatusTypeConverter))]
        public ListStatusType ListStatusType { get; set; }
        /// <summary>
        /// The order status
        /// </summary>
        [JsonConverter(typeof(ListOrderStatusConverter))]
        public ListOrderStatus ListOrderStatus { get; set; }
        /// <summary>
        /// The client id of the order list
        /// </summary>
        public string ListClientOrderId { get; set; } = string.Empty;
        /// <summary>
        /// The transaction time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }
        /// <summary>
        /// The symbol of the order list
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The order in this list
        /// </summary>
        public IEnumerable<HitoBitOrderId> Orders { get; set; } = Array.Empty<HitoBitOrderId>();
        /// <summary>
        /// The order details
        /// </summary>
        public IEnumerable<HitoBitPlacedOcoOrder> OrderReports { get; set; } = Array.Empty<HitoBitPlacedOcoOrder>();
    }

    /// <summary>
    /// Order reference
    /// </summary>
    public class HitoBitOrderId
    {
        /// <summary>
        /// The symbol of the order
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The id of the order
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// The client order id
        /// </summary>
        public string ClientOrderId { get; set; } = string.Empty;
    }

    /// <summary>
    /// The result of placing a new order
    /// </summary>
    public class HitoBitPlacedOcoOrder: HitoBitOrderBase
    {
        /// <summary>
        /// The time the order was placed
        /// </summary>
        [JsonProperty("transactTime"), JsonConverter(typeof(DateTimeConverter))]
        public new DateTime CreateTime { get; set; }
    }
}
