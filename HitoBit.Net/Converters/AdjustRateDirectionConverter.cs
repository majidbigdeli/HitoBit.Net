using System.Collections.Generic;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;

namespace HitoBit.Net.Converters
{
    internal class AdjustRateDirectionConverter : BaseConverter<AdjustRateDirection>
    {
        public AdjustRateDirectionConverter() : this(true) { }
        public AdjustRateDirectionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<AdjustRateDirection, string>> Mapping => new List<KeyValuePair<AdjustRateDirection, string>>
        {
            new KeyValuePair<AdjustRateDirection, string>(AdjustRateDirection.Reduced, "REDUCED"),
            new KeyValuePair<AdjustRateDirection, string>(AdjustRateDirection.Additional, "ADDITIONAL")
        };
    }
}
