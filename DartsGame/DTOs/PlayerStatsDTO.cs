namespace DartsGame.DTOs
{
    public class PlayerStatsDTO
    {
        public string PlayerName { get; set; }
        public decimal AveragePPD { get; set; }
        public decimal BestPPD { get; set; }

        public decimal AverageFirst9PPD { get; set; }
        public decimal BestFirst9PPD { get; set; }

        public decimal AverageCheckoutPercentage { get; set; }
        public decimal BestCheckoutPercentage { get; set; }

        public decimal AverageCheckoutPoints { get; set; }
        public int BestCheckoutPoints { get; set; }

        public int LegsWon { get; set; }
        public int TotalLegs { get; set; }
        public decimal WinPercentage { get; set; }

        public int Total60Plus { get; set; }
        public int Total100Plus { get; set; }
        public int Total140Plus { get; set; }
        public int Total180s { get; set; }

        public decimal PerLeg60Plus { get; set; }
        public decimal PerLeg100Plus { get; set; }
        public decimal PerLeg140Plus { get; set; }
        public decimal PerLeg180s { get; set; }
    }
}
