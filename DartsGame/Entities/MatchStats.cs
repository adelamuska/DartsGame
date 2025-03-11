
namespace DartsGame.Entities
{
    public class MatchStats
    {
        public Guid MatchStatsId { get; set; }
        public Guid MatchId { get; set; }
        public Guid PlayerId { get; set; }
        public int SetsWin { get; set; }
        public int LegsWin { get; set; }
        public decimal PPD { get; set; }
        public decimal First9PPD { get; set; }
        public decimal CheckoutPercentage { get; set; }
        public decimal BestCheckoutPoints { get; set; }

        public int Count60Plus { get; set; }
        public int Count100Plus { get; set; }
        public int Count140Plus { get; set; }
        public int Count180s { get; set; }


        public Match Match { get; set; }
        public Player Player { get; set; }
    }
}
