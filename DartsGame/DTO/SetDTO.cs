namespace DartsGame.DTO
{
    public class SetDTO
    {
        public Guid SetId { get; set; }
        public Guid MatchId { get; set; }
        public int BestOfLegs { get; set; }
        public int SetNumber { get; set; }
        public bool IsFinished { get; set; }
    }
}
