using System.Collections.Generic;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;

namespace HitoBit.Net.Converters
{
    internal class WithdrawDepositTransferTypeConverter : BaseConverter<WithdrawDepositTransferType>
    {
        public WithdrawDepositTransferTypeConverter() : this(true)
        {
        }

        public WithdrawDepositTransferTypeConverter(bool quotes) : base(quotes)
        {
        }

        protected override List<KeyValuePair<WithdrawDepositTransferType, string>> Mapping =>
            new List<KeyValuePair<WithdrawDepositTransferType, string>>
            {
                new KeyValuePair<WithdrawDepositTransferType, string>(
                    WithdrawDepositTransferType.Internal, "1"),
                new KeyValuePair<WithdrawDepositTransferType, string>(
                    WithdrawDepositTransferType.External, "0"),
            };
    }
}