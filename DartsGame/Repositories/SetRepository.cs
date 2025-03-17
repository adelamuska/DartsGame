using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class SetRepository : BaseRepository<Set>, ISetRepository
    {
        public SetRepository(AppDbContext context) : base(context) { }

        public async Task<Set> GetSetById(Guid setId)
        {
            return await _context.Sets.FindAsync(setId);
        }

        public async Task<Set> GetSetWithLegs(Guid setId)
        {
            return await _context.Sets
                .Include(s => s.Legs)
                .FirstOrDefaultAsync(s => s.SetId == setId);
        }

        //public async Task<List<SetResult>> GetSetResults(Guid setId)
        //{
        //    return await _context.SetResults
        //        .Where(s => s.SetId == setId)
        //        .ToListAsync();
        //}

        //public async Task<bool> HasAnyPlayerWonSet(Guid setId, int legsNeededToWin)
        //{
        //    var setResults = await _context.SetResults
        //        .Where(s => s.SetId == setId)
        //        .ToListAsync();

        //    return setResults.Any(s => s.LegsWon >= legsNeededToWin);
        //}

        public async Task<int> GetLastSetNumber(Guid matchId)
        {
            return await _context.Sets
                .Where(s => s.MatchId == matchId)
                .Select(s => s.SetNumber)
                .MaxAsync();
        }

        public async Task<bool> HasUnfinishedSets(Guid matchId)
        {
            return await _context.Sets.AnyAsync(s => s.MatchId == matchId && !s.IsFinished);
        }

        public async Task<Dictionary<Guid, int>> GetSetsWonByPlayers(Guid matchId)
        {
            var setsWonByPlayer = await _context.Sets
                .Where(s => s.MatchId == matchId && s.IsFinished && s.WinnerPlayerId.HasValue)
                .GroupBy(s => s.WinnerPlayerId.Value)
                .Select(g => new { PlayerId = g.Key, SetsWon = g.Count() })
                .ToDictionaryAsync(g => g.PlayerId, g => g.SetsWon);

            return setsWonByPlayer;
        }
    }
}