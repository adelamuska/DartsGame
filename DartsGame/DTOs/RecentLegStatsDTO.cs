namespace DartsGame.DTOs
{
    public class RecentLegStatDto
    {
        public decimal PPD { get; set; }
        public decimal First9PPD { get; set; }
        public decimal CheckoutPercentage { get; set; }
        public int CheckoutPoints { get; set; }
        public int Count60Plus { get; set; }
        public int Count100Plus { get; set; }
        public int Count140Plus { get; set; }
        public int Count180s { get; set; }
        public Guid WinnerId { get; set; }
        public int LegNumber { get; set; }
        public Guid PlayerId { get; set; }
    }

}
