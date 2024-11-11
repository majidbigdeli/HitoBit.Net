using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace Blazor.DataProvider
{
    public class HitoBitDataProvider
    {
        private IHitoBitRestClient _client;
        private IHitoBitSocketClient _socketClient;

        public HitoBitDataProvider(IHitoBitRestClient client, IHitoBitSocketClient socketClient)
        {
            _client = client;
            _socketClient = socketClient;
        }

        public Task<WebCallResult<IEnumerable<IHitoBitTick>>> Get24HPrices()
        {
            return _client.SpotApi.ExchangeData.GetTickersAsync();
        }

        public Task<CallResult<UpdateSubscription>> SubscribeTickerUpdates(Action<DataEvent<IEnumerable<IHitoBitTick>>> tickHandler)
        {
            return _socketClient.SpotApi.ExchangeData.SubscribeToAllTickerUpdatesAsync(tickHandler);
        }

        public async Task Unsubscribe(UpdateSubscription subscription)
        {
            await _socketClient.UnsubscribeAsync(subscription);
        }
    }
}
