using System.Security.Cryptography;
using System.Text;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                var sb = new StringBuilder();
                foreach (var b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
