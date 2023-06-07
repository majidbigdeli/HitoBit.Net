using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.Socket;
using HitoBit.Net.UnitTests.TestImplementations;
using Newtonsoft.Json;
using NUnit.Framework;

namespace HitoBit.Net.UnitTests
{
    internal class JsonSocketTests
    {
        [Test]
        public async Task ValidatAggregatedTradeUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamAggregatedTrade>(@"JsonResponses/Spot/Socket/AggregatedTradeUpdate.txt");
        }

        [Test]
        public async Task ValidateTradeUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamTrade>(@"JsonResponses/Spot/Socket/TradeUpdate.txt");
        }

        [Test]
        public async Task ValidateKlineUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamKlineData>(@"JsonResponses/Spot/Socket/KlineUpdate.txt", new List<string> { "B" });
        }

        [Test]
        public async Task ValidateMiniTickUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamMiniTick>(@"JsonResponses/Spot/Socket/MiniTickUpdate.txt", new List<string> { "B" });
        }

        [Test]
        public async Task ValidateBookPriceUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamBookPrice>(@"JsonResponses/Spot/Socket/BookPriceUpdate.txt", new List<string> { "B" });
        }

        [Test]
        public async Task ValidateTickerUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamTick>(@"JsonResponses/Spot/Socket/TickerUpdate.txt", new List<string> { "B" });
        }

        [Test]
        public async Task ValidateRollingTickerUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamRollingWindowTick>(@"JsonResponses/Spot/Socket/RollingTickerUpdate.txt", new List<string> { "B" });
        }

        [Test]
        public async Task ValidateUserUpdateStreamJson()
        {
            await TestFileToObject<HitoBitStreamOrderUpdate>(@"JsonResponses/Spot/Socket/UserUpdate1.txt", new List<string> { "M" });
            await TestFileToObject<HitoBitStreamOrderList>(@"JsonResponses/Spot/Socket/UserUpdate2.txt", new List<string> { "B" });
            await TestFileToObject<HitoBitStreamPositionsUpdate>(@"JsonResponses/Spot/Socket/UserUpdate3.txt", new List<string> { "B" });
            await TestFileToObject<HitoBitStreamBalanceUpdate>(@"JsonResponses/Spot/Socket/UserUpdate4.txt", new List<string> { "B" });
        }

        private static async Task TestFileToObject<T>(string filePath, List<string> ignoreProperties = null)
        {
            var listener = new EnumValueTraceListener();
            Trace.Listeners.Add(listener);
            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string json;
            try
            {
                var file = File.OpenRead(Path.Combine(path, filePath));
                using var reader = new StreamReader(file);
                json = await reader.ReadToEndAsync();
            }
            catch (FileNotFoundException)
            {
                throw;
            }

            var result = JsonConvert.DeserializeObject<T>(json);
            JsonToObjectComparer<IHitoBitSocketClient>.ProcessData("", result, json, ignoreProperties: new Dictionary<string, List<string>>
            {
                { "", ignoreProperties ?? new List<string>() }
            });
            Trace.Listeners.Remove(listener);
        }
    }

    internal class EnumValueTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }

        public override void WriteLine(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }
    }
}
