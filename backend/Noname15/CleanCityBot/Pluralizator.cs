namespace CleanCityBot
{
    public static class Pluralizator
    {
        public static (int Count, string Caption) Pluralize(int count, string zeroWord, string oneWord, string twoWord)
        {
            if (count % 10 == 1)
                return (count, oneWord);
            if (count % 10 == 0 || (10 <= count && count < 20))
                return (count, zeroWord);
            return (count, twoWord);
        }
    }
}