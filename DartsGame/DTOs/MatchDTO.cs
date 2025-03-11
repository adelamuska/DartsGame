using System.Text.Json.Serialization;
using DartsGame.Entities;
using DartsGame.Enums;

namespace DartsGame.DTO
{
    public class MatchDTO
    {
        public Guid MatchId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public BestOfSets BestOfSets { get; set; }
        public StartingScore StartingScore { get; set; }
        public Guid? WinnerPlayerId { get; set; }
        public bool IsFinished { get; set; }
       // public ICollection<Set> Sets { get; set; }

        public MatchDTO(Guid matchId, DateTime startTime, DateTime? endTime, BestOfSets bestOfSets, StartingScore startingScore, bool isFinished)
        {
            MatchId = matchId;
            StartTime = startTime;
            EndTime = endTime;
            BestOfSets = bestOfSets;
            StartingScore = startingScore;
            IsFinished = isFinished;
        }
    }
}
