//using DartsGame.Data;
//using DartsGame.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace DartsGame.Repositories
//{
//    public class GameRepository
//    {
//        private readonly AppDbContext _context;

//        public GameRepository(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Leg> GetLegById(Guid legId)
//        {
//            return await _context.Legs.FindAsync(legId);
//        }

//        public async Task<Set> GetSetById(Guid setId)
//        {
//            return await _context.Sets.FindAsync(setId);
//        }

//        public async Task<Set> GetSetWithLegs(Guid setId)
//        {
//            return await _context.Sets.Include(s => s.Legs)
//                .FirstOrDefaultAsync(s => s.SetId == setId);
//        }

//        public async Task<Match> GetMatchById(Guid matchId)
//        {
//            return await _context.Matches.FindAsync(matchId);
//        }

//        public async Task<Turn> GetCurrentTurn(Guid legId)
//        {
//            return await _context.Turns
//                .Where(t => t.LegId == legId && !t.IsDeleted)
//                .OrderByDescending(t => t.TimeStamp)
//                .FirstOrDefaultAsync();
//        }

//        public async Task<List<LegScore>> GetLegScores(Guid legId)
//        {
//            return await _context.LegScores
//                .Where(ls => ls.LegId == legId)
//                .ToListAsync();
//        }

//        public async Task<List<TurnThrow>> GetTurnThrows(Guid turnId)
//        {
//            return await _context.TurnThrows
//                .Where(tt => tt.TurnId == turnId)
//                .OrderBy(tt => tt.TurnThrowId)
//                .ToListAsync();
//        }

//        public async Task<SetResult> GetSetResult(Guid setId, Guid playerId)
//        {
//            return await _context.SetResults
//                .FirstOrDefaultAsync(s => s.SetId == setId && s.PlayerId == playerId);
//        }

//        public async Task<List<SetResult>> GetSetResults(Guid setId)
//        {
//            return await _context.SetResults
//                .Where(l => l.SetId == setId)
//                .ToListAsync();
//        }

//        public async Task<List<Player>> GetMatchPlayers(Guid matchId)
//        {
//            return await _context.PlayerMatches
//                .Where(p => p.MatchId == matchId)
//                .Select(pm => pm.Player)
//                .ToListAsync();
//        }

//        public async Task<List<Guid>> GetMatchPlayerIds(Guid matchId)
//        {
//            return await _context.PlayerMatches
//                .Where(m => m.MatchId == matchId)
//                .Select(m => m.PlayerId)
//                .ToListAsync();
//        }

//        public async Task<bool> HasUnfinishedSets(Guid matchId)
//        {
//            return await _context.Sets
//                .AnyAsync(s => s.MatchId == matchId && !s.IsFinished);
//        }

//        public async Task<Set> GetLastSet(Guid matchId)
//        {
//            return await _context.Sets
//                .Where(s => s.MatchId == matchId)
//                .OrderByDescending(s => s.SetNumber)
//                .FirstOrDefaultAsync();
//        }

//        public async Task<List<dynamic>> GetSetsWonByPlayer(Guid matchId)
//        {
//            return await _context.Sets
//                .Where(s => s.MatchId == matchId && s.IsFinished && s.WinnerPlayerId.HasValue)
//                .GroupBy(s => s.WinnerPlayerId.Value)
//                .Select(g => new { PlayerId = g.Key, SetsWon = g.Count() })
//                .ToListAsync<dynamic>();
//        }

//        public async Task<Player> GetPlayerById(Guid playerId)
//        {
//            return await _context.Players.FindAsync(playerId);
//        }

//        public async Task<Turn> GetFirstTurnOfLeg(Guid legId)
//        {
//            return await _context.Turns
//                .Where(t => t.LegId == legId && !t.IsDeleted)
//                .OrderBy(t => t.TimeStamp)
//                .FirstOrDefaultAsync();
//        }

//        public void AddLeg(Leg leg)
//        {
//            _context.Legs.Add(leg);
//        }

//        public void AddLegScore(LegScore legScore)
//        {
//            _context.LegScores.Add(legScore);
//        }

//        public void AddTurn(Turn turn)
//        {
//            _context.Turns.Add(turn);
//        }

//        public void AddSet(Set set)
//        {
//            _context.Sets.Add(set);
//        }

//        public async Task SaveChangesAsync()
//        {
//            await _context.SaveChangesAsync();
//        }
//    }
//}