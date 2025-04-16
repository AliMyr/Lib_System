using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDbService _dbService;
        private readonly IPasswordHasher _hasher;

        public AuthService(IDbService dbService, IPasswordHasher hasher)
        {
            _dbService = dbService;
            _hasher = hasher;
        }

        public bool Register(LogUser user, string plainPassword)
        {
            using (IDbConnection conn = _dbService.GetConnection())
            {
                conn.Open();
                var existing = conn.QueryFirstOrDefault<LogUser>(
                    "SELECT id, username, email, password_hash as PasswordHash FROM MA_log_users WHERE username = @Username OR email = @Email",
                    new { user.Username, user.Email });
                if (existing != null) return false;
                user.PasswordHash = _hasher.Hash(plainPassword);
                conn.Execute("INSERT INTO MA_log_users (username, email, password_hash) VALUES (@Username, @Email, @PasswordHash)", user);
                return true;
            }
        }

        public bool Login(string username, string plainPassword)
        {
            using (IDbConnection conn = _dbService.GetConnection())
            {
                conn.Open();
                var user = conn.QueryFirstOrDefault<LogUser>(
                    "SELECT id, username, email, password_hash as PasswordHash FROM MA_log_users WHERE username = @Username",
                    new { Username = username });
                if (user == null) return false;
                return user.PasswordHash == _hasher.Hash(plainPassword);
            }
        }
    }
}
