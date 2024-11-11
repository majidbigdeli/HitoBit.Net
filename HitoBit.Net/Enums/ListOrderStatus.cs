using CryptoExchange.Net.Attributes;

namespace HitoBit.Net.Enums
{
    /// <summary>
    /// List order status
    /// </summary>
    public enum ListOrderStatus
    {
        /// <summary>
        /// Executing
        /// </summary>
        [Map("EXECUTING")]
        Executing,
        /// <summary>
        /// Executed
        /// </summary>
        [Map("REJECT")]
        Done,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("ALL_DONE")]
        Rejected
    }
}
