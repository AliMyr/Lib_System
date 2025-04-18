using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class LogAuditService : ILogAuditService
    {
        private readonly IDbService _db;
        public LogAuditService(IDbService db) => _db = db;
        public IEnumerable<LogAudit> GetAllAudits()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<LogAudit>(
                @"SELECT 
                    id AS Id, 
                    user_id AS UserId, 
                    action AS Action, 
                    created_at AS CreatedAt 
                  FROM MA_log_audit");
        }
    }
}
