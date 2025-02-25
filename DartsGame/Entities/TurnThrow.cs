namespace DartsGame.Entities
{
    public class TurnThrow : ISoftDeletable
    {
        public Guid TurnThrowId { get; set; }
        public Guid TurnId { get; set; }
        public int Throw1 { get; set; }
        public int Throw2 { get; set; }
        public int Throw3 { get; set; }
        public int Score { get; set; }
        public bool IsDeleted { get; set; }


        public Turn Turn { get; set; }

    }
}

