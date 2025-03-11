using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class PlayerRepository : BaseRepository<Player>
    {
        public PlayerRepository(AppDbContext context) : base(context) { }

        public async Task<Player> GetById(Guid playerId)
        {
            return await _context.Players.FindAsync(playerId);
        }


        //public async Task UpdatePlayersStats(Guid winnerId, List<Guid> loserIds)
        //{
        //    var winner = await _context.Players.FindAsync(winnerId);
        //    if (winner != null)
        //    {
        //        winner.GamesWon++;
        //    }

        //    foreach (var loserId in loserIds)
        //    {
        //        var loser = await _context.Players.FindAsync(loserId);
        //        if (loser != null)
        //        {
        //            loser.GamesLost++;
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}

        public async Task<List<Guid>> GetActivePlayerIds(Guid matchId)
        {
            return await _context.PlayerMatches
                .Where(pm => pm.MatchId == matchId)
                .Select(pm => pm.PlayerId)
                .ToListAsync();
        }
    }


}

