using DartsGame.Data;
using DartsGame.DTOs;
using DartsGame.Entities;
using DartsGame.Repositories.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services.Statistics
{
    public class PlayerStatsService
    {
        private readonly PlayerStatsRepository _repository;

        public PlayerStatsService(PlayerStatsRepository repository)
        {
            _repository = repository;
        }

        public async Task<PlayerStats> GetPlayerStats(Guid playerId, int recentLegsLimit = 10)
        {
            var player = await _repository.GetPlayerByIdAsync(playerId);
            var recentLegs = await _repository.GetRecentLegStats(playerId, recentLegsLimit);


            if (!recentLegs.Any()) { 
                return new PlayerStats();
            }

            var playerStats = new PlayerStats()
            {

                PlayerName = player.Name,

                AveragePPD = Math.Round(recentLegs.Average(p => p.PPD), 2),
                BestPPD = Math.Round(recentLegs.Max(p => p.PPD), 2),

                AverageFirst9PPD = Math.Round(recentLegs.Average(p => p.First9PPD), 2),
                BestFirst9PPD = Math.Round(recentLegs.Max(p => p.First9PPD), 2),

                AverageCheckoutPercentage = Math.Round(recentLegs.Average(p => p.CheckoutPercentage), 2),
                BestCheckoutPercentage = Math.Round(recentLegs.Max(p => p.CheckoutPercentage), 2),

                AverageCheckoutPoints = recentLegs.Where(p => p.CheckoutPoints > 0).Any() ? Math.Round(recentLegs.Where(p=> p.CheckoutPoints > 0).Average(p => (decimal)p.CheckoutPoints), 2) : 0,
                BestCheckoutPoints = recentLegs.Max(p=> p.CheckoutPoints),

                LegsWon = recentLegs.Count(p => p.WinnerId == playerId),
                TotalLegs = recentLegs.Count(),
                WinPercentage = recentLegs.Count() > 0 ? Math.Round((decimal)recentLegs.Count(p=> p.WinnerId == playerId) / recentLegs.Count() * 100, 2) : 0,


                Total60Plus = recentLegs.Sum(p=> p.Count60Plus),
                Total100Plus = recentLegs.Sum(p=> p.Count100Plus),
                Total140Plus = recentLegs.Sum(p=> p.Count140Plus),
                Total180s = recentLegs.Sum(p=> p.Count180s),

                PerLeg60Plus = recentLegs.Count() > 0 ? Math.Round((decimal)recentLegs.Sum(p => p.Count60Plus) / recentLegs.Count(), 2) : 0,
                PerLeg100Plus = recentLegs.Count() > 0 ? Math.Round((decimal)recentLegs.Sum(p => p.Count100Plus) / recentLegs.Count(), 2) : 0,
                PerLeg140Plus = recentLegs.Count() > 0 ? Math.Round((decimal)recentLegs.Sum(p => p.Count140Plus) / recentLegs.Count(), 2) : 0,
                PerLeg180s = recentLegs.Count() > 0 ? Math.Round((decimal)recentLegs.Sum(p => p.Count180s) / recentLegs.Count(), 2) : 0

            };

            return playerStats;
                             
        }
    }
}
