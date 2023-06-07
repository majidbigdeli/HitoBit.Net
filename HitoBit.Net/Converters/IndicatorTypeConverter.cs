using System.Collections.Generic;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;

namespace HitoBit.Net.Converters
{
    internal class IndicatorTypeConverter : BaseConverter<IndicatorType>
    {
        public IndicatorTypeConverter() : this(true) { }
        public IndicatorTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<IndicatorType, string>> Mapping => new List<KeyValuePair<IndicatorType, string>>
        {
            new KeyValuePair<IndicatorType, string>(IndicatorType.CancelationRatio, "GCR"),
            new KeyValuePair<IndicatorType, string>(IndicatorType.UnfilledRatio, "UFR"),
            new KeyValuePair<IndicatorType, string>(IndicatorType.ExpirationRatio, "IFER")
        };
    }
}
