namespace DartsGame.Helpers
{
    public static class DartsRulesHelper
    {
        public static bool IsBust(int remainingScore, bool isZero, string lastThrow)
        {
            if (remainingScore < 0)
                return true;

            if (remainingScore == 1)
                return true;

            if (isZero && !IsValidCheckout(lastThrow))
                return true;

            return false;
        }

        public static bool IsValidCheckout(string lastThrow)
        {
            return lastThrow != null && lastThrow.StartsWith("D") || lastThrow.Equals("Bull", StringComparison.OrdinalIgnoreCase);
        }
        public static bool CanCheckout(int score)
        {
            if (score < 170 && score != 169 && score != 168 && score != 166 &&
                score != 165 && score != 163 && score != 162 && score != 159)
            {
                return false;
            }
            return true;
        }
    }
}
