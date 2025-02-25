namespace DartsGame.RequestDTOs
{
    public class ChangeTurnRequest
    {
        public Guid MatchId { get; set; }
        public int Throw1 { get; set; }
        public int Throw2 { get; set; }
        public int Throw3 { get; set; }
    }
}
