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
        public ICollection<LegStats> LegStats { get; set; } = new List<LegStats>();
        public ICollection<SetStats> SetStats { get; set; } = new List<SetStats>();
        public ICollection<MatchStats> MatchStats { get; set; } = new List<MatchStats>();


        public Player(Guid playerId, string name)
        {
            PlayerId = playerId;
            Name = name;
        }
    }
}
