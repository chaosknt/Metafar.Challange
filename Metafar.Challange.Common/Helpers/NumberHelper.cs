namespace Metafar.Challange.Common.Helpers
{
    public static class RandomNumberGenerator
    {
        private static readonly Random random = new Random();

        public static decimal GenerateRandomDecimal(decimal minValue, decimal maxValue)
        {
            decimal range = maxValue - minValue;
            decimal randomValue = (decimal)random.NextDouble();
            return minValue + (range * randomValue);
        }

        public static int GenerateRandomNumber(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }
    }
}
