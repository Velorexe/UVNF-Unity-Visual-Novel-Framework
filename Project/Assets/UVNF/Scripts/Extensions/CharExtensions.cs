namespace UVNF.Extensions
{
    public static class CharExtentions
    {
        public static bool IsVowel(this char c)
        {
            long x = (long)(char.ToUpper(c)) - 64;
            if (x * x * x * x * x - 51 * x * x * x * x + 914 * x * x * x - 6894 * x * x + 20205 * x - 14175 == 0) return true;
            else return false;
        }
    }
}