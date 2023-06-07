using HitoBit.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Clients.SpotApi;
using CryptoExchange.Net.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc cref="IHitoBitClientGeneralApi" />
    public class HitoBitClientGeneralApi : RestApiClient, IHitoBitClientGeneralApi
    {
        #region fields 
        internal new readonly HitoBitClientOptions Options;
        private readonly HitoBitClient _baseClient;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IHitoBitClientGeneralApiBrokerage Brokerage { get; }
        /// <inheritdoc />
        public IHitoBitClientGeneralApiFutures Futures { get; }
        /// <inheritdoc />
        public IHitoBitClientGeneralApiSavings Savings { get; }
        /// <inheritdoc />
        public IHitoBitClientGeneralApiCryptoLoans CryptoLoans { get; }
        /// <inheritdoc />
        public IHitoBitClientGeneralApiMining Mining { get; }
        /// <inheritdoc />
        public IHitoBitClientGeneralApiSubAccount SubAccount { get; }
        #endregion

        #region constructor/destructor

        internal HitoBitClientGeneralApi(Log log, HitoBitClient baseClient, HitoBitClientOptions options) : base(log, options, options.SpotApiOptions)
        {
            _baseClient = baseClient;
            Options = options;

            Brokerage = new HitoBitClientGeneralApiBrokerage(this);
            Futures = new HitoBitClientGeneralApiFutures(this);
            Savings = new HitoBitClientGeneralApiSavings(this);
            CryptoLoans = new HitoBitClientGeneralApiCryptoLoans(this);
            Mining = new HitoBitClientGeneralApiMining(this);
            SubAccount = new HitoBitClientGeneralApiSubAccount(this);

            requestBodyEmptyContent = "";
            requestBodyFormat = RequestBodyFormat.FormData;
            arraySerialization = ArrayParametersSerialization.MultipleValues;
        }

        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider((HitoBitApiCredentials)credentials);

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
            if (!result && result.Error!.Code == -1021 && Options.SpotApiOptions.AutoTimestamp)
            {
                _log.Write(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
                HitoBitClientSpotApi.TimeSyncState.LastSyncTime = DateTime.MinValue;
            }
            return result;
        }


        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => _baseClient.SpotApi.ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_log, Options.SpotApiOptions.AutoTimestamp, Options.SpotApiOptions.TimestampRecalculationInterval, HitoBitClientSpotApi.TimeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => HitoBitClientSpotApi.TimeSyncState.TimeOffset;

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
