namespace DartsGame.RequestDTOs
{
    public class ProcessTurnRequest
    {
        public Guid MatchId { get; set; }
        public TurnThrowRequestDTO TurnThrows { get; set; }
    }
}
