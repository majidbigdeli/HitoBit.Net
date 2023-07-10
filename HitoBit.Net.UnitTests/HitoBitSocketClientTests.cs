using System;
using System.Linq;
using HitoBit.Net.Objects;
using HitoBit.Net.UnitTests.TestImplementations;
using NUnit.Framework;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using System.Threading.Tasks;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.Socket;
using Microsoft.Extensions.Logging;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Options;

namespace HitoBit.Net.UnitTests
{
    [TestFixture()]
    public class HitoBitNetTest
    {
        [TestCase()]
        public void SubscribingToKlineStream_Should_TriggerWhenKlineStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            IHitoBitStreamKlineData result = null;
            client.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync("ETHBTC", KlineInterval.OneMinute, (test) => result = test.Data);

            var data = new HitoBitCombinedStream<HitoBitStreamKlineData>()
            {
                Stream = "ethbtc@kline_1m",
                Data = new HitoBitStreamKlineData()
                {
                    Event = "TestKlineStream",
                    EventTime = new DateTime(2017, 1, 1),
                    Symbol = "ETHBTC",
                    Data = new HitoBitStreamKline()
                    {
                        TakerBuyBaseVolume = 0.1m,
                        ClosePrice = 0.2m,
                        CloseTime = new DateTime(2017, 1, 2),
                        Final = true,
                        FirstTrade = 10000000000,
                        HighPrice = 0.3m,
                        Interval = KlineInterval.OneMinute,
                        LastTrade = 2000000000000,
                        LowPrice = 0.4m,
                        OpenPrice = 0.5m,
                        TakerBuyQuoteVolume = 0.6m,
                        QuoteVolume = 0.7m,
                        OpenTime = new DateTime(2017, 1, 1),
                        Symbol = "test",
                        TradeCount = 10,
                        Volume = 0.8m
                    }
                }
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data, result, "Data"));
            Assert.IsTrue(TestHelpers.AreEqual(data.Data.Data, result.Data));
        }

        [TestCase()]
        public void SubscribingToContinuousKlineStream_Should_TriggerWhenContinuousKlineStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            IHitoBitStreamKlineData result = null;
            client.UsdFuturesApi.SubscribeToContinuousContractKlineUpdatesAsync("ETHBTC", ContractType.Perpetual, KlineInterval.OneMinute, (test) => result = test.Data);

            var data = new HitoBitCombinedStream<HitoBitStreamContinuousKlineData>()
            {
                Stream = "ethbtc_perpetual@continuousKline_1m",
                Data = new HitoBitStreamContinuousKlineData()
                {
                    Event = "TestContinuousKlineStream",
                    EventTime = new DateTime(2017, 1, 1),
                    Symbol = "ETHBTC",
                    ContractType = ContractType.Perpetual,
                    Data = new HitoBitStreamKline()
                    {
                        TakerBuyBaseVolume = 0.1m,
                        ClosePrice = 0.2m,
                        CloseTime = new DateTime(2017, 1, 2),
                        Final = true,
                        FirstTrade = 10000000000,
                        HighPrice = 0.3m,
                        Interval = KlineInterval.OneMinute,
                        LastTrade = 2000000000000,
                        LowPrice = 0.4m,
                        OpenPrice = 0.5m,
                        TakerBuyQuoteVolume = 0.6m,
                        QuoteVolume = 0.7m,
                        OpenTime = new DateTime(2017, 1, 1),
                        Symbol = "ETHBTC",
                        TradeCount = 10,
                        Volume = 0.8m
                    }
                }
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data, result, "Data"));
            Assert.IsTrue(TestHelpers.AreEqual(data.Data.Data, result.Data));
        }

        [TestCase()]
        public async Task SubscribingToSymbolTicker_Should_TriggerWhenSymbolTickerStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            IHitoBitTick result = null;
            await client.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("ETHBTC", (test) => result = test.Data);

            var data = new HitoBitCombinedStream<HitoBitStreamTick>()
            {
                Stream = "ethbtc@ticker",
                Data = new HitoBitStreamTick() { 
                    FirstTradeId = 1,
                    HighPrice = 0.7m,
                    LastTradeId = 2,
                    LowPrice = 0.8m,
                    OpenPrice = 0.9m,
                    PrevDayClosePrice = 1.0m,
                    PriceChange = 1.1m,
                    Symbol = "test",
                    Volume = 1.3m,
                    QuoteVolume = 1.4m,
                    TotalTrades = 3
                }
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data, result));
        }

        [TestCase()]
        public async Task SubscribingToAllSymbolTicker_Should_TriggerWhenAllSymbolTickerStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            IHitoBitTick[] result = null;
            await client.SpotApi.ExchangeData.SubscribeToAllTickerUpdatesAsync((test) => result = test.Data.ToArray());

            var data = new HitoBitCombinedStream<HitoBitStreamTick[]>
            {
                Data = new[]
                {
                     new HitoBitStreamTick()
                    {
                        FirstTradeId = 1,
                        HighPrice = 0.7m,
                        LastTradeId = 2,
                        LowPrice = 0.8m,
                        OpenPrice = 0.9m,
                        PrevDayClosePrice = 1.0m,
                        PriceChange = 1.1m,
                        Symbol = "test",
                        Volume = 1.3m,
                        QuoteVolume = 1.4m,
                        TotalTrades = 3
                    }
                },
                Stream = "!ticker@arr"
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data[0], result[0]));
        }

        [TestCase()]
        public async Task SubscribingToTradeStream_Should_TriggerWhenTradeStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            HitoBitStreamTrade result = null;
            await client.SpotApi.ExchangeData.SubscribeToTradeUpdatesAsync("ETHBTC", (test) => result = test.Data);

            var data = new HitoBitCombinedStream<HitoBitStreamTrade>()
            {
                Stream = "ethbtc@trade",
                Data = new HitoBitStreamTrade()
                {
                    Event = "TestTradeStream",
                    EventTime = new DateTime(2017, 1, 1),
                    Symbol = "ETHBTC",
                    BuyerIsMaker = true,
                    BuyerOrderId = 10000000000000,
                    SellerOrderId = 2000000000000,
                    Price = 1.1m,
                    Quantity = 2.2m,
                    TradeTime = new DateTime(2017, 1, 1)
                }
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data, result));
        }

        [TestCase()]
        public async Task SubscribingToUserStream_Should_TriggerWhenAccountUpdateStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            HitoBitStreamBalanceUpdate result = null;
            await client.SpotApi.Account.SubscribeToUserDataUpdatesAsync("test", null, null, null, (test) => result = test.Data);

            var data = new HitoBitCombinedStream<HitoBitStreamBalanceUpdate>
            {
                Stream = "test",
                Data = new HitoBitStreamBalanceUpdate()
                {
                    Event = "balanceUpdate",
                    EventTime = new DateTime(2017, 1, 1),
                    Asset = "BTC",
                    BalanceDelta = 1,
                    ClearTime = new DateTime(2018, 1, 1),
                }
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data, result, "ListenKey"));
        }

        [TestCase()]
        public void SubscribingToUserStream_Should_TriggerWhenOcoOrderUpdateStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            HitoBitStreamOrderList result = null;
            client.SpotApi.Account.SubscribeToUserDataUpdatesAsync("test", null, (test) => result = test.Data, null, null);

            var data = new HitoBitCombinedStream<HitoBitStreamOrderList>
            {
                Stream = "test",
                Data = new HitoBitStreamOrderList()
                {
                    Event = "listStatus",
                    EventTime = new DateTime(2017, 1, 1),
                    Symbol = "BNBUSDT",
                    ContingencyType = "OCO",
                    ListStatusType = ListStatusType.Done,
                    ListOrderStatus = ListOrderStatus.Done,
                    Id = 1,
                    ListClientOrderId = "2",
                    TransactionTime = new DateTime(2018, 1, 1),
                    Orders = new[]
                {
                    new HitoBitStreamOrderId()
                    {
                        Symbol = "BNBUSDT",
                        OrderId = 2,
                        ClientOrderId = "3"
                    },
                    new HitoBitStreamOrderId()
                    {
                        Symbol = "BNBUSDT",
                        OrderId = 3,
                        ClientOrderId = "4"
                    }
                }
                }
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data, result, "Orders", "ListenKey"));
            Assert.IsTrue(TestHelpers.AreEqual(data.Data.Orders.ToList()[0], result.Orders.ToList()[0]));
            Assert.IsTrue(TestHelpers.AreEqual(data.Data.Orders.ToList()[1], result.Orders.ToList()[1]));
        }

        [TestCase()]
        public void SubscribingToUserStream_Should_TriggerWhenOrderUpdateStreamMessageIsReceived()
        {
            // arrange
            var socket = new TestSocket();
            var client = TestHelpers.CreateSocketClient(socket);

            HitoBitStreamOrderUpdate result = null;
            client.SpotApi.Account.SubscribeToUserDataUpdatesAsync("test", (test) => result = test.Data, null, null, null);

            var data = new HitoBitCombinedStream<HitoBitStreamOrderUpdate>
            {
                Stream = "test",
                Data = new HitoBitStreamOrderUpdate()
                {
                    Event = "executionReport",
                    EventTime = new DateTime(2017, 1, 1),
                    BuyerIsMaker = true,
                    Fee = 2.2m,
                    FeeAsset = "test",
                    ExecutionType = ExecutionType.Trade,
                    I = 100000000000,
                    Id = 100000000000,
                    Price = 6.6m,
                    Quantity = 8.8m,
                    RejectReason = OrderRejectReason.AccountCannotSettle,
                    Side = OrderSide.Buy,
                    Status = OrderStatus.Filled,
                    Symbol = "test",
                    TimeInForce = TimeInForce.GoodTillCanceled,
                    TradeId = 10000000000000,
                    Type = SpotOrderType.Limit,
                    ClientOrderId = "123",
                    IcebergQuantity = 9.9m,
                    IsWorking = true,
                    OriginalClientOrderId = "456",
                    StopPrice = 10.10m
                }
            };

            // act
            socket.InvokeMessage(data);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestHelpers.AreEqual(data.Data, result, "Balances", "ListenKey"));
        }
    }
}
