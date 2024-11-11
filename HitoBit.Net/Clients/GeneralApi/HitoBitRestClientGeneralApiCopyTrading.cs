using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot.CopyTrading;

namespace HitoBit.Net.Clients.GeneralApi
{
    internal class HitoBitRestClientGeneralApiCopyTrading : IHitoBitRestClientGeneralApiCopyTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiCopyTrading(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get User Status

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitCopyTradingUserStatus>> GetUserStatusAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/copyTrading/futures/userStatus", HitoBitExchange.RateLimiter.SpotRestUid, 20, true);
            var data = await _baseClient.SendAsync<HitoBitResult<HitoBitCopyTradingUserStatus>>(request, parameters, ct).ConfigureAwait(false);

            return data.As(data.Data.Data);
        }

        #endregion

        #region Get Lead Symbol

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitCopyTradingLeadSymbol>>> GetLeadSymbolAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalString("recvWindow", receiveWindow ?? (long)_baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/copyTrading/futures/leadSymbol", HitoBitExchange.RateLimiter.SpotRestUid, 20, true);
            var data = await _baseClient.SendAsync<HitoBitResult<IEnumerable<HitoBitCopyTradingLeadSymbol>>>(request, parameters, ct).ConfigureAwait(false);

            return data.As(data.Data.Data);
        }

        #endregion

    }
}
