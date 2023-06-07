﻿using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace HitoBit.Net.Converters
{
    internal class WalletTypeConverter : BaseConverter<WalletType>
    {
        public WalletTypeConverter() : this(true) { }
        public WalletTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<WalletType, string>> Mapping => new List<KeyValuePair<WalletType, string>>
        {
            new KeyValuePair<WalletType, string>(WalletType.Spot, "0"),
            new KeyValuePair<WalletType, string>(WalletType.Funding, "1"),
        };
    }
}
