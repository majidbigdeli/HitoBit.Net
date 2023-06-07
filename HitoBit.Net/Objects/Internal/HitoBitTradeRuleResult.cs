namespace HitoBit.Net.Objects.Internal
{
    internal class HitoBitTradeRuleResult
    {
        public bool Passed { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuoteQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? StopPrice { get; set; }
        public string? ErrorMessage { get; set; }

        public static HitoBitTradeRuleResult CreatePassed(decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice)
        {
            return new HitoBitTradeRuleResult
            {
                Passed = true,
                Quantity = quantity,
                Price = price,
                StopPrice = stopPrice,
                QuoteQuantity = quoteQuantity
            };
        }

        public static HitoBitTradeRuleResult CreateFailed(string message)
        {
            return new HitoBitTradeRuleResult
            {
                Passed = false,
                ErrorMessage = message
            };
        }
    }
}
