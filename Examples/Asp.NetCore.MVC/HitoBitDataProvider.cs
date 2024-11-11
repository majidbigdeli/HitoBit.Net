using System;
using System.Threading.Tasks;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Hosting;

namespace Asp.Net
{
    public interface IHitoBitDataProvider 
    {
        IHitoBitStreamKlineData LastKline { get; }
        Action<IHitoBitStreamKlineData> OnKlineData { get; set; }

        Task Start();
        Task Stop();
    }

    public class HitoBitDataProvider: IHitoBitDataProvider
    {
        private IHitoBitSocketClient _socketClient;
        private UpdateSubscription _subscription;

        public IHitoBitStreamKlineData LastKline { get; private set; }
        public Action<IHitoBitStreamKlineData> OnKlineData { get; set; }
       
        public HitoBitDataProvider(IHitoBitSocketClient socketClient)
        {
            _socketClient = socketClient;

            Start().Wait(); // Probably want to do this in some initialization step at application startup
        }

        public async Task Start()
        {
            var subResult = await _socketClient.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", KlineInterval.FifteenMinutes, data =>
            {
                LastKline = data.Data;
                OnKlineData?.Invoke(data.Data);
            });
            if (subResult.Success)            
                _subscription = subResult.Data;            
        }

        public async Task Stop()
        {
            await _socketClient.UnsubscribeAsync(_subscription);
        }
    }
}
