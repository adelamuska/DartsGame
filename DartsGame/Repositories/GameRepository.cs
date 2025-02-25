using DartsGame.Entities;

namespace DartsGame.Repositories
{
    public interface GameRepository
    {

        Task<Match> StartMatch(int score, int sets, string numberOfPlayers, string playerName);
        Task<Leg> StartLeg(Match match);
        public void EndSet(Set set);
    }
}
