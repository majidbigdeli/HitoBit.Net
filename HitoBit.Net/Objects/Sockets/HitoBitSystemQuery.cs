using HitoBit.Net.Objects.Internal;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.Objects.Sockets
{
    internal class HitoBitSystemQuery<T> : Query<T> where T: HitoBitSocketQueryResponse
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public HitoBitSystemQuery(HitoBitSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ListenerIdentifiers = new HashSet<string> { request.Id.ToString() };
        }
    }
}
