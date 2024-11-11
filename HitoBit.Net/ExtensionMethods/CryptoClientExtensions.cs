using HitoBit.Net.Clients;
using HitoBit.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the HitoBit REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IHitoBitRestClient HitoBit(this ICryptoRestClient baseClient) => baseClient.TryGet<IHitoBitRestClient>(() => new HitoBitRestClient());

        /// <summary>
        /// Get the HitoBit Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IHitoBitSocketClient HitoBit(this ICryptoSocketClient baseClient) => baseClient.TryGet<IHitoBitSocketClient>(() => new HitoBitSocketClient());
    }
}
