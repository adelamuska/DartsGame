using DartsGame.Data;
using DartsGame.Entities;

namespace DartsGame.Repositories
{
    public class PlayerRepository : BaseRepository<Player>
    {
        public PlayerRepository(AppDbContext context) : base(context) { }
        
    }
}
