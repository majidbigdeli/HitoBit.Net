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
        /// Endpoints related to crypto loans
        /// </summary>
        public IHitoBitRestClientGeneralApiLoans CryptoLoans { get; }

        /// <summary>
        /// Endpoints related to auto invest
        /// </summary>
        public IHitoBitRestClientGeneralApiAutoInvest AutoInvest { get; }

        /// <summary>
        /// Endpoints related to mining
        /// </summary>
        public IHitoBitRestClientGeneralApiMining Mining { get; }

        /// <summary>
        /// Endpoints related to requesting data for and controlling sub accounts
        /// </summary>
        public IHitoBitRestClientGeneralApiSubAccount SubAccount { get; }

        /// <summary>
        /// Endpoints related to staking
        /// </summary>
        IHitoBitRestClientGeneralApiStaking Staking { get; }

        /// <summary>
        /// Endpoints related to HitoBit Simple Earn
        /// </summary>
        IHitoBitRestClientGeneralApiSimpleEarn SimpleEarn { get; }

        /// <summary>
        /// Endpoints related to HitoBit Copy Trading
        /// </summary>
        IHitoBitRestClientGeneralApiCopyTrading CopyTrading { get; }
    }
}
