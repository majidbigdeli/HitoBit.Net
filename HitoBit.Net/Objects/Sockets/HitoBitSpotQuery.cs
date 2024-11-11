using HitoBit.Net.Objects.Internal;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.Objects.Sockets
{
    internal class HitoBitSpotQuery<T> : Query<T> where T : HitoBitResponse
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public HitoBitSpotQuery(HitoBitSocketQuery request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ListenerIdentifiers = new HashSet<string> { request.Id.ToString() };
        }

        public override CallResult<T> HandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            if (message.Data.Status != 200)
            {
                if (message.Data.Status == 418 || message.Data.Status == 429)
                {
                    // Rate limit error 
                    return new CallResult<T>(new HitoBitRateLimitError(message.Data.Error!.Code, message.Data.Error!.Message, null)
                    {
                        RetryAfter = message.Data.Error.Data!.RetryAfter
                    }, message.OriginalData);
                }

                return new CallResult<T>(new ServerError(message.Data.Error!.Code, message.Data.Error!.Message), message.OriginalData);
            }

            return new CallResult<T>(message.Data, message.OriginalData, null);
        }
    }
}
