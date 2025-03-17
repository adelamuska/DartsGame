using DartsGame.Data;
using DartsGame.Interfaces.RepositoryInterfaces;

namespace DartsGame.Repositories
{
    public class GameRepositoryService : IGameRepositoryService
    {
        public LegRepository LegRepository { get; }
        public TurnRepository TurnRepository { get; }
        public LegScoreRepository LegScoreRepository { get; }
        public TurnThrowRepository TurnThrowRepository { get; }
        public SetRepository SetRepository { get; }
        public SetResultRepository SetResultRepository { get; }
        public MatchRepository MatchRepository { get; }
        public PlayerRepository PlayerRepository { get; }

        public GameRepositoryService(AppDbContext context)
        {
            LegRepository = new LegRepository(context);
            TurnRepository = new TurnRepository(context);
            LegScoreRepository = new LegScoreRepository(context);
            TurnThrowRepository = new TurnThrowRepository(context);
            SetRepository = new SetRepository(context);
            SetResultRepository = new SetResultRepository(context);
            MatchRepository = new MatchRepository(context);
            PlayerRepository = new PlayerRepository(context);
        }
    }
}
