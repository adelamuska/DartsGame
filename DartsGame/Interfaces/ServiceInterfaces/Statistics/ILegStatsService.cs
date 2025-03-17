namespace DartsGame.Interfaces.ServiceInterfaces.Statistics
{
    public interface ILegStatsService
    {
        public Task<decimal> CalculatePPD(Guid Id, Guid playerId);
        public Task<decimal> CalculateFirst9PPD(Guid Id, Guid playerId);
        public Task<decimal> CalculateCheckoutPercentage(Guid Id, Guid playerId);
        Task<int> CalculateDartsThrown(Guid legId, Guid playerId);
        Task<int> CalculateCheckoutPoints(Guid legId, Guid playerId);
        public Task<int> CalculateSixtyPlusCount(Guid Id, Guid playerId);
        public Task<int> CalculateHundredPlusCount(Guid Id, Guid playerId);
        public Task<int> CalculateHundredFortyPlusCount(Guid Id, Guid playerId);
        public Task<int> CalculateOneEightyCount(Guid Id, Guid playerId);
    }
}
