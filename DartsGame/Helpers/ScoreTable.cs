namespace DartsGame.Helpers
{
    public static class ScoreTable
    {
        public static int GetScore(string dartThrow) =>
            dartThrow.ToUpper() switch
            {
                "BULL" => 50,
                "OBULL" => 25,
                "MISS" => 0,
                _ => dartThrow.Length > 1 && int.TryParse(dartThrow[1..], out int baseScore) && baseScore >= 1 && baseScore <= 20 ?
                baseScore * (dartThrow[0] == 'T' ? 3 : dartThrow[0] == 'D' ? 2 : 1) : 0
            };

    }
}

