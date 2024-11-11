using HitoBit.Net;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Objects.Spot;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Serverless.FunctionApp.Startup))]
namespace Serverless.FunctionApp
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			// HitoBit Setup
			builder.Services.AddSingleton<IHitoBitSocketClient>(x => new HitoBitSocketClient(new HitoBitSocketClientOptions
			{
				ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(Environment.GetEnvironmentVariable("HitoBitApiKey"), Environment.GetEnvironmentVariable("HitoBitSecretKey"))
			}));

			builder.Services.AddTransient<IHitoBitClient>(x => new HitoBitClient(new HitoBitClientOptions
			{
				ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(Environment.GetEnvironmentVariable("HitoBitApiKey"), Environment.GetEnvironmentVariable("HitoBitSecretKey"))
			}));

			builder.Services.AddSingleton<IHitoBitDataProvider, HitoBitDataProvider>();

			// additional setup
			string insKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
			builder.Services.AddApplicationInsightsTelemetry(insKey);

		}
	}
}
