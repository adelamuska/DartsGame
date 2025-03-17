namespace DartsGame.Interfaces.ServiceInterfaces.Statistics
{
    public interface ISetStatsService
    {
        Task<decimal> CalculatePPD(Guid setId, Guid playerId);
        Task<decimal> CalculateFirst9PPD(Guid setId, Guid playerId);
        Task<decimal> CalculateCheckoutPercentage(Guid setId, Guid playerId);
        Task<int> CalculateBestCheckoutPoints(Guid setId, Guid playerId);
        Task<int> CalculateLegsWon(Guid setId, Guid playerId);
        Task<int> CalculateSixtyPlusCount(Guid setId, Guid playerId);
        Task<int> CalculateHundredPlusCount(Guid setId, Guid playerId);
        Task<int> CalculateHundredFortyPlusCount(Guid setId, Guid playerId);
        Task<int> CalculateOneEightyCount(Guid setId, Guid playerId);
    }
}
