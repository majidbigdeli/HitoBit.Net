using HitoBit.Net.Clients;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Objects.Models.Futures.Socket;
using HitoBit.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HitoBit.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new HitoBitSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<HitoBitSocketClient>(client, "Subscriptions/Spot/ExchangeData", "https://api.hitobit.com", "data", stjCompare: true);
            //await tester.ValidateAsync<HitoBitStreamTrade>((client, handler) => client.SpotApi.ExchangeData.SubscribeToTradeUpdatesAsync("BTCUSDT", handler), "Trades");
            //await tester.ValidateAsync<HitoBitStreamAggregatedTrade>((client, handler) => client.SpotApi.ExchangeData.SubscribeToAggregatedTradeUpdatesAsync("BTCUSDT", handler), "AggregatedTrades");
            await tester.ValidateAsync<IHitoBitStreamKlineData>((client, handler) => client.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", Enums.KlineInterval.EightHour, handler), "Klines", ignoreProperties: new List<string> { "B" });
            await tester.ValidateAsync<IHitoBitMiniTick>((client, handler) => client.SpotApi.ExchangeData.SubscribeToMiniTickerUpdatesAsync("BTCUSDT", handler), "MiniTicker");
            await tester.ValidateAsync<HitoBitStreamBookPrice>((client, handler) => client.SpotApi.ExchangeData.SubscribeToBookTickerUpdatesAsync("BTCUSDT", handler), "BookTicker");
            await tester.ValidateAsync<IHitoBitOrderBook>((client, handler) => client.SpotApi.ExchangeData.SubscribeToPartialOrderBookUpdatesAsync("BTCUSDT", 5, 100, handler), "PartialBook");
            await tester.ValidateAsync<IHitoBitTick>((client, handler) => client.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("BTCUSDT", handler), "Ticker");
            await tester.ValidateAsync<IHitoBitTick>((client, handler) => client.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("BTCUSDT", handler), "Ticker");
        }

        [Test]
        public async Task ValidateSpotAccountSubscriptions()
        {
            var client = new HitoBitSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<HitoBitSocketClient>(client, "Subscriptions/Spot/Account", "https://api.hitobit.com", "data", stjCompare: true);
            await tester.ValidateAsync<HitoBitStreamOrderUpdate>((client, handler) => client.SpotApi.Account.SubscribeToUserDataUpdatesAsync("123", onOrderUpdateMessage: handler), "Order");
            await tester.ValidateAsync<HitoBitStreamOrderList>((client, handler) => client.SpotApi.Account.SubscribeToUserDataUpdatesAsync("123", onOcoOrderUpdateMessage: handler), "OcoOrder");
            await tester.ValidateAsync<HitoBitStreamPositionsUpdate>((client, handler) => client.SpotApi.Account.SubscribeToUserDataUpdatesAsync("123", onAccountPositionMessage: handler), "AccountPosition");
            await tester.ValidateAsync<HitoBitStreamBalanceUpdate>((client, handler) => client.SpotApi.Account.SubscribeToUserDataUpdatesAsync("123", onAccountBalanceUpdate: handler), "Balance");
        }

        [Test]
        public async Task ValidateUsdFuturesSubscriptions()
        {
            var client = new HitoBitSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<HitoBitSocketClient>(client, "Subscriptions/UsdFutures", "https://fapi.hitobit.com", "data", stjCompare: true);
            await tester.ValidateAsync<HitoBitFuturesUsdtStreamMarkPrice>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToMarkPriceUpdatesAsync("BTCUSDT", 1000, handler), "MarkPrice");
            await tester.ValidateAsync<IHitoBitStreamKlineData>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", Enums.KlineInterval.OneMonth, handler), "Klines", ignoreProperties: new List<string> { "B" });
            await tester.ValidateAsync<HitoBitStreamContinuousKlineData>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToContinuousContractKlineUpdatesAsync("BTCUSDT", Enums.ContractType.Perpetual, Enums.KlineInterval.OneMonth, handler), "ContKlines", ignoreProperties: new List<string> { "B" });
            await tester.ValidateAsync<IHitoBitMiniTick>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToMiniTickerUpdatesAsync("BTCUSDT", handler), "MiniTicker");
            await tester.ValidateAsync<IHitoBit24HPrice>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToTickerUpdatesAsync("BTCUSDT", handler), "Ticker");
            await tester.ValidateAsync<HitoBitFuturesStreamCompositeIndex>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToCompositeIndexUpdatesAsync("BTCUSDT", handler), "CompositIndex");
            await tester.ValidateAsync<HitoBitStreamAggregatedTrade>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToAggregatedTradeUpdatesAsync("BTCUSDT", handler), "AggTrades");
            await tester.ValidateAsync<HitoBitFuturesStreamBookPrice>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToBookTickerUpdatesAsync("BTCUSDT", handler), "BookTicker");
            await tester.ValidateAsync<HitoBitFuturesStreamLiquidation>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToLiquidationUpdatesAsync("BTCUSDT", handler), "Liquidations", "data.o", ignoreProperties: new List<string> { "e", "E" });
            await tester.ValidateAsync<IHitoBitFuturesEventOrderBook>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToPartialOrderBookUpdatesAsync("BTCUSDT", 5, 100, handler), "PartialBook");
            await tester.ValidateAsync<IHitoBitFuturesEventOrderBook>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToOrderBookUpdatesAsync("BTCUSDT", 100, handler), "Book");
            await tester.ValidateAsync<HitoBitFuturesStreamSymbolUpdate>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToSymbolUpdatesAsync(handler), "SymbolUpdates");
            await tester.ValidateAsync<IEnumerable<HitoBitFuturesStreamAssetIndexUpdate>>((client, handler) => client.UsdFuturesApi.ExchangeData.SubscribeToAssetIndexUpdatesAsync(handler), "AssetIndex");
            await tester.ValidateAsync<HitoBitFuturesStreamConfigUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onLeverageUpdate: handler), "Leverage");
            await tester.ValidateAsync<HitoBitFuturesStreamConfigUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onLeverageUpdate: handler), "MultiAssetMode");
            await tester.ValidateAsync<HitoBitFuturesStreamMarginUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onMarginUpdate: handler), "MarginUpdate");
            await tester.ValidateAsync<HitoBitFuturesStreamAccountUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onAccountUpdate: handler), "AccountUpdate");
            await tester.ValidateAsync<HitoBitFuturesStreamOrderUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onOrderUpdate : handler), "OrderUpdate", ignoreProperties: new List<string> { "si", "ss" });
            await tester.ValidateAsync<HitoBitFuturesStreamTradeUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onTradeUpdate: handler), "TradeUpdate");
            await tester.ValidateAsync<HitoBitStrategyUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onStrategyUpdate : handler), "StrategyUpdate");
            await tester.ValidateAsync<HitoBitConditionOrderTriggerRejectUpdate>((client, handler) => client.UsdFuturesApi.Account.SubscribeToUserDataUpdatesAsync("123", onConditionalOrderTriggerRejectUpdate: handler), "ConditionalTrigger");
        }
    }
}
