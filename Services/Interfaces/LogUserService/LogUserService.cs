using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class LogUserService : ILogUserService
    {
        private readonly IDbService _db;
        public LogUserService(IDbService db) => _db = db;

        public IEnumerable<LogUser> GetAllUsers()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<LogUser>(
                @"SELECT id AS Id, username AS Username, email AS Email, password_hash AS PasswordHash, created_at AS CreatedAt, updated_at AS UpdatedAt 
                  FROM MA_log_users");
        }

        public int CreateUser(LogUser user)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_log_users (username,email,password_hash,created_at) 
              VALUES (@Username,@Email,@PasswordHash,@CreatedAt);
              SELECT LAST_INSERT_ID();", user);
        }

        public bool UpdateUser(LogUser user)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_log_users 
                  SET username=@Username, email=@Email, password_hash=@PasswordHash, updated_at=@UpdatedAt 
                  WHERE id=@Id", user) > 0;
        }

        public bool DeleteUser(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_log_users WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
