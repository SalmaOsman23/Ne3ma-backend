using System.Text.RegularExpressions;

namespace Neama.Helper
{
    public static class CheckEmail
    {
        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
