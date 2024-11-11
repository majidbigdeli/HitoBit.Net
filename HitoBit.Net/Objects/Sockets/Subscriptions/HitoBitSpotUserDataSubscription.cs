using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class HitoBitSpotUserDataSubscription : Subscription<HitoBitSocketQueryResponse, HitoBitSocketQueryResponse>
    {
        private static readonly MessagePath _ePath = MessagePath.Get().Property("data").Property("e");

        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly Action<DataEvent<HitoBitStreamOrderUpdate>>? _orderHandler;
        private readonly Action<DataEvent<HitoBitStreamOrderList>>? _orderListHandler;
        private readonly Action<DataEvent<HitoBitStreamPositionsUpdate>>? _positionHandler;
        private readonly Action<DataEvent<HitoBitStreamBalanceUpdate>>? _balanceHandler;
        private readonly Action<DataEvent<HitoBitStreamEvent>>? _listenKeyExpiredHandler;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            var identifier = message.GetValue<string>(_ePath);
            if (string.Equals(identifier, "outboundAccountPosition", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitStreamPositionsUpdate>);
            if (string.Equals(identifier, "balanceUpdate", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitStreamBalanceUpdate>);
            if (string.Equals(identifier, "executionReport", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitStreamOrderUpdate>);
            if (string.Equals(identifier, "listStatus", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitStreamOrderList>);
            if (string.Equals(identifier, "listenKeyExpired", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitStreamEvent>);

            return null;
        }

        /// <inheritdoc />
        public HitoBitSpotUserDataSubscription(
            ILogger logger,
            List<string> topics,
            Action<DataEvent<HitoBitStreamOrderUpdate>>? orderHandler,
            Action<DataEvent<HitoBitStreamOrderList>>? orderListHandler,
            Action<DataEvent<HitoBitStreamPositionsUpdate>>? positionHandler,
            Action<DataEvent<HitoBitStreamBalanceUpdate>>? balanceHandler,
            Action<DataEvent<HitoBitStreamEvent>>? listenKeyExpiredHandler,
            bool auth) : base(logger, auth)
        {
            _orderHandler = orderHandler;
            _orderListHandler = orderListHandler;
            _positionHandler = positionHandler;
            _balanceHandler = balanceHandler;
            _listenKeyExpiredHandler = listenKeyExpiredHandler;
            ListenerIdentifiers = new HashSet<string>(topics);
        }

        /// <inheritdoc />
        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new HitoBitSystemQuery<HitoBitSocketQueryResponse>(new HitoBitSocketRequest
            {
                Method = "SUBSCRIBE",
                Params = ListenerIdentifiers.ToArray(),
                Id = ExchangeHelpers.NextId()
            }, false);
        }

        /// <inheritdoc />
        public override Query? GetUnsubQuery()
        {
            return new HitoBitSystemQuery<HitoBitSocketQueryResponse>(new HitoBitSocketRequest
            {
                Method = "UNSUBSCRIBE",
                Params = ListenerIdentifiers.ToArray(),
                Id = ExchangeHelpers.NextId()
            }, false);
        }

        /// <inheritdoc />
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is HitoBitCombinedStream<HitoBitStreamPositionsUpdate> positionUpdate)
                _positionHandler?.Invoke(message.As(positionUpdate.Data, positionUpdate.Stream, null, SocketUpdateType.Update));
            else if (message.Data is HitoBitCombinedStream<HitoBitStreamBalanceUpdate> balanceUpdate)
                _balanceHandler?.Invoke(message.As(balanceUpdate.Data, balanceUpdate.Stream, null, SocketUpdateType.Update));
            else if (message.Data is HitoBitCombinedStream<HitoBitStreamOrderUpdate> orderUpdate)
                _orderHandler?.Invoke(message.As(orderUpdate.Data, orderUpdate.Stream, orderUpdate.Data.Symbol, SocketUpdateType.Update));
            else if (message.Data is HitoBitCombinedStream<HitoBitStreamOrderList> orderListUpdate)
                _orderListHandler?.Invoke(message.As(orderListUpdate.Data, orderListUpdate.Stream, null, SocketUpdateType.Update));
            else if (message.Data is HitoBitCombinedStream<HitoBitStreamEvent> listenKeyExpired)
                _listenKeyExpiredHandler?.Invoke(message.As(listenKeyExpired.Data, listenKeyExpired.Stream, null, SocketUpdateType.Update));

            return new CallResult(null);
        }
    }
}
