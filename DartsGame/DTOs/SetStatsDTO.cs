using DartsGame.DTO;
using static DartsGame.DTOs.SetStatsDTO;

namespace DartsGame.DTOs
{
    public class SetStatsDTO
    {

        public Guid SetId { get; set; }
        public int LegsWon { get; set; }
        public decimal PPD { get; set; }
        public decimal First9PPD { get; set; }
        public decimal CheckoutPercentage { get; set; }
        public decimal BestCheckoutPoints { get; set; }
        public int Count60Plus { get; set; }
        public int Count100Plus { get; set; }
        public int Count140Plus { get; set; }
        public int Count180s { get; set; }
        public List<LegStatsDTO> Legs { get; set; }


    }
}
