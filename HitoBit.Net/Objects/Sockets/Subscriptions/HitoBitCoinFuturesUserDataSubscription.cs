using HitoBit.Net.Objects.Internal;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Futures.Socket;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.Objects.Sockets
{
    /// <inheritdoc />
    internal class HitoBitCoinFuturesUserDataSubscription : Subscription<HitoBitSocketQueryResponse, HitoBitSocketQueryResponse>
    {
        private static readonly MessagePath _ePath = MessagePath.Get().Property("data").Property("e");

        /// <inheritdoc />
        public override HashSet<string> ListenerIdentifiers { get; set; }

        private readonly Action<DataEvent<HitoBitFuturesStreamOrderUpdate>>? _orderHandler;
        private readonly Action<DataEvent<HitoBitFuturesStreamConfigUpdate>>? _configHandler;
        private readonly Action<DataEvent<HitoBitFuturesStreamMarginUpdate>>? _marginHandler;
        private readonly Action<DataEvent<HitoBitFuturesStreamAccountUpdate>>? _accountHandler;
        private readonly Action<DataEvent<HitoBitStreamEvent>>? _listenkeyHandler;
        private readonly Action<DataEvent<HitoBitStrategyUpdate>>? _strategyHandler;
        private readonly Action<DataEvent<HitoBitGridUpdate>>? _gridHandler;

        /// <inheritdoc />
        public override Type? GetMessageType(IMessageAccessor message)
        {
            var identifier = message.GetValue<string>(_ePath);
            if (string.Equals(identifier, "ACCOUNT_CONFIG_UPDATE", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitFuturesStreamConfigUpdate>);
            if (string.Equals(identifier, "MARGIN_CALL", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitFuturesStreamMarginUpdate>);
            if (string.Equals(identifier, "ACCOUNT_UPDATE", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitFuturesStreamAccountUpdate>);
            if (string.Equals(identifier, "ORDER_TRADE_UPDATE", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitFuturesStreamOrderUpdate>);
            if (string.Equals(identifier, "listenKeyExpired", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitStreamEvent>);
            if (string.Equals(identifier, "STRATEGY_UPDATE", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitStrategyUpdate>);
            if (string.Equals(identifier, "GRID_UPDATE", StringComparison.Ordinal))
                return typeof(HitoBitCombinedStream<HitoBitGridUpdate>);

            return null;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="topics"></param>
        /// <param name="orderHandler"></param>
        /// <param name="configHandler"></param>
        /// <param name="marginHandler"></param>
        /// <param name="accountHandler"></param>
        /// <param name="listenkeyHandler"></param>
        /// <param name="strategyHandler"></param>
        /// <param name="gridHandler"></param>
        public HitoBitCoinFuturesUserDataSubscription(
            ILogger logger,
            List<string> topics,
            Action<DataEvent<HitoBitFuturesStreamOrderUpdate>>? orderHandler,
            Action<DataEvent<HitoBitFuturesStreamConfigUpdate>>? configHandler,
            Action<DataEvent<HitoBitFuturesStreamMarginUpdate>>? marginHandler,
            Action<DataEvent<HitoBitFuturesStreamAccountUpdate>>? accountHandler,
            Action<DataEvent<HitoBitStreamEvent>>? listenkeyHandler,
            Action<DataEvent<HitoBitStrategyUpdate>>? strategyHandler,
            Action<DataEvent<HitoBitGridUpdate>>? gridHandler) : base(logger, false)
        {
            _orderHandler = orderHandler;
            _configHandler = configHandler;
            _marginHandler = marginHandler;
            _accountHandler = accountHandler;
            _listenkeyHandler = listenkeyHandler;
            _strategyHandler = strategyHandler;
            _gridHandler = gridHandler;
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
            if (message.Data is HitoBitCombinedStream<HitoBitFuturesStreamConfigUpdate> configUpdate)
            {
                configUpdate.Data.ListenKey = configUpdate.Stream;
                _configHandler?.Invoke(message.As(configUpdate.Data, configUpdate.Stream, null, SocketUpdateType.Update));
            }
            else if (message.Data is HitoBitCombinedStream<HitoBitFuturesStreamMarginUpdate> marginUpdate)
            {
                marginUpdate.Data.ListenKey = marginUpdate.Stream;
                _marginHandler?.Invoke(message.As(marginUpdate.Data, marginUpdate.Stream, null, SocketUpdateType.Update));
            }
            else if (message.Data is HitoBitCombinedStream<HitoBitFuturesStreamAccountUpdate> accountUpdate)
            {
                accountUpdate.Data.ListenKey = accountUpdate.Stream;
                _accountHandler?.Invoke(message.As(accountUpdate.Data, accountUpdate.Stream, null, SocketUpdateType.Update));
            }
            else if (message.Data is HitoBitCombinedStream<HitoBitFuturesStreamOrderUpdate> orderUpdate)
            {
                orderUpdate.Data.ListenKey = orderUpdate.Stream;
                _orderHandler?.Invoke(message.As(orderUpdate.Data, orderUpdate.Stream, orderUpdate.Data.UpdateData.Symbol, SocketUpdateType.Update));
            }
            else if (message.Data is HitoBitCombinedStream<HitoBitStreamEvent> listenKeyUpdate)
            {
                _listenkeyHandler?.Invoke(message.As(listenKeyUpdate.Data, listenKeyUpdate.Stream, null, SocketUpdateType.Update));
            }
            else if (message.Data is HitoBitCombinedStream<HitoBitStrategyUpdate> strategyUpdate)
            {
                _strategyHandler?.Invoke(message.As(strategyUpdate.Data, strategyUpdate.Stream, null, SocketUpdateType.Update));
            }
            else if (message.Data is HitoBitCombinedStream<HitoBitGridUpdate> gridUpdate)
            {
                _gridHandler?.Invoke(message.As(gridUpdate.Data, gridUpdate.Stream, null, SocketUpdateType.Update));
            }

            return new CallResult(null);
        }
    }
}
