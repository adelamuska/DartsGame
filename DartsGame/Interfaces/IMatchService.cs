using DartsGame.Entities;

namespace DartsGame.Interfaces
{
    public interface IMatchService
    {
        Task<Match> StartMatch(int score, int sets, int legs, int numberOfPlayers, List<string> playerNames);
    }
}
