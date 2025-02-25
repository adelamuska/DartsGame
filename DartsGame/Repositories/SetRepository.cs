using DartsGame.Data;
using DartsGame.Entities;

namespace DartsGame.Repositories
{
    public class SetRepository : BaseRepository<Set>
    {
        public SetRepository(AppDbContext context) : base(context) { }
   
    }
}
