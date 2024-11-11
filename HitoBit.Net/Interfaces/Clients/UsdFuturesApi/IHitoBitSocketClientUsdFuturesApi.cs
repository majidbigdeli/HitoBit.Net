namespace HitoBit.Net.Interfaces.Clients.UsdFuturesApi
{
    /// <summary>
    /// HitoBit USD futures streams
    /// </summary>
    public interface IHitoBitSocketClientUsdFuturesApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        IHitoBitSocketClientUsdFuturesApiShared SharedClient { get; }

        /// <summary>
        /// Account streams and queries
        /// </summary>
        IHitoBitSocketClientUsdFuturesApiAccount Account { get; }
        /// <summary>
        /// Exchange data streams and queries
        /// </summary>
        IHitoBitSocketClientUsdFuturesApiExchangeData ExchangeData { get; }
        /// <summary>
        /// Trading data and queries
        /// </summary>
        IHitoBitSocketClientUsdFuturesApiTrading Trading { get; }
    }
}
