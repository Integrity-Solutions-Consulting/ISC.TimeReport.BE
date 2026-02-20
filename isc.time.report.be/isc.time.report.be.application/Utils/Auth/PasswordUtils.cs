namespace isc.time.report.be.application.Utils.Auth
{

    public class PasswordUtils
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static string GenerateSecurePassword(int length = 12)
        {
            var allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[]{}|;:,.<>?";
            var random = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => allChars[random.Next(allChars.Length)]).ToArray());
        }

    }
}
