using HitoBit.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using System;

namespace HitoBit.Net.UnitTests.TestImplementations
{
    public class HitoBitRestApiClient : RestApiClient
    {
        public HitoBitRestApiClient(ILogger logger, HitoBitRestOptions options, HitoBitRestApiOptions apiOptions) : base(logger, null, "https://test.com", options, apiOptions)
        {
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode futuresType, DateTime? deliverDate = null) => $"{baseAsset.ToUpperInvariant()}{quoteAsset.ToUpperInvariant()}";
        public override TimeSpan? GetTimeOffset() => null;
        public override TimeSyncInfo GetTimeSyncInfo() => null;
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) => throw new NotImplementedException();
    }
}
