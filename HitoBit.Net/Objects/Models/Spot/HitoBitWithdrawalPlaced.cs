namespace HitoBit.Net.Objects.Models.Spot
{
    /// <summary>
    /// Result of placing a withdrawal
    /// </summary>
    public record HitoBitWithdrawalPlaced
    {
        /// <summary>
        /// The id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}
