namespace DartsGame.RequestDTOs
{
    public class StartMatchRequest
    {
        public int StartingScore { get; set; }
        public int NumberOfSets { get; set; }
        public int NumberOfPlayers { get; set; }
        public List<string> PlayerNames { get; set; }
    }
}
