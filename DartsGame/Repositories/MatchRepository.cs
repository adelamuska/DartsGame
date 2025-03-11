using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class MatchRepository : BaseRepository<Match>
    {
        public MatchRepository(AppDbContext context) : base(context) { }

        public async Task<Match> GetById(Guid matchId)
        {
            return await _context.Matches.Include(m => m.Sets.OrderBy(s => s.SetNumber)).ThenInclude(s => s.Legs).FirstOrDefaultAsync(m => m.MatchId == matchId && !m.IsDeleted);
        }

        public async Task<Match> GetMatchWithSets(Guid matchId)
        {
            return await _context.Matches
                .Include(m => m.Sets)
                .FirstOrDefaultAsync(m => m.MatchId == matchId);
        }

        public async Task<List<Guid>> GetMatchPlayerIds(Guid matchId)
        {
            return await _context.PlayerMatches
                .Where(m => m.MatchId == matchId)
                .Select(m => m.PlayerId)
                .ToListAsync();
        }

        //public async Task CompleteMatch(Guid matchId, Guid winnerPlayerId)
        //{
        //    var match = await _context.Matches.FindAsync(matchId);
        //    if (match != null && !match.IsFinished)
        //    {
        //        match.IsFinished = true;
        //        match.WinnerPlayerId = winnerPlayerId;
        //        match.EndTime = DateTime.Now;
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}