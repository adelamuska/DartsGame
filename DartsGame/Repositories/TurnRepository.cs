using DartsGame.Data;
using DartsGame.Entities;

namespace DartsGame.Repositories
{
    public class TurnRepository : BaseRepository<Turn>
    {
        public TurnRepository(AppDbContext context) : base(context) { }
    }
}
