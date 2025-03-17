using DartsGame.Enums;

namespace DartsGame.DTOs
{
    public class MatchWithStatsDTO
    {
        public Guid MatchId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public BestOfSets BestOfSets { get; set; }
        public StartingScore StartingScore { get; set; }
        public Guid? WinnerPlayerId { get; set; }
        public bool IsFinished { get; set; }
        public List<PlayerMatchStatsDTO> Players { get; set; } = new List<PlayerMatchStatsDTO>();
    }
}
