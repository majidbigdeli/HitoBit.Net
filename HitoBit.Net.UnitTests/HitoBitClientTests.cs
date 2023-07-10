using HitoBit.Net.Clients;
using HitoBit.Net.Clients.SpotApi;
using HitoBit.Net.Objects.Options;
using HitoBit.Net.UnitTests.TestImplementations;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Requests;
using CryptoExchange.Net.Sockets;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.UnitTests
{
    [TestFixture()]
    public class HitoBitClientTests
    {
        [TestCase(1508837063996)]
        [TestCase(1507156891385)]
        public async Task GetServerTime_Should_RespondWithServerTimeDateTime(long milisecondsTime)
        {
            // arrange
            DateTime expected = new DateTime(1970, 1, 1).AddMilliseconds(milisecondsTime);
            var time = new HitoBitCheckTime() { ServerTime = expected };
            var client = TestHelpers.CreateResponseClient(JsonConvert.SerializeObject(time));

            // act
            var result = await client.SpotApi.ExchangeData.GetServerTimeAsync();

            // assert
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(expected, result.Data);
        }
       
        [TestCase]
        public async Task StartUserStream_Should_RespondWithListenKey()
        {
            // arrange
            var key = new HitoBitListenKey()
            {
                ListenKey = "123"
            };

            var client = TestHelpers.CreateResponseClient(key, options =>
            {
                options.ApiCredentials = new ApiCredentials("Test", "Test");
                options.SpotOptions.AutoTimestamp = false;
            });

            // act
            var result = await client.SpotApi.Account.StartUserStreamAsync();

            // assert
            Assert.IsTrue(result.Success);
            Assert.IsTrue(key.ListenKey == result.Data);
        }

        [TestCase]
        public async Task KeepAliveUserStream_Should_Respond()
        {
            // arrange
            var client = TestHelpers.CreateResponseClient("{}", options =>
            {
                options.ApiCredentials = new ApiCredentials("Test", "Test");
                options.SpotOptions.AutoTimestamp = false;
            });

            // act
            var result = await client.SpotApi.Account.KeepAliveUserStreamAsync("test");

            // assert
            Assert.IsTrue(result.Success);
        }

        [TestCase]
        public async Task StopUserStream_Should_Respond()
        {
            // arrange
            var client = TestHelpers.CreateResponseClient("{}", options => { options.ApiCredentials = new ApiCredentials("Test", "Test"); options.SpotOptions.AutoTimestamp = false; });

            // act
            var result = await client.SpotApi.Account.StopUserStreamAsync("test");

            // assert
            Assert.IsTrue(result.Success);
        }

        [TestCase()]
        public async Task EnablingAutoTimestamp_Should_CallServerTime()
        {
            // arrange
            var client = TestHelpers.CreateResponseClient("{}", options =>
            {
                options.ApiCredentials = new ApiCredentials("Test", "Test");
                options.SpotOptions.AutoTimestamp = true;
            });

            // act
            try
            {
                await client.SpotApi.Trading.GetOpenOrdersAsync();
            }
            catch (Exception)
            {
                // Exception is thrown because stream is being read twice, doesn't happen normally
            }


            // assert
            Mock.Get(client.SpotApi.RequestFactory).Verify(f => f.Create(It.IsAny<HttpMethod>(), It.Is<Uri>((uri) => uri.ToString().Contains("/time")), It.IsAny<int>()), Times.Exactly(2));
        }

        [TestCase()]
        public async Task ReceivingHitoBitError_Should_ReturnHitoBitErrorAndNotSuccess()
        {
            // arrange
            var client = TestHelpers.CreateClient();
            TestHelpers.SetErrorWithResponse(client, "{\"msg\": \"Error!\", \"code\": 123}", HttpStatusCode.BadRequest);

            // act
            var result = await client.SpotApi.ExchangeData.GetServerTimeAsync();

            // assert
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.IsTrue(result.Error.Code == 123);
            Assert.IsTrue(result.Error.Message == "Error!");
        }

        [Test]
        public void ProvidingApiCredentials_Should_SaveApiCredentials()
        {
            // arrange
            // act
            var authProvider = new HitoBitAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));

            // assert
            Assert.AreEqual(authProvider.GetApiKey(), "TestKey");
        }

        [Test]
        public void AddingAuthToRequest_Should_AddApiKeyHeader()
        {
            // arrange
            var authProvider = new HitoBitAuthenticationProvider(new ApiCredentials("TestKey", "TestSecret"));
            var client = new HttpClient();
            var request = new Request(new HttpRequestMessage(HttpMethod.Get, "https://test.test-api.com"), client, 1);

            // act
            var headers = new Dictionary<string, string>();
            authProvider.AuthenticateRequest(new HitoBitRestApiClient(new TraceLogger(), new HitoBitRestOptions(), new HitoBitRestOptions().SpotOptions), request.Uri, HttpMethod.Get, new Dictionary<string, object>(), true, ArrayParametersSerialization.MultipleValues,
                HttpMethodParameterPosition.InUri, out var uriParameters, out var bodyParameters, out headers);

            // assert
            Assert.IsTrue(headers.First().Key == "X-MBX-APIKEY" && headers.First().Value == "TestKey");
        }       

        [TestCase("BTCUSDT", true)]
        [TestCase("NANOUSDT", true)]
        [TestCase("NANOAUSDTA", true)]
        [TestCase("NANOBTC", true)]
        [TestCase("ETHBTC", true)]
        [TestCase("BEETC", true)]
        [TestCase("EETC", false)]
        [TestCase("KP3RBNB", true)]
        [TestCase("BTC-USDT", false)]
        [TestCase("BTC-USD", false)]
        public void CheckValidHitoBitSymbol(string symbol, bool isValid)
        {
            if (isValid)
                Assert.DoesNotThrow(symbol.ValidateHitoBitSymbol);
            else
                Assert.Throws(typeof(ArgumentException), symbol.ValidateHitoBitSymbol);
        }

        [Test]
        public void CheckRestInterfaces()
        {
            var assembly = Assembly.GetAssembly(typeof(HitoBitRestClient));
            var ignore = new string[] { "IHitoBitClientUsdFuturesApi", "IHitoBitClientCoinFuturesApi", "IHitoBitClientSpotApi" };
            var clientInterfaces = assembly.GetTypes().Where(t => t.Name.StartsWith("IHitoBitClient") && !ignore.Contains(t.Name));
            
            foreach(var clientInterface in clientInterfaces)
            {
                var implementation = assembly.GetTypes().Single(t => t.IsAssignableTo(clientInterface) && t != clientInterface);
                int methods = 0;
                foreach (var method in implementation.GetMethods().Where(m => m.ReturnType.IsAssignableTo(typeof(Task))))
                {
                    var interfaceMethod = clientInterface.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray());
                    Assert.NotNull(interfaceMethod, $"Missing interface for method {method.Name} in {implementation.Name} implementing interface {clientInterface.Name}");
                    methods++;
                }
                Debug.WriteLine($"{clientInterface.Name} {methods} methods validated");
            }
        }

        [Test]
        public void CheckSocketInterfaces()
        {
            var assembly = Assembly.GetAssembly(typeof(HitoBitSocketClientSpotApi));
            var clientInterfaces = assembly.GetTypes().Where(t => t.Name.StartsWith("IHitoBitSocketClient"));

            foreach (var clientInterface in clientInterfaces)
            {
                var implementation = assembly.GetTypes().Single(t => t.IsAssignableTo(clientInterface) && t != clientInterface);
                int methods = 0;
                foreach (var method in implementation.GetMethods().Where(m => m.ReturnType.IsAssignableTo(typeof(Task<CallResult<UpdateSubscription>>))))
                {
                    var interfaceMethod = clientInterface.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray());
                    Assert.NotNull(interfaceMethod, $"Missing interface for method {method.Name} in {implementation.Name} implementing interface {clientInterface.GetType().Name}");
                    methods++;
                }
                Debug.WriteLine($"{clientInterface.Name} {methods} methods validated");
            }
        }
    }
}
