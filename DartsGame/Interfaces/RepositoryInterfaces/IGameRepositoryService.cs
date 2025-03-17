using DartsGame.Repositories;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface IGameRepositoryService
    {
        LegRepository LegRepository { get; }
        TurnRepository TurnRepository { get; }
        LegScoreRepository LegScoreRepository { get; }
        TurnThrowRepository TurnThrowRepository { get; }
        SetRepository SetRepository { get; }
        SetResultRepository SetResultRepository { get; }
        MatchRepository MatchRepository { get; }
        PlayerRepository PlayerRepository { get; }
    }
}
