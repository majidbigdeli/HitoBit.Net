using HitoBit.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Clients.SpotApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using HitoBit.Net.Objects.Options;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc cref="IHitoBitRestClientGeneralApi" />
    public class HitoBitRestClientGeneralApi : RestApiClient, IHitoBitRestClientGeneralApi
    {
        #region fields 
        /// <inheritdoc />
        public new HitoBitRestApiOptions ApiOptions => (HitoBitRestApiOptions)base.ApiOptions;
        /// <inheritdoc />
        public new HitoBitRestOptions ClientOptions => (HitoBitRestOptions)base.ClientOptions;

        private readonly HitoBitRestClient _baseClient;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiBrokerage Brokerage { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiFutures Futures { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiSavings Savings { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiLoans CryptoLoans { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiMining Mining { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiSubAccount SubAccount { get; }
        #endregion

        #region constructor/destructor

        internal HitoBitRestClientGeneralApi(ILogger logger, HttpClient? httpClient, HitoBitRestClient baseClient, HitoBitRestOptions options) 
            : base(logger, httpClient, options.Environment.SpotRestAddress, options, options.SpotOptions)
        {
            _baseClient = baseClient;

            Brokerage = new HitoBitRestClientGeneralApiBrokerage(this);
            Futures = new HitoBitRestClientGeneralApiFutures(this);
            Savings = new HitoBitRestClientGeneralApiSavings(this);
            CryptoLoans = new HitoBitRestClientGeneralApiLoans(this);
            Mining = new HitoBitRestClientGeneralApiMining(this);
            SubAccount = new HitoBitRestClientGeneralApiSubAccount(this);

            requestBodyEmptyContent = "";
            requestBodyFormat = RequestBodyFormat.FormData;
            arraySerialization = ArrayParametersSerialization.MultipleValues;
        }

        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider(credentials);

        internal Uri GetUrl(string endpoint, string api, string? version = null)
        {
            var result = BaseAddress.AppendPath(api);

            if (!string.IsNullOrEmpty(version))
                result = result.AppendPath($"v{version}");

            return new Uri(result.AppendPath(endpoint));
        }

        internal async Task<WebCallResult<T>> SendRequestInternal<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        {
            var result = await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
            if (!result && result.Error!.Code == -1021 && (ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp))
            {
                _logger.Log(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
                HitoBitRestClientSpotApi._timeSyncState.LastSyncTime = DateTime.MinValue;
            }
            return result;
        }


        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => _baseClient.SpotApi.ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, (ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp), (ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval), HitoBitRestClientSpotApi._timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => HitoBitRestClientSpotApi._timeSyncState.TimeOffset;

        /// <inheritdoc />
        protected override Error ParseErrorResponse(JToken error)
        {
            if (!error.HasValues)
                return new ServerError(error.ToString());

            if (error["msg"] == null && error["code"] == null)
                return new ServerError(error.ToString());

            if (error["msg"] != null && error["code"] == null)
                return new ServerError((string)error["msg"]!);

            return new ServerError((int)error["code"]!, (string)error["msg"]!);
        }
    }
}
