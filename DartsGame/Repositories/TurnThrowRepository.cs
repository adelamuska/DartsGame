using DartsGame.Data;
using DartsGame.Entities;

namespace DartsGame.Repositories
{
    public class TurnThrowRepository : BaseRepository<TurnThrow>
    {
        public TurnThrowRepository(AppDbContext context) : base(context) { }
        
    }
}
