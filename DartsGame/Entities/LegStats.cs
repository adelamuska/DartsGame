using System.ComponentModel.DataAnnotations.Schema;

namespace DartsGame.Entities
{
    public class LegStats
    {
        public Guid LegStatsId { get; set; }
        public Guid LegId { get; set; }
        public Guid PlayerId { get; set; }
        public decimal PPD { get; set; } 
        public decimal First9PPD { get; set; }
        public int DartsThrown { get; set; }
        public int CheckoutPoints { get; set; }
        public decimal CheckoutPercentage { get; set; } 
        public int Count60Plus { get; set; }
        public int Count100Plus { get; set; }
        public int Count140Plus { get; set; }
        public int Count180s { get; set; }

        public Leg Leg { get; set; }
        public Player Player { get; set; }
    }
}
