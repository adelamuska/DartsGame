namespace DartsGame.Interfaces.RepositoryInterfaces.Statistics
{
    public interface ILegStatsRepository
    {
        Task<int> GetTotalScore(Guid legId, Guid playerId);
        Task<int> GetDartsThrownCount(Guid legId, Guid playerId);
        Task<int> GetSixtyPlusCount(Guid legId, Guid playerId);
        Task<int> GetHundredPlusCount(Guid legId, Guid playerId);
        Task<int> GetHundredFortyPlusCount(Guid legId, Guid playerId);
        Task<int> GetOneEightyCount(Guid legId, Guid playerId);
        Task<int> GetCheckoutSuccessCount(Guid legId, Guid playerId);
        Task<int> GetCheckoutAttemptCount(Guid legId, Guid playerId);
        Task<List<int>> GetFirstNineThrows(Guid legId, Guid playerId);
    }
}
