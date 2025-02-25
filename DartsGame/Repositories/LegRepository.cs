using DartsGame.Data;
using DartsGame.Entities;

namespace DartsGame.Repositories
{
    public class LegRepository : BaseRepository<Leg>
    {
        public LegRepository(AppDbContext context) : base(context){ }
    }
}
