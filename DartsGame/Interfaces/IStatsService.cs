namespace DartsGame.Interfaces
{
    public interface IStatsService
    {
        public Task<decimal> CalculatePPD(Guid Id, Guid playerId);
        public Task<decimal> CalculateFirst9PPD(Guid Id, Guid playerId);
        public Task<decimal> CalculateCheckoutPercentage(Guid Id, Guid playerId);
        public Task<int> CalculateSixtyPlusCount(Guid Id, Guid playerId);
        public Task<int> CalculateHundredPlusCount(Guid Id, Guid playerId);
        public Task<int> CalculateHundredFortyPlusCount(Guid Id, Guid playerId);
        public Task<int> CalculateOneEightyCount(Guid Id, Guid playerId);





    }
}
