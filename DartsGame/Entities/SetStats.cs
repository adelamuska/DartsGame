namespace DartsGame.Entities
{
    public class SetStats
    {
        public Guid SetStatsId { get; set; }
        public Guid SetId { get; set; }
        public Guid PlayerId { get; set; }
        public int LegsWin { get; set; }
        public decimal PPD { get; set; }
        public decimal First9PPD { get; set; }
        public decimal CheckoutPercentage { get; set; }
        public decimal BestCheckoutPoints { get; set; }

        public int Count60Plus { get; set; }
        public int Count100Plus { get; set; }
        public int Count140Plus { get; set; }
        public int Count180s { get; set; }

        public Set Set { get; set; }
        public Player Player { get; set; }
    }
}
