using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using HitoBit.Net.Interfaces.Clients.GeneralApi;
using HitoBit.Net.Objects.Models;
using HitoBit.Net.Objects.Models.Spot;
using HitoBit.Net.Objects.Models.Spot.Futures;
using HitoBit.Net.Objects.Models.Spot.Margin;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace HitoBit.Net.Clients.GeneralApi
{
    /// <inheritdoc />
    public class HitoBitRestClientGeneralApiFutures : IHitoBitRestClientGeneralApiFutures
    {
        private const string futuresTransferEndpoint = "futures/transfer";
        private const string futuresTransferHistoryEndpoint = "futures/transfer";
        private const string futuresBorrowEndpoint = "futures/loan/borrow";
        private const string futuresBorrowHistoryEndpoint = "futures/loan/borrow/history";
        private const string futuresRepayEndpoint = "futures/loan/repay";
        private const string futuresRepayHistoryEndpoint = "futures/loan/repay/history";
        private const string futuresWalletEndpoint = "futures/loan/wallet";
        private const string futuresInformationEndpoint = "futures/loan/configs";
        private const string futuresCalculateAdjustLevelEndpoint = "futures/loan/calcAdjustLevel";
        private const string futuresCalculateMaxAdjustAmountEndpoint = "futures/loan/calcMaxAdjustAmount";
        private const string futuresAdjustCrossCollateralEndpoint = "futures/loan/adjustCollateral";
        private const string futuresAdjustCrossCollateralHistoryEndpoint = "futures/loan/adjustCollateral/history";
        private const string futuresCrossCollateralLiquidationHistoryEndpoint = "futures/loan/liquidationHistory";

        private const string api = "sapi";
        private const string publicVersion = "1";

        private readonly HitoBitRestClientGeneralApi _baseClient;

        internal HitoBitRestClientGeneralApiFutures(HitoBitRestClientGeneralApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region New Future Account Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitTransaction>> TransferFuturesAccountAsync(string asset, decimal quantity, FuturesTransferType transferType, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "type", JsonConvert.SerializeObject(transferType, new FuturesTransferTypeConverter(false)) }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitTransaction>(_baseClient.GetUrl(futuresTransferEndpoint, api, publicVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get future account transaction history

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitSpotFuturesTransfer>>> GetFuturesTransferHistoryAsync(string asset, DateTime startTime, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "startTime", DateTimeConverter.ConvertToMilliseconds(startTime)! }
            };

            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitSpotFuturesTransfer>>(_baseClient.GetUrl(futuresTransferHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Borrow for cross-collateral

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitCrossCollateralBorrowResult>> BorrowForCrossCollateralAsync(string asset, string collateralAsset, decimal? quantity = null, decimal? collateralQuantity = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            if (quantity.HasValue == collateralQuantity.HasValue)
                throw new ArgumentException("Either quantity or collateralQuantity should be specified", nameof(quantity));

            var parameters = new Dictionary<string, object>
            {
                { "coin", asset },
                { "collateralCoin", collateralAsset }
            };

            parameters.AddOptionalParameter("amount", quantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("collateralAmount", collateralQuantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitCrossCollateralBorrowResult>(_baseClient.GetUrl(futuresBorrowEndpoint, api, publicVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        }

        #endregion

        #region Get cross collateral borrow history

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitCrossCollateralBorrowHistory>>> GetCrossCollateralBorrowHistoryAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitCrossCollateralBorrowHistory>>(_baseClient.GetUrl(futuresBorrowHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Repay for cross-collateral

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitCrossCollateralRepayResult>> RepayForCrossCollateralAsync(string asset, string collateralAsset, decimal quantity, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "coin", asset },
                { "collateralCoin", collateralAsset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitCrossCollateralRepayResult>(_baseClient.GetUrl(futuresRepayEndpoint, api, publicVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        }

        #endregion

        #region Get cross collateral repay history

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitCrossCollateralRepayHistory>>> GetCrossCollateralRepayHistoryAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("coin", asset);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitCrossCollateralRepayHistory>>(_baseClient.GetUrl(futuresRepayHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Cross collateral wallet

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitCrossCollateralWallet>> GetCrossCollateralWalletAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitCrossCollateralWallet>(_baseClient.GetUrl(futuresWalletEndpoint, api, "2"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Cross collateral information

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<HitoBitCrossCollateralInformation>>> GetCrossCollateralInformationAsync(long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<HitoBitCrossCollateralInformation>>(_baseClient.GetUrl(futuresInformationEndpoint, api, "2"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Calculate Rate After Adjust Cross-Collateral LTV

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitCrossCollateralAfterAdjust>> GetRateAfterAdjustLoanToValueAsync(string collateralAsset, string loanAsset, decimal quantity, AdjustRateDirection direction, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "collateralCoin", collateralAsset },
                { "loanCoin", loanAsset},
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "direction", JsonConvert.SerializeObject(direction, new AdjustRateDirectionConverter(false)) },
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitCrossCollateralAfterAdjust>(_baseClient.GetUrl(futuresCalculateAdjustLevelEndpoint, api, "2"), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);
        }

        #endregion

        #region Get Max Amount for Adjust Cross-Collateral LTV

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitCrossCollateralAdjustMaxAmounts>> GetMaxAmountForAdjustCrossCollateralLoanToValueAsync(string collateralAsset, string loanAsset, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "collateralCoin", collateralAsset },
                { "loanCoin", loanAsset },
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitCrossCollateralAdjustMaxAmounts>(_baseClient.GetUrl(futuresCalculateMaxAdjustAmountEndpoint, api, "2"), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);
        }

        #endregion

        #region Adjust cross collateral LTV

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitCrossCollateralAdjustLtvResult>> AdjustCrossCollateralLoanToValueAsync(string collateralAsset, string loanAsset, decimal quantity, AdjustRateDirection direction, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "collateralCoin", collateralAsset },
                { "loanCoin", loanAsset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "direction", JsonConvert.SerializeObject(direction, new AdjustRateDirectionConverter(false)) },
            };

            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitCrossCollateralAdjustLtvResult>(_baseClient.GetUrl(futuresAdjustCrossCollateralEndpoint, api, "2"), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        }

        #endregion

        #region Adjust Cross-Collateral LTV History

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitCrossCollateralAdjustLtvHistory>>> GetAdjustCrossCollateralLoanToValueHistoryAsync(string? collateralAsset = null, string? loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("loanCoin", loanAsset);
            parameters.AddOptionalParameter("collateralCoin", collateralAsset);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitCrossCollateralAdjustLtvHistory>>(_baseClient.GetUrl(futuresAdjustCrossCollateralHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion

        #region Cross-Collateral Liquidation History

        /// <inheritdoc />
        public async Task<WebCallResult<HitoBitQueryRecords<HitoBitCrossCollateralLiquidationHistory>>> GetCrossCollateralLiquidationHistoryAsync(string? collateralAsset = null, string? loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("loanCoin", loanAsset);
            parameters.AddOptionalParameter("collateralCoin", collateralAsset);
            parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<HitoBitQueryRecords<HitoBitCrossCollateralLiquidationHistory>>(_baseClient.GetUrl(futuresCrossCollateralLiquidationHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
        }

        #endregion
    }
}
