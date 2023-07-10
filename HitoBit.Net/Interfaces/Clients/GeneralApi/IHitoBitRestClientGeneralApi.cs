using CryptoExchange.Net.Interfaces;
using System;

namespace HitoBit.Net.Interfaces.Clients.GeneralApi
{
    /// <summary>
    /// HitoBit general API endpoints
    /// </summary>
    public interface IHitoBitRestClientGeneralApi : IRestApiClient, IDisposable
    {
        /// <summary>
        /// Endpoints related to brokerage
        /// </summary>
        public IHitoBitRestClientGeneralApiBrokerage Brokerage { get; }

        /// <summary>
        /// Endpoints related to futures account interactions
        /// </summary>
        public IHitoBitRestClientGeneralApiFutures Futures { get; }

        /// <summary>
        /// Endpoints related to savings
        /// </summary>
        public IHitoBitRestClientGeneralApiSavings Savings { get; }

        /// <summary>
        /// Endpoints related to crypto loans
        /// </summary>
        public IHitoBitRestClientGeneralApiLoans CryptoLoans { get; }

        /// <summary>
        /// Endpoints related to mining
        /// </summary>
        public IHitoBitRestClientGeneralApiMining Mining { get; }

        /// <summary>
        /// Endpoints related to requesting data for and controlling sub accounts
        /// </summary>
        public IHitoBitRestClientGeneralApiSubAccount SubAccount { get; }
    }
}
