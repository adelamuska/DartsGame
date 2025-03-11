namespace DartsGame.Entities
{
    public class Leg : ISoftDeletable
    {
        public Guid LegId { get; set; }
        public Guid SetId { get; set; }
        public int LegNumber { get; set; }
        public bool IsFinished { get; set; }
        public Guid? WinnerId { get; set; }
        public bool IsDeleted { get; set; }

        public Set Set { get; set; }
        public Player Winner { get; set; }
        public ICollection<LegScore> LegScores { get; set; }
        public ICollection<Turn> Turns { get; set; }
        public ICollection<LegStats> LegStats { get; set; } = new List<LegStats>();


        public Leg(Guid legId, Guid setId, int legNumber, bool isFinished)
        {
            LegId = legId;
            SetId = setId;
            LegNumber = legNumber;
            IsFinished = isFinished;
        }
    }
}
