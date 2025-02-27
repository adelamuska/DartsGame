namespace DartsGame.DTO
{
    public class TurnThrowDTO
    {
        public Guid TurnThrowId { get; set; }
        public Guid TurnId { get; set; }
        public string Throw1 { get; set; }
        public string Throw2 { get; set; }
        public string Throw3 { get; set; }
        public int Score { get; set; }
    }
}
