﻿namespace HitoBit.Net.Objects.Models.Spot.Margin
{
    /// <summary>
    /// The result quantity of getting maxBorrowable or maxTransferable 
    /// </summary>
    public record HitoBitMarginAmount
    {
        /// <summary>
        /// The quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The borrow limit
        /// </summary>
        [JsonPropertyName("borrowLimit")]
        public decimal BorrowLimit { get; set; }
    }
}
