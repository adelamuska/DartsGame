namespace DartsGame.Interfaces.ServiceInterfaces.Statistics
{
    public interface IMatchStatsService
    {
        Task<int> CalculateSetsWon(Guid matchId, Guid playerId);
        Task<int> CalculateLegsWon(Guid matchId, Guid playerId);
        Task<decimal> CalculatePPD(Guid matchId, Guid playerId);
        Task<decimal> CalculateCheckoutPercentage(Guid matchId, Guid playerId);
        Task<int> CalculateBestCheckoutPoints(Guid matchId, Guid playerId);
        Task<int> CalculateSixtyPlusCount(Guid matchId, Guid playerId);
        Task<int> CalculateHundredPlusCount(Guid matchId, Guid playerId);
        Task<int> CalculateHundredFortyPlusCount(Guid matchId, Guid playerId);
        Task<int> CalculateOneEightyCount(Guid matchId, Guid playerId);
        Task<decimal> CalculateFirst9PPD(Guid matchId, Guid playerId);
    }
}
