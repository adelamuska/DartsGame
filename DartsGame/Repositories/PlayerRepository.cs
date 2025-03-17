using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(AppDbContext context) : base(context) { }

        public async Task<Player> GetById(Guid playerId)
        {
            return await _context.Players.FindAsync(playerId);
        }


      

        public async Task<List<Guid>> GetActivePlayerIds(Guid matchId)
        {
            return await _context.PlayerMatches
                .Where(pm => pm.MatchId == matchId)
                .Select(pm => pm.PlayerId)
                .ToListAsync();
        }
    }


}

