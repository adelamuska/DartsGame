namespace DartsGame.Entities
{
    public class Player : ISoftDeletable
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public bool IsDeleted { get; set; }


        public ICollection<PlayerMatch> PlayerMatches { get; set; }


        public Player(Guid playerId, string name)
        {
            PlayerId = playerId;
            Name = name;
        }
    }
}
