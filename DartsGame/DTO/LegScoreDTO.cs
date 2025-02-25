namespace DartsGame.DTO
{
    public class LegScoreDTO
    {
        public Guid LegId { get; set; }
        public Guid PlayerId { get; set; }
        public int RemainingScore { get; set; }
    }
}
