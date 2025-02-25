namespace DartsGame.DTO
{
    public class TurnThrowDTO
    {
        public Guid TurnThrowId { get; set; }
        public Guid TurnId { get; set; }
        public int Throw1 { get; set; }
        public int Throw2 { get; set; }
        public int Throw3 { get; set; }
        public int Score { get; set; }
    }
}
