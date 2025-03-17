using DartsGame.DTO;

namespace DartsGame.DTOs
{
    public class LegStatsDTO
    {
        public Guid LegId { get; set; }
        public int DartsThrown { get; set; }
        public decimal PPD { get; set; }
        public decimal First9PPD { get; set; }
        public int CheckoutPoints { get; set; }
        public decimal CheckoutPercentage { get; set; }
        public int Count60Plus { get; set; }
        public int Count100Plus { get; set; }
        public int Count140Plus { get; set; }
        public int Count180s { get; set; }
        public List<TurnThrowDTO> TurnThrows { get; set; }
    }
}
