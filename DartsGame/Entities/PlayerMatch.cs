namespace DartsGame.Entities
{
    public class PlayerMatch
    {
        public Guid MatchId { get; set; }
        public Match Match { get; set; }
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
