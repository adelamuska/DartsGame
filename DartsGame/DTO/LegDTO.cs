namespace DartsGame.DTO
{
    public class LegDTO
    {
        public Guid LegId { get; set; }
        public Guid SetId { get; set; }
        public int LegNumber { get; set; }
        public bool IsFinished { get; set; }
        public Guid? WinnerId { get; set; }
    }
}
