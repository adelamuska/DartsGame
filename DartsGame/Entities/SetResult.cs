namespace DartsGame.Entities
{
    public class SetResult
    {
        public Guid SetId { get; set; }
        public Guid PlayerId { get; set; }
        public int LegsWon { get; set; }
        
        public Player Player { get; set; }
        public Set Set { get; set; }    

    }
}
