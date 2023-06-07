using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace HitoBit.Net.Converters
{
    internal class HitoBitEarningTypeConverter : BaseConverter<HitoBitEarningType>
    {
        public HitoBitEarningTypeConverter() : this(true) { }
        public HitoBitEarningTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<HitoBitEarningType, string>> Mapping => new List<KeyValuePair<HitoBitEarningType, string>>
        {
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.MiningWallet, "0"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.MergedMining, "1"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.ActivityBonus, "2"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.Rebate, "3"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.SmartPool, "4"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.MiningAddress, "5"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.IncomeTransfer, "6"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.PoolSavings, "7"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.Transfered, "8"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.IncomeTransfer, "31"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.HashrateResaleMiningWallet, "32"),
            new KeyValuePair<HitoBitEarningType, string>(HitoBitEarningType.HashrateResalePoolSavings, "33")
        };
    }
}
