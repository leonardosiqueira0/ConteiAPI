using System.Text;
using System.Security.Cryptography;

namespace ConteiAPI.Helpers
{
    public class Sha256Hasher
    {
        public static string HashPassword(string password)
        {
            if (password == "")
            {
                return "";
            }
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
