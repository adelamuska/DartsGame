using DartsGame.Data;
using DartsGame.DTOs;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories.Statistics
{
    public class PlayerStatsRepository : IPlayerStatsRepository
    {
        private readonly AppDbContext _context;
        public PlayerStatsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Player> GetPlayerByIdAsync(Guid playerId)
        {
            return await _context.Players
              .Where(p => p.PlayerId == playerId)
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RecentLegStatDto>> GetRecentLegStats(Guid playerId, int limit)
        {
            return await _context.LegStats
                .Where(p => p.PlayerId == playerId)
                .OrderByDescending(m => m.Leg.Set.Match.StartTime)
                .ThenByDescending(s => s.Leg.Set.SetNumber)
                .ThenByDescending(l => l.Leg.LegNumber)
                .Take(limit)
                .Select(ls => new RecentLegStatDto
                {
                    PPD = ls.PPD,
                    First9PPD = ls.First9PPD,
                    CheckoutPercentage = ls.CheckoutPercentage,
                    CheckoutPoints = ls.CheckoutPoints,
                    Count60Plus = ls.Count60Plus,
                    Count100Plus = ls.Count100Plus,
                    Count140Plus = ls.Count140Plus,
                    Count180s = ls.Count180s,
                    LegNumber = ls.Leg.LegNumber,
                    WinnerId = ls.Leg.WinnerId ?? Guid.Empty,
                    PlayerId = ls.PlayerId
                })
                .ToListAsync();
        }
    }
}
