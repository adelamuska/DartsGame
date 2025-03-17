using DartsGame.DTO;
using DartsGame.DTOs;
using DartsGame.Entities;

namespace DartsGame.Interfaces.ServiceInterfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchWithStatsDTO>> GetAllWithStats();
        Task<MatchDTO> GetById(Guid matchId);
        Task<Match> StartMatch(int score, int sets, int legs, int numberOfPlayers, List<string> playerNames);
        Task DeleteMatch(Guid matchId);
    }
}
