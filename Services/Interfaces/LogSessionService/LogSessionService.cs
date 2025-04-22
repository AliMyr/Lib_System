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

        public IEnumerable<LogSessionViewModel> GetAllSessionDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<LogSessionViewModel>(@"
                SELECT ls.id          AS Id,
                       ls.user_id     AS UserId,
                       u.username     AS Username,
                       ls.token       AS Token,
                       ls.created_at  AS CreatedAt,
                       ls.expires_at  AS ExpiresAt
                  FROM MA_log_sessions ls
                  LEFT JOIN MA_log_users u ON ls.user_id = u.id");
        }

        public LogSession GetSessionById(ulong id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<LogSession>(@"
                SELECT id AS Id,
                       user_id AS UserId,
                       token AS Token,
                       created_at AS CreatedAt,
                       expires_at AS ExpiresAt
                  FROM MA_log_sessions
                 WHERE id = @Id", new { Id = id });
        }

        public ulong CreateSession(LogSession session)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<ulong>(@"
                INSERT INTO MA_log_sessions (user_id,token,created_at,expires_at)
                VALUES (@UserId,@Token,@CreatedAt,@ExpiresAt);
                SELECT LAST_INSERT_ID();", session);
        }

        public bool UpdateSession(LogSession session)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(@"
                UPDATE MA_log_sessions
                   SET user_id    = @UserId,
                       token      = @Token,
                       created_at = @CreatedAt,
                       expires_at = @ExpiresAt
                 WHERE id = @Id", session) > 0;
        }

        public bool DeleteSession(ulong id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                "DELETE FROM MA_log_sessions WHERE id = @Id",
                new { Id = id }) > 0;
        }
    }
}
