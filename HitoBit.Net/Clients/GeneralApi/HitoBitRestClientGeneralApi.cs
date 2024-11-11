using HitoBit.Net.Objects;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Clients.SpotApi;
using HitoBit.Net.Objects.Options;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc cref="IHitoBitRestClientGeneralApi" />
    internal class HitoBitRestClientGeneralApi : RestApiClient, IHitoBitRestClientGeneralApi
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
        public IHitoBitRestClientGeneralApiLoans CryptoLoans { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiAutoInvest AutoInvest { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiMining Mining { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiSubAccount SubAccount { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiStaking Staking { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiSimpleEarn SimpleEarn { get; }
        /// <inheritdoc />
        public IHitoBitRestClientGeneralApiCopyTrading CopyTrading { get; }
        #endregion

        #region constructor/destructor

        internal HitoBitRestClientGeneralApi(ILogger logger, HttpClient? httpClient, HitoBitRestClient baseClient, HitoBitRestOptions options) 
            : base(logger, httpClient, options.Environment.SpotRestAddress, options, options.SpotOptions)
        {
            _baseClient = baseClient;

            Brokerage = new HitoBitRestClientGeneralApiBrokerage(this);
            Futures = new HitoBitRestClientGeneralApiFutures(this);
            CryptoLoans = new HitoBitRestClientGeneralApiLoans(this);
            AutoInvest = new HitoBitRestClientGeneralApiAutoInvest(this);
            Mining = new HitoBitRestClientGeneralApiMining(this);
            SubAccount = new HitoBitRestClientGeneralApiSubAccount(this);
            Staking = new HitoBitRestClientGeneralApiStaking(this);
            SimpleEarn = new HitoBitRestClientGeneralApiSimpleEarn(this);
            CopyTrading = new HitoBitRestClientGeneralApiCopyTrading(this);

            RequestBodyEmptyContent = "";
            RequestBodyFormat = RequestBodyFormat.FormData;
            ArraySerialization = ArrayParametersSerialization.MultipleValues;
        }

        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new HitoBitAuthenticationProvider(credentials);

        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor();

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
                => HitoBitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverTime);

        internal Uri GetUrl(string endpoint) => new Uri(BaseAddress.AppendPath(endpoint));

        internal Uri GetUrl(string endpoint, string api, string? version = null)
        {
            var result = BaseAddress.AppendPath(api);

            if (!string.IsNullOrEmpty(version))
                result = result.AppendPath($"v{version}");

            return new Uri(result.AppendPath(endpoint));
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result && result.Error!.Code == -1021 && (ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp))
            {
                _logger.Log(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
                HitoBitRestClientSpotApi._timeSyncState.LastSyncTime = DateTime.MinValue;
            }
            return result;
        }

        internal async Task<WebCallResult<T>> SendRequestInternal<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, IRateLimitGate? gate = null) where T : class
        {
            var result = await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, null, postPosition, arraySerialization, weight, gate: gate).ConfigureAwait(false);
            if (!result && result.Error!.Code == -1021 && (ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp))
            {
                _logger.Log(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
                HitoBitRestClientSpotApi._timeSyncState.LastSyncTime = DateTime.MinValue;
            }
            return result;
        }

        internal async Task<WebCallResult> SendRequestInternal(Uri uri, HttpMethod method, CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null, int weight = 1, IRateLimitGate? gate = null)
        {
            var result = await SendRequestAsync(uri, method, cancellationToken, parameters, signed, null, postPosition, arraySerialization, weight, gate: gate).ConfigureAwait(false);
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
        protected override Error ParseErrorResponse(int httpStatusCode, IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders, IMessageAccessor accessor)
        {
            if (!accessor.IsJson)
                return new ServerError(accessor.GetOriginalString());

            var code = accessor.GetValue<int?>(MessagePath.Get().Property("code"));
            var msg = accessor.GetValue<string>(MessagePath.Get().Property("msg"));
            if (msg == null)
                return new ServerError(accessor.GetOriginalString());

            if (code == null)
                return new ServerError(msg);

            return new ServerError(code.Value, msg);
        }

        /// <inheritdoc />
        protected override ServerRateLimitError ParseRateLimitResponse(int httpStatusCode, IEnumerable<KeyValuePair<string, IEnumerable<string>>> responseHeaders, IMessageAccessor accessor)
        {
            var error = GetRateLimitError(accessor);
            var retryAfterHeader = responseHeaders.SingleOrDefault(r => r.Key.Equals("Retry-After", StringComparison.InvariantCultureIgnoreCase));
            if (retryAfterHeader.Value?.Any() != true)
                return error;

            var value = retryAfterHeader.Value.First();
            if (!int.TryParse(value, out var seconds))
                return error;

            if (seconds == 0)
            {
                var now = DateTime.UtcNow;
                seconds = (int)(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc).AddMinutes(1) - now).TotalSeconds + 1;
            }

            error.RetryAfter = DateTime.UtcNow.AddSeconds(seconds);
            return error;
        }

        private HitoBitRateLimitError GetRateLimitError(IMessageAccessor accessor)
        {
            if (!accessor.IsJson)
                return new HitoBitRateLimitError(accessor.GetOriginalString());

            var code = accessor.GetValue<int?>(MessagePath.Get().Property("code"));
            var msg = accessor.GetValue<string>(MessagePath.Get().Property("msg"));
            if (msg == null)
                return new HitoBitRateLimitError(accessor.GetOriginalString());

            if (code == null)
                return new HitoBitRateLimitError(msg);

            return new HitoBitRateLimitError(code.Value, msg, null);
        }
    }
}
