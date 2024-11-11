using System;
using System.Net.Http;
using System.Threading.Tasks;
using HitoBit.Net;
using Blazor.DataProvider;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CryptoExchange.Net.Authentication;

namespace Blazor.ClientSide
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHitoBit(restOptions =>
            {
            }, socketOptions =>
            {
            });

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<HitoBitDataProvider>();

            await builder.Build().RunAsync();
        }
    }
}
