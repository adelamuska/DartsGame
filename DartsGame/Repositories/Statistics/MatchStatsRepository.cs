using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories.Statistics
{
    public class MatchStatsRepository
    {
        private readonly AppDbContext _context;

        public MatchStatsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Set>> GetSetsByMatchId(Guid matchId)
        {
            return await _context.Sets
              .Where(s => s.MatchId == matchId)
              .OrderBy(s => s.SetNumber)
              .Include(s => s.Legs) 
              .ToListAsync();
        }

        public async Task<List<Leg>> GetLegsByMatchId(Guid matchId)
        {
            return await _context.Legs
                .Where(l => l.Set.MatchId == matchId && !l.IsDeleted)  
                .OrderBy(l => l.LegId)  
                .ToListAsync();
        }

        public async Task<int> GetSetsWon(Guid matchId, Guid playerId)
        {
            var match = await _context.Matches
                .Where(m => m.MatchId == matchId)
                .Include(m => m.Sets)
                .FirstOrDefaultAsync();

            if (match == null)
                throw new Exception("Match not found.");

            return match.Sets.Count(s => s.WinnerPlayerId == playerId);
        }

        //public async Task<int> GetLegsWon(Guid matchId, Guid playerId)
        //{
        //    var match = await _context.Matches
        //        .Where(m => m.MatchId == matchId)
        //        .Include(m => m.Sets)
        //        .ThenInclude(s => s.SetResults)
        //        .FirstOrDefaultAsync();

        //    if (match == null)
        //        throw new Exception("Match not found.");

        //    return match.Sets
        //        .SelectMany(s => s.SetResults)
        //        .Where(sr => sr.PlayerId == playerId)
        //        .Sum(sr => sr.LegsWon);
        //}

        //public async Task<int> GetTotalScore(Guid matchId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //         .Where(t => t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
        //         .SumAsync(t => t.Score);
        //}

        public async Task<int> GetTotalDartsThrown(Guid setId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Turn.Leg.Set.SetId == setId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .SumAsync(t => (t.Throw1 > 0 ? 1 : 0) + (t.Throw2 > 0 ? 1 : 0) + (t.Throw3 > 0 ? 1 : 0));
        }

        public async Task<int> GetCheckoutSuccesses(Guid matchId, Guid playerId)
        {
            return await _context.Turns
                .Where(t => t.Leg.Set.MatchId == matchId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutSuccess && !t.IsBusted)
                .GroupBy(t => t.Leg.Set.MatchId)
                .CountAsync();
        }

        public async Task<int> GetCheckoutAttempts(Guid matchId, Guid playerId)
        {
            return await _context.Turns
                .Where(t => t.Leg.Set.MatchId == matchId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutAttempt && !t.IsBusted)
                .GroupBy(t => t.Leg.Set.MatchId)
                .CountAsync();

        }
        
        

        //public async Task<int> GetSixtyPlusCount(Guid matchId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score >= 60 && t.Score< 100 && t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        //public async Task<int> GetHundredPlusCount(Guid matchId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score >= 100 && t.Score< 140 && t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        //public async Task<int> GetHundredFortyPlusCount(Guid matchId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score >= 140 && t.Score < 180 && t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        //public async Task<int> GetOneEightyCount(Guid matchId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score == 180 && t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        public async Task<IQueryable<TurnThrow>> GetFirstNineThrows(Guid matchId, Guid playerId)
        {
            return _context.TurnThrows
                .Where(t => t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .OrderBy(t => t.Turn.TimeStamp)
                .Take(9);
        }

        //public async Task<int> GetCheckoutPoints(Guid matchId, Guid playerId)
        //{
        //    return await _context.Turns
        //         .Where(t => t.Leg.Set.MatchId == matchId && t.PlayerId == playerId && t.IsCheckoutSuccess && !t.IsBusted)
        //         .SelectMany(t => t.TurnThrows)
        //         .Where(tw => tw.Throw1.HasValue || tw.Throw2.HasValue || tw.Throw3.HasValue)
        //         .Select(tw => (tw.Throw1 ?? 0) + (tw.Throw2 ?? 0) + (tw.Throw3 ?? 0))
        //         .DefaultIfEmpty()
        //         .MaxAsync();
        //}


    }
}

