using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class MatchRepository : BaseRepository<Match>, IMatchRepository
    {
        public MatchRepository(AppDbContext context) : base(context) { }

        public async Task<Match> GetById(Guid matchId)
        {
            return await _context.Matches.Include(m => m.Sets.OrderBy(s => s.SetNumber)).ThenInclude(s => s.Legs).FirstOrDefaultAsync(m => m.MatchId == matchId && !m.IsDeleted);
        }

        //public async Task<Match> GetMatchWithSets(Guid matchId)
        //{
        //    return await _context.Matches
        //        .Include(m => m.Sets)
        //        .FirstOrDefaultAsync(m => m.MatchId == matchId);
        //}

        public async Task<List<Guid>> GetMatchPlayerIds(Guid matchId)
        {
            return await _context.PlayerMatches
                .Where(m => m.MatchId == matchId)
                .Select(m => m.PlayerId)
                .ToListAsync();
        }

      
    }
}