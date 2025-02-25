namespace DartsGame.Entities
{
    public class LegScore
    {
        public Guid LegId { get; set; }
        public Guid PlayerId { get; set; }
        public int RemainingScore { get; set; }

        public Leg Leg { get; set; }
        public Player Player { get; set; }
    }
}
