using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.DTOs;
using DartsGame.Interfaces.ServiceInterfaces.Statistics;
using DartsGame.Repositories.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services.Statistics
{
    public class GameHistoryService : IGameHistoryService
    {
        private readonly GameHistoryRepository _gameHistoryRepository;

        public GameHistoryService(GameHistoryRepository gameHistoryRepository)
        {
            _gameHistoryRepository = gameHistoryRepository;
        }

        public async Task<List<MatchStatsDTO>> GetMatchStatsByMatchId(Guid matchId)
        {
            return await _gameHistoryRepository.GetMatchStatsByMatchId(matchId);
        }
    }


}
