using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HitoBit.Net.Objects.Models.Spot.Loans
{
    /// <summary>
    /// Ltv adjustment info
    /// </summary>
    public class HitoBitCryptoLoanLtvAdjustRecord
    {
        /// <summary>
        /// The loaning asset
        /// </summary>
        [JsonProperty("loanCoin")]
        public string LoanAsset { get; set; } = string.Empty;
        /// <summary>
        /// The collateral asset
        /// </summary>
        [JsonProperty("collateralCoin")]
        public string CollateralAsset { get; set; } = string.Empty;
        /// <summary>
        /// Direction
        /// </summary>
        public string Direction { get; set; } = string.Empty;
        /// <summary>
        /// Amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Pre adjust ltv
        /// </summary>
        public decimal PreLtv { get; set; }
        /// <summary>
        /// Post adjust ltv
        /// </summary>
        public decimal AfterLtv { get; set; }
        /// <summary>
        /// Adjust time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime AdjustTime { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        public long OrderId { get; set; }
    }

    public class HitoBitCryptoLoanAsset
    {
        /// <summary>
        /// Loan asset
        /// </summary>
        [JsonProperty("loanCoin")]
        public string LoanAsset { get; set; } = string.Empty;
        /// <summary>
        /// Hourly interest rate for 7 days
        /// </summary>
        [JsonProperty("_7dHourlyInterestRate")]
        public decimal HourlyInterest7Days { get; set; }

        /// <summary>
        /// Daily interest rate for 7 days
        /// </summary>
        [JsonProperty("_7dDailyInterestRate")]
        public decimal DailyInterest7Days { get; set; }
        /// <summary>
        /// Hourly interest rate for 14 days
        /// </summary>
        [JsonProperty("_14dHourlyInterestRate")]
        public decimal HourlyInterest14Days { get; set; }

        /// <summary>
        /// Daily interest rate for 14 days
        /// </summary>
        [JsonProperty("_14dDailyInterestRate")]
        public decimal DailyInterest14Days { get; set; }
        /// <summary>
        /// Daily interest rate for 30 days
        /// </summary>
        [JsonProperty("_30dHourlyInterestRate")]
        public decimal HourlyInterest30Days { get; set; }
        /// <summary>
        /// Daily interest rate for 30 days
        /// </summary>
        [JsonProperty("_30dDailyInterestRate")]
        public decimal DailyInterest30Days { get; set; }
        /// <summary>
        /// Daily interest rate for 90 days
        /// </summary>
        [JsonProperty("_90dHourlyInterestRate")]
        public decimal HourlyInterest90Days { get; set; }
        /// <summary>
        /// Daily interest rate for 90 days
        /// </summary>
        [JsonProperty("_90dDailyInterestRate")]
        public decimal DailyInterest90Days { get; set; }
        /// <summary>
        /// Daily interest rate for 180 days
        /// </summary>
        [JsonProperty("_180dHourlyInterestRate")]
        public decimal HourlyInterest180Days { get; set; }
        /// <summary>
        /// Daily interest rate for 180 days
        /// </summary>
        [JsonProperty("_180dDailyInterestRate")]
        public decimal DailyInterest180Days { get; set; }

        /// <summary>
        /// Min limit
        /// </summary>
        [JsonProperty("minLimit")]
        public decimal MinLimit { get; set; }
        /// <summary>
        /// Min limit
        /// </summary>
        [JsonProperty("maxLimit")]
        public decimal MaxLimit { get; set; }
        /// <summary>
        /// Vip level
        /// </summary>
        [JsonProperty("vipLevel")]
        public int VipLevel { get; set; }
    }

    /// <summary>
    /// Collateral asset info
    /// </summary>
    public class HitoBitCryptoLoanCollateralAsset
    {
        /// <summary>
        /// Collateral asset
        /// </summary>
        [JsonProperty("collateralCoin")]
        public string ColleteralAsset { get; set; } = string.Empty;
        /// <summary>
        /// Initial ltv
        /// </summary>
        [JsonProperty("initialLTV")]
        public decimal InitialLtv { get; set; }
        /// <summary>
        /// Margin call ltv
        /// </summary>
        [JsonProperty("marginCallLTV")]
        public decimal MarginCallLtv { get; set; }
        /// <summary>
        /// Liquidation ltv
        /// </summary>
        [JsonProperty("liquidationLTV")]
        public decimal LiquidationLtv { get; set; }
        /// <summary>
        /// Max limit
        /// </summary>
        [JsonProperty("maxLimit")]
        public decimal MaxLimit { get; set; }
        /// <summary>
        /// Vip level
        /// </summary>
        [JsonProperty("vipLevel")]
        public int VipLevel { get; set; }
    }


    /// <summary>
    /// Repay rate info
    /// </summary>
    public class HitoBitCryptoLoanRepayRate
    {
        /// <summary>
        /// Loan asset
        /// </summary>
        [JsonProperty("loanCoin")]
        public string LoanAsset { get; set; } = string.Empty;
        /// <summary>
        /// Collateral asset
        /// </summary>
        [JsonProperty("collateralCoin")]
        public string CollateralAsset { get; set; } = string.Empty;
        /// <summary>
        /// Repay quantity
        /// </summary>
        [JsonProperty("repayAmount")]
        public decimal RepayQuantity { get; set; }
        /// <summary>
        /// Rate
        /// </summary>
        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }

    public class HitoBitCryptoLoanMarginCallResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Collateral asset
        /// </summary>
        [JsonProperty("collateralCoin")]
        public string CollateralAsset { get; set; } = string.Empty;
        /// <summary>
        /// Pre margin call 
        /// </summary>
        [JsonProperty("preMarginCall")]
        public decimal PreMarginCall { get; set; }
        /// <summary>
        /// After margin call
        /// </summary>
        [JsonProperty("afterMarginCall")]
        public decimal AfterMarginCall { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("customizeTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }

}
