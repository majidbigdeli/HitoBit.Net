using System.Collections.Generic;
using HitoBit.Net.Enums;
using CryptoExchange.Net.Converters;

namespace HitoBit.Net.Converters
{
    internal class ProjectStatusConverter : BaseConverter<ProjectStatus>
    {
        public ProjectStatusConverter() : this(true) { }
        public ProjectStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<ProjectStatus, string>> Mapping => new List<KeyValuePair<ProjectStatus, string>>
        {
            new KeyValuePair<ProjectStatus, string>(ProjectStatus.Holding, "HOLDING"),
            new KeyValuePair<ProjectStatus, string>(ProjectStatus.Redeemed, "REDEEMED")
        };
    }
}
