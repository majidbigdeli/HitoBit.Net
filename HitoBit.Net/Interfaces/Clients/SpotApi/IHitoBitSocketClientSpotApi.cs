namespace HitoBit.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Spot API socket subscriptions and requests
    /// </summary>
    public interface IHitoBitSocketClientSpotApi : ISocketApiClient
    {
        /// <summary>
        /// Account streams and queries
        /// </summary>
        IHitoBitSocketClientSpotApiAccount Account { get; }
        /// <summary>
        /// Exchange data streams and queries
        /// </summary>
        IHitoBitSocketClientSpotApiExchangeData ExchangeData { get; }
        /// <summary>
        /// Trading data and queries
        /// </summary>
        IHitoBitSocketClientSpotApiTrading Trading { get; }

        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exhanges to allow for a common implementation for different exchanges.
        /// </summary>
        IHitoBitSocketClientSpotApiShared SharedClient { get; }
    }
}