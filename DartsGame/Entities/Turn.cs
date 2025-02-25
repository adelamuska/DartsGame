namespace DartsGame.Entities
{
    public class Turn : ISoftDeletable
    {
        public Guid TurnId { get; set; }
        public Guid PlayerId { get; set; }  
        public Guid LegId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsCheckoutAttempt { get; set; }
        public bool IsCheckoutSuccess { get; set; }
        public bool IsBusted { get; set; }
        public bool IsDeleted { get; set; }


        public Leg Leg { get; set; }
        public Player Player { get; set; }
        public ICollection<TurnThrow> TurnThrows { get; set; }

        public Turn(Guid turnId, Guid playerId, Guid legId, DateTime timeStamp, bool isCheckoutAttempt, bool isCheckoutSuccess, bool isBusted)
        {
            TurnId = turnId;
            PlayerId = playerId;
            LegId = legId;
            TimeStamp = timeStamp;
            IsCheckoutAttempt = isCheckoutAttempt;
            IsCheckoutSuccess = isCheckoutSuccess;
            IsBusted = isBusted;
        }
    }
}
