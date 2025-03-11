using System.Text.Json.Serialization;
using DartsGame.Enums;

namespace DartsGame.Entities
{
    public class Match : ISoftDeletable
    {
        public Guid MatchId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public BestOfSets BestOfSets { get; set; }
        public StartingScore StartingScore { get; set; }
        public Guid? WinnerPlayerId { get; set; }
        public bool IsFinished { get; set; }
        public bool IsDeleted { get; set; }


        public ICollection<PlayerMatch> PlayerMatches { get; set; }
        public ICollection<Set> Sets { get; set; } = new List<Set>();
        public ICollection<MatchStats> MatchStats { get; set; } = new List<MatchStats>();

        public Match()
        {

        }

        public Match(Guid matchId, DateTime startTime, DateTime? endTime, BestOfSets bestOfSets, StartingScore startingScore, bool isFinished)
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
