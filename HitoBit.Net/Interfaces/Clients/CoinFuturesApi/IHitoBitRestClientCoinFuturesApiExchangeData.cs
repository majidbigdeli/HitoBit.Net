﻿using HitoBit.Net.Enums;
using HitoBit.Net.Objects.Models.Futures;
using HitoBit.Net.Objects.Models.Spot;

namespace HitoBit.Net.Interfaces.Clients.CoinFuturesApi
{
    /// <summary>
    /// HitoBit COIN-M futures exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IHitoBitRestClientCoinFuturesApiExchangeData
    {
        /// <summary>
        /// Pings the HitoBit Futures API
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#test-connectivity" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if successful ping, false if no response</returns>
        Task<WebCallResult<long>> PingAsync(CancellationToken ct = default);

        /// <summary>
        /// Requests the server for the local time. This function also determines the offset between server and local time and uses this for subsequent API calls
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#check-server-time" /></para>
        /// </summary>
        /// <param name="resetAutoTimestamp">Whether the response should be used for a new auto timestamp calculation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Server time</returns>
        Task<WebCallResult<DateTime>> GetServerTimeAsync(bool resetAutoTimestamp = false, CancellationToken ct = default);

        /// <summary>
        /// Get's information about the exchange including rate limits and symbol list
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#exchange-information" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Exchange info</returns>
        Task<WebCallResult<HitoBitFuturesCoinExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the recent trades for a symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#recent-trades-list" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get recent trades for, for example `BTCUSD_PERP`</param>
        /// <param name="limit">Result limit</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of recent trades</returns>
        Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the historical  trades for a symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#old-trades-lookup-market_data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get recent trades for, for example `BTCUSD_PERP`</param>
        /// <param name="limit">Result limit</param>
        /// <param name="fromId">From which trade id on results should be retrieved</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of recent trades</returns>
        Task<WebCallResult<IEnumerable<IHitoBitRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default);

        /// <summary>
        /// Gets compressed, aggregate trades. Trades that fill at the time, from the same order, with the same price will have the quantity aggregated.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#compressed-aggregate-trades-list" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the trades for, for example `BTCUSD_PERP`</param>
        /// <param name="fromId">ID to get aggregate trades from INCLUSIVE.</param>
        /// <param name="startTime">Time to start getting trades from</param>
        /// <param name="endTime">Time to stop getting trades from</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The aggregated trades list for the symbol</returns>
        Task<WebCallResult<IEnumerable<HitoBitAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get candlestick data for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#kline-candlestick-data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="interval">The candlestick timespan</param>
        /// <param name="startTime">Start time to get candlestick data</param>
        /// <param name="endTime">End time to get candlestick data</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The candlestick data for the provided symbol</returns>
        Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetKlinesAsync(string symbol, KlineInterval interval,
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get premium index kline data for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#premium-index-kline-data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="interval">The candlestick timespan</param>
        /// <param name="startTime">Start time to get candlestick data</param>
        /// <param name="endTime">End time to get candlestick data</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The candlestick data for the provided symbol</returns>
        Task<WebCallResult<IEnumerable<HitoBitMarkIndexKline>>> GetPremiumIndexKlinesAsync(string symbol, KlineInterval interval,
            DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get funding rate info for symbols that had FundingRateCap/ FundingRateFloor / fundingIntervalHours adjustment
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#get-funding-rate-info" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesFundingInfo>>> GetFundingInfoAsync(CancellationToken ct = default);

        /// <summary>
        /// Get funding rate history for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#get-funding-rate-history-of-perpetual-futures" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="startTime">Start time to get funding rate history</param>
        /// <param name="endTime">End time to get funding rate history</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The funding rate history for the provided symbol</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesFundingRateHistory>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Gets Top Trader Long/Short Ratio (Accounts)
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#top-trader-long-short-ratio-accounts" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="period">The period timespan</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Start time to get top trader long/short ratio (accounts)</param>
        /// <param name="endTime">End time to get top trader long/short ratio (accounts)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Top Trader Long/Short Ratio (Accounts) info</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Gets Top Trader Long/Short Ratio (Positions)
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#top-trader-long-short-ratio-positions" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="period">The period timespan</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Start time to get top trader long/short ratio (positions)</param>
        /// <param name="endTime">End time to get top trader long/short ratio (positions)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Top Trader Long/Short Ratio (Positions) info</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Gets Global Long/Short Ratio (Accounts)
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#long-short-ratio" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="period">The period timespan</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Start time to get global long/short ratio (accounts)</param>
        /// <param name="endTime">End time to get global long/short ratio (accounts)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Global Long/Short Ratio (Accounts) info</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Kline/candlestick bars for the mark price of a symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#mark-price-kline-candlestick-data" /></para>
        /// </summary>
        /// <param name="symbol">The symbol get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="interval">The interval of the klines</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitMarkIndexKline>>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the order book for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#order-book" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the order book for, for example `BTCUSD_PERP`</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The order book for the symbol</returns>
        Task<WebCallResult<HitoBitFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get Mark Price and Funding Rate for the provided symbol
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#index-price-and-mark-price" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="pair">Filter by pair, for example `BTCUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesCoinMarkPrice>>> GetMarkPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default);

        /// <summary>
        /// Get candlestick data for the provided pair
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#continuous-contract-kline-candlestick-data" /></para>
        /// </summary>
        /// <param name="pair">The symbol to get the data for, for example `BTCUSD`</param>
        /// <param name="contractType">The contract type</param>
        /// <param name="interval">The candlestick timespan</param>
        /// <param name="startTime">Start time to get candlestick data</param>
        /// <param name="endTime">End time to get candlestick data</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The candlestick data for the provided symbol</returns>
        Task<WebCallResult<IEnumerable<IHitoBitKline>>> GetContinuousContractKlinesAsync(string pair, ContractType contractType, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get candlestick data for the provided pair
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#index-price-kline-candlestick-data" /></para>
        /// </summary>
        /// <param name="pair">The symbol to get the data for, for example `BTCUSD`</param>
        /// <param name="interval">The candlestick timespan</param>
        /// <param name="startTime">Start time to get candlestick data</param>
        /// <param name="endTime">End time to get candlestick data</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The candlestick data for the provided symbol</returns>
        Task<WebCallResult<IEnumerable<HitoBitMarkIndexKline>>> GetIndexPriceKlinesAsync(string pair, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get data regarding the last 24 hours change
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#24hr-ticker-price-change-statistics" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="pair">Filter by pair, for example `BTCUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Data over the last 24 hours</returns>
        Task<WebCallResult<IEnumerable<IHitoBit24HPrice>>> GetTickersAsync(string? symbol = null, string? pair = null, CancellationToken ct = default);

        /// <summary>
        /// Gets the best price/quantity on the order book for a symbol.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#symbol-order-book-ticker" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to get book price for, for example `BTCUSD_PERP`</param>
        /// <param name="pair">Filter by pair, for example `BTCUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of book prices</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesBookPrice>>> GetBookPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default);

        /// <summary>
        /// Get present open interest of a specific symbol.
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#open-interest" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to get the data for, for example `BTCUSD_PERP`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Open Interest info</returns>
        Task<WebCallResult<HitoBitFuturesCoinOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets Open Interest History
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#open-interest-statistics" /></para>
        /// </summary>
        /// <param name="pair">The pair to get the data for, for example `BTCUSD`</param>
        /// <param name="contractType">The contract type</param>
        /// <param name="period">The period timespan</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Start time to get open interest history</param>
        /// <param name="endTime">End time to get open interest history</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Open Interest History info</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesCoinOpenInterestHistory>>> GetOpenInterestHistoryAsync(string pair, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Gets Taker Buy/Sell Volume Ratio
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#taker-buy-sell-volume" /></para>
        /// </summary>
        /// <param name="pair">The pair to get the data for, for example `BTCUSD`</param>
        /// <param name="contractType">The contract type</param>
        /// <param name="period">The period timespan</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Start time to get taker buy/sell volume ratio</param>
        /// <param name="endTime">End time to get taker buy/sell volume ratio</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Taker Buy/Sell Volume Ratio info</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesCoinBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string pair, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Gets basis
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#basis" /></para>
        /// </summary>
        /// <param name="pair">The pair to get the data for, for example `BTCUSD`</param>
        /// <param name="contractType">The contract type</param>
        /// <param name="period">The period timespan</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Basis</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesBasis>>> GetBasisAsync(string pair, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of the prices of all symbols
        /// <para><a href="https://hitobit-docs.github.io/apidocs/delivery/en/#symbol-price-ticker" /></para>
        /// </summary>
        /// <param name="symbol">Retrieve for a symbol, for example `BTCUSD_PERP`</param>
        /// <param name="pair">Retrieve prices for a specific pair, for example `BTCUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of prices</returns>
        Task<WebCallResult<IEnumerable<HitoBitFuturesCoinPrice>>> GetPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default);
    }
}
