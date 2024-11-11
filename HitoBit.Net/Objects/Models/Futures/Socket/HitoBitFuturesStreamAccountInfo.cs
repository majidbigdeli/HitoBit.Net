using HitoBit.Net.Converters;
using HitoBit.Net.Enums;

namespace HitoBit.Net.Objects.Models.Futures.Socket
{
    /// <summary>
    /// Account update
    /// </summary>
    public record HitoBitFuturesStreamAccountUpdate: HitoBitStreamEvent
    {
        /// <summary>
        /// The update data
        /// </summary>
        [JsonPropertyName("a")]
        public HitoBitFuturesStreamAccountUpdateData UpdateData { get; set; } = new HitoBitFuturesStreamAccountUpdateData();
        /// <summary>
        /// Transaction time
        /// </summary>
        [JsonPropertyName("T"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// The listen key the update was for
        /// </summary>
        public string ListenKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// Account update data
    /// </summary>
    public record HitoBitFuturesStreamAccountUpdateData
    {
        /// <summary>
        /// Account update reason type
        /// </summary>
        [JsonPropertyName("m"), JsonConverter(typeof(EnumConverter))]
        public AccountUpdateReason Reason { get; set; }

        /// <summary>
        /// Balances
        /// </summary>
        [JsonPropertyName("B")]
        public IEnumerable<HitoBitFuturesStreamBalance> Balances { get; set; } = Array.Empty<HitoBitFuturesStreamBalance>();

        /// <summary>
        /// Positions
        /// </summary>
        [JsonPropertyName("P")]
        public IEnumerable<HitoBitFuturesStreamPosition> Positions { get; set; } = Array.Empty<HitoBitFuturesStreamPosition>();
    }

    /// <summary>
    /// Information about an asset balance
    /// </summary>
    public record HitoBitFuturesStreamBalance
    {
        /// <summary>
        /// The asset this balance is for
        /// </summary>
        [JsonPropertyName("a")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The quantity that isn't locked in a trade
        /// </summary>
        [JsonPropertyName("wb")]
        public decimal WalletBalance { get; set; }
        /// <summary>
        /// The quantity that is locked in a trade
        /// </summary>
        [JsonPropertyName("cw")]
        public decimal CrossWalletBalance { get; set; }
        /// <summary>
        /// The balance change except PnL and commission
        /// </summary>
        [JsonPropertyName("bc")]
        public decimal BalanceChange { get; set; }
    }

    /// <summary>
    /// Information about an asset position
    /// </summary>
    public record HitoBitFuturesStreamPosition
    {
        /// <summary>
        /// The symbol this balance is for
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// The quantity of the position
        /// </summary>
        [JsonPropertyName("pa")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The entry price
        /// </summary>
        [JsonPropertyName("ep")]
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// The break even price
        /// </summary>
        [JsonPropertyName("bep")]
        public decimal BreakEvenPrice { get; set; }
        /// <summary>
        /// The accumulated realized PnL
        /// </summary>
        [JsonPropertyName("cr")]
        public decimal RealizedPnl { get; set; }
        /// <summary>
        /// The Unrealized PnL
        /// </summary>
        [JsonPropertyName("up")]
        public decimal UnrealizedPnl { get; set; }

        /// <summary>
        /// The margin type
        /// </summary>
        [JsonPropertyName("mt")]
        public FuturesMarginType MarginType { get; set; }

        /// <summary>
        /// The isolated wallet (if isolated position)
        /// </summary>
        [JsonPropertyName("iw")]
        public decimal IsolatedMargin { get; set; }

        /// <summary>
        /// Position Side
        /// </summary>
        [JsonPropertyName("ps")]
        public PositionSide PositionSide { get; set; }
    }
}
