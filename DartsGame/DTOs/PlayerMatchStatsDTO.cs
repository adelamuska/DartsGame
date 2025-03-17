namespace DartsGame.DTOs
{
    public class PlayerMatchStatsDTO
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int SetsWon { get; set; }
    }
}
