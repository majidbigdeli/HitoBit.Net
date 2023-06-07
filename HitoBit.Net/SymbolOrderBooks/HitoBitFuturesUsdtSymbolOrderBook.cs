using System;
using System.Threading;
using System.Threading.Tasks;
using HitoBit.Net.Clients;
using HitoBit.Net.Interfaces;
using HitoBit.Net.Interfaces.Clients;
using HitoBit.Net.Objects;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;

namespace HitoBit.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class HitoBitFuturesUsdtSymbolOrderBook : SymbolOrderBook
    {
        private readonly IHitoBitClient _restClient;
        private readonly IHitoBitSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;
        private readonly int? _limit;
        private readonly int? _updateInterval;
        private readonly bool _restOwner;
        private readonly bool _socketOwner;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="symbol">The symbol of the order book</param>
        /// <param name="options">The options for the order book</param>
        public HitoBitFuturesUsdtSymbolOrderBook(string symbol, HitoBitOrderBookOptions? options = null) : base("HitoBit", symbol, options ?? new HitoBitOrderBookOptions())
        {
            _limit = options?.Limit;
            _updateInterval = options?.UpdateInterval;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _restClient = options?.RestClient ?? new HitoBitClient();
            _socketClient = options?.SocketClient ?? new HitoBitSocketClient();
            _restOwner = options?.RestClient == null;
            _socketOwner = options?.SocketClient == null;

            sequencesAreConsecutive = options?.Limit == null;
            strictLevels = false;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            CallResult<UpdateSubscription> subResult;
            if (_limit == null)
                subResult = await _socketClient.UsdFuturesStreams.SubscribeToOrderBookUpdatesAsync(Symbol, _updateInterval, HandleUpdate).ConfigureAwait(false);
            else
                subResult = await _socketClient.UsdFuturesStreams.SubscribeToPartialOrderBookUpdatesAsync(Symbol, _limit.Value, _updateInterval, HandleUpdate).ConfigureAwait(false);

            if (!subResult)
                return new CallResult<UpdateSubscription>(subResult.Error!);

            if (ct.IsCancellationRequested)
            {
                await subResult.Data.CloseAsync().ConfigureAwait(false);
                return subResult.AsError<UpdateSubscription>(new CancellationRequestedError());
            }

            Status = OrderBookStatus.Syncing;
            if (_limit == null)
            {
                var bookResult = await _restClient.UsdFuturesApi.ExchangeData.GetOrderBookAsync(Symbol, _limit ?? 1000).ConfigureAwait(false);
                if (!bookResult)
                {
                    await _socketClient.UnsubscribeAsync(subResult.Data).ConfigureAwait(false);
                    return new CallResult<UpdateSubscription>(bookResult.Error!);
                }

                SetInitialOrderBook(bookResult.Data.LastUpdateId, bookResult.Data.Bids, bookResult.Data.Asks);
            }
            else
            {
                var setResult = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
                return setResult ? subResult : new CallResult<UpdateSubscription>(setResult.Error!);
            }

            return new CallResult<UpdateSubscription>(subResult.Data);
        }

        private void HandleUpdate(DataEvent<IHitoBitFuturesEventOrderBook> data)
        {
            if (_limit == null)
            {
                UpdateOrderBook(data.Data.FirstUpdateId ?? 0, data.Data.LastUpdateId, data.Data.Bids, data.Data.Asks);
            }
            else
            {
                SetInitialOrderBook(data.Data.LastUpdateId, data.Data.Bids, data.Data.Asks);
            }
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            if (_limit != null)
                return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);

            var bookResult = await _restClient.UsdFuturesApi.ExchangeData.GetOrderBookAsync(Symbol, _limit ?? 1000).ConfigureAwait(false);
            if (!bookResult)
                return new CallResult<bool>(bookResult.Error!);

            SetInitialOrderBook(bookResult.Data.LastUpdateId, bookResult.Data.Bids, bookResult.Data.Asks);
            return new CallResult<bool>(true);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_restOwner)
                _restClient?.Dispose();
            if (_socketOwner)
                _socketClient?.Dispose();

            base.Dispose(disposing);
        }
    }
}
