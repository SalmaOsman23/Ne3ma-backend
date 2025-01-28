namespace Neama.Helper
{
    public class CheckEmailorPhoneForResetPassword
    {


        public static bool IsValidEmail(string input)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.Address == input;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^\+?[0-9]\d{1,14}$");
        }
    }
}
