using CryptoExchange.Net.Interfaces;
using System;

namespace HitoBit.Net.Interfaces.Clients.GeneralApi
{
    /// <summary>
    /// HitoBit general API endpoints
    /// </summary>
    public interface IHitoBitClientGeneralApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to brokerage
        /// </summary>
        public IHitoBitClientGeneralApiBrokerage Brokerage { get; }

        /// <summary>
        /// Endpoints related to futures account interactions
        /// </summary>
        public IHitoBitClientGeneralApiFutures Futures { get; }

        /// <summary>
        /// Endpoints related to savings
        /// </summary>
        public IHitoBitClientGeneralApiSavings Savings { get; }

        /// <summary>
        /// Endpoints related to crypto loans
        /// </summary>
        public IHitoBitClientGeneralApiCryptoLoans CryptoLoans { get; }

        /// <summary>
        /// Endpoints related to mining
        /// </summary>
        public IHitoBitClientGeneralApiMining Mining { get; }

        /// <summary>
        /// Endpoints related to requesting data for and controlling sub accounts
        /// </summary>
        public IHitoBitClientGeneralApiSubAccount SubAccount { get; }
    }
}
