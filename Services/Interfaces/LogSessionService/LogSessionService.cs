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
            return c.Query<LogSession>(
                @"SELECT 
                    id AS Id, 
                    user_id AS UserId, 
                    token AS Token, 
                    created_at AS CreatedAt, 
                    expires_at AS ExpiresAt 
                  FROM MA_log_sessions");
        }
    }
}
