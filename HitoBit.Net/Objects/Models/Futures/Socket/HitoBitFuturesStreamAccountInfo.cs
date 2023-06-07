using System;
using System.Collections.Generic;
using HitoBit.Net.Converters;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Account update
    /// </summary>
    public class HitoBitFuturesStreamAccountUpdate: HitoBitStreamEvent
    {
        /// <summary>
        /// The update data
        /// </summary>
        [JsonProperty("a")]
        public HitoBitFuturesStreamAccountUpdateData UpdateData { get; set; } = new HitoBitFuturesStreamAccountUpdateData();
        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// The listen key the update was for
        /// </summary>
        public string ListenKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// Account update data
    /// </summary>
    public class HitoBitFuturesStreamAccountUpdateData
    {
        /// <summary>
        /// Account update reason type
        /// </summary>
        [JsonProperty("m"), JsonConverter(typeof(AccountUpdateReasonConverter))]
        public AccountUpdateReason Reason { get; set; }

        /// <summary>
        /// Balances
        /// </summary>
        [JsonProperty("B")]
        public IEnumerable<HitoBitFuturesStreamBalance> Balances { get; set; } = Array.Empty<HitoBitFuturesStreamBalance>();

        /// <summary>
        /// Positions
        /// </summary>
        [JsonProperty("P")]
        public IEnumerable<HitoBitFuturesStreamPosition> Positions { get; set; } = Array.Empty<HitoBitFuturesStreamPosition>();
    }

    /// <summary>
    /// Information about an asset balance
    /// </summary>
    public class HitoBitFuturesStreamBalance
    {
        /// <summary>
        /// The asset this balance is for
        /// </summary>
        [JsonProperty("a")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The quantity that isn't locked in a trade
        /// </summary>
        [JsonProperty("wb")]
        public decimal WalletBalance { get; set; }
        /// <summary>
        /// The quantity that is locked in a trade
        /// </summary>
        [JsonProperty("cw")]
        public decimal CrossWalletBalance { get; set; }
        /// <summary>
        /// The balance change except PnL and commission
        /// </summary>
        [JsonProperty("bc")]
        public decimal BalanceChange { get; set; }
    }

    /// <summary>
    /// Information about an asset position
    /// </summary>
    public class HitoBitFuturesStreamPosition
    {
        /// <summary>
        /// The symbol this balance is for
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The quantity of the position
        /// </summary>
        [JsonProperty("pa")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The entry price
        /// </summary>
        [JsonProperty("ep")]
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// The accumulated realized PnL
        /// </summary>
        [JsonProperty("cr")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// The Unrealized PnL
        /// </summary>
        [JsonProperty("up")]
        public decimal UnrealizedPnl { get; set; }

        /// <summary>
        /// The margin type
        /// </summary>
        [JsonProperty("mt"), JsonConverter(typeof(FuturesMarginTypeConverter))]
        public FuturesMarginType MarginType { get; set; }

        /// <summary>
        /// The isolated wallet (if isolated position)
        /// </summary>
        [JsonProperty("iw")]
        public decimal IsolatedMargin { get; set; }

        /// <summary>
        /// Position Side
        /// </summary>
        [JsonProperty("ps"), JsonConverter(typeof(PositionSideConverter))]
        public PositionSide PositionSide { get; set; }
    }
}
