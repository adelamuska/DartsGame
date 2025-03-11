namespace DartsGame.DTO
{
    public class TurnDTO
    {
        public Guid TurnId { get; set; }
        public Guid PlayerId { get; set; }
        public Guid LegId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsCheckoutAttempt { get; set; }
        public bool IsCheckoutSuccess { get; set; }
        public bool IsBusted { get; set; }
    }
}
