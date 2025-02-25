namespace DartsGame.RequestDTOs
{
    public class ValidateLegCompletionRequest
    {
        public Guid LegId { get; set; }
        public int TurnScore { get; set; }
        public string LastThrow { get; set; }
    }
}
