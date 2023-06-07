namespace HitoBit.Net.Objects.Models.Spot.Staking
{
    /// <summary>
    /// Staking result
    /// </summary>
    public class HitoBitStakingResult 
    {
        /// <summary>
        /// Successful
        /// </summary>
        public bool Success { get; set; }
    }

    /// <summary>
    /// Staking result
    /// </summary>
    public class HitoBitStakingPositionResult: HitoBitStakingResult
    {
        /// <summary>
        /// Id of the position
        /// </summary>
        public string? PositionId { get; set; }
    }
}
