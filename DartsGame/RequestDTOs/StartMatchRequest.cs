namespace DartsGame.RequestDTOs
{
    public class StartMatchRequest
    {
        public int StartingScore { get; set; }
        public int BestOfLegs { get; set; }
        public int BestOfSets { get; set; } 
        public List<string> PlayerNames { get; set; } = new List<string>();
    }
}
