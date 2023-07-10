using HitoBit.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;

namespace HitoBit.Net.UnitTests.TestImplementations
{
    public class HitoBitRestApiClient : RestApiClient
    {
        public HitoBitRestApiClient(ILogger logger, HitoBitRestOptions options, HitoBitRestApiOptions apiOptions) : base(logger, null, "https://test.com", options, apiOptions)
        {
        }

        public override TimeSpan? GetTimeOffset() => null;
        public override TimeSyncInfo GetTimeSyncInfo() => null;
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) => throw new NotImplementedException();
    }
}
