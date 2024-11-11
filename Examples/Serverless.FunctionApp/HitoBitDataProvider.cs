using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Objects.Spot.MarketStream;
using CryptoExchange.Net.Sockets;
using System;
using System.Threading.Tasks;

namespace Serverless.FunctionApp
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
        private IHitoBitClient _hitobitClient;
        private UpdateSubscription _subscription;

        public IHitoBitStreamKlineData LastKline { get; private set; }
        public Action<IHitoBitStreamKlineData> OnKlineData { get; set; }
       
        public HitoBitDataProvider(IHitoBitSocketClient socketClient, IHitoBitClient hitobitClient)
        {
            _socketClient = socketClient;
            _hitobitClient = hitobitClient;
            Start().Wait(); 
        }

        public async Task Start()
        {
            var subResult = await _socketClient.Spot.SubscribeToKlineUpdatesAsync("BTCUSDT", KlineInterval.FifteenMinutes, data =>
            {
                LastKline = data;
                OnKlineData?.Invoke(data);
            });
            if (subResult.Success)            
                _subscription = subResult.Data;            
        }

        public async Task Stop()
        {
            await _socketClient.Unsubscribe(_subscription);
        }
    }
}
