namespace DartsGame.DTO
{
    public class PlayerDTO
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
    }
}
