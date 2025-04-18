using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class LogSessionService : ILogSessionService
    {
        private readonly IDbService _db;
        public LogSessionService(IDbService db) => _db = db;

        public IEnumerable<LogSession> GetAllSessions()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<LogSession>("SELECT id AS Id, user_id AS UserId, token AS Token, created_at AS CreatedAt, expires_at AS ExpiresAt FROM MA_log_sessions");
        }

        public ulong CreateSession(LogSession s)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<ulong>(
                @"INSERT INTO MA_log_sessions (user_id, token, created_at, expires_at) 
                  VALUES (@UserId,@Token,@CreatedAt,@ExpiresAt);
                  SELECT LAST_INSERT_ID();", s);
        }

        public bool UpdateSession(LogSession s)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_log_sessions 
                  SET user_id=@UserId, token=@Token, created_at=@CreatedAt, expires_at=@ExpiresAt 
                  WHERE id=@Id", s) > 0;
        }

        public bool DeleteSession(ulong id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_log_sessions WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
