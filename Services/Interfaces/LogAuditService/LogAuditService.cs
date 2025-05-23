﻿using System.Collections.Generic;
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

        public IEnumerable<LogAuditViewModel> GetAllAuditDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<LogAuditViewModel>(@"
                SELECT la.id          AS Id,
                       la.user_id     AS UserId,
                       u.username     AS Username,
                       la.action      AS Action,
                       la.created_at  AS CreatedAt
                  FROM MA_log_audit la
                  LEFT JOIN MA_log_users u ON la.user_id = u.id");
        }

        public LogAudit GetAuditById(ulong id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<LogAudit>(@"
                SELECT id AS Id,
                       user_id AS UserId,
                       action AS Action,
                       created_at AS CreatedAt
                  FROM MA_log_audit
                 WHERE id = @Id", new { Id = id });
        }

        public ulong CreateAudit(LogAudit audit)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<ulong>(@"
                INSERT INTO MA_log_audit (user_id,action,created_at)
                VALUES (@UserId,@Action,@CreatedAt);
                SELECT LAST_INSERT_ID();", audit);
        }

        public bool UpdateAudit(LogAudit audit)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(@"
                UPDATE MA_log_audit
                   SET user_id    = @UserId,
                       action     = @Action,
                       created_at = @CreatedAt
                 WHERE id = @Id", audit) > 0;
        }

        public bool DeleteAudit(ulong id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                "DELETE FROM MA_log_audit WHERE id = @Id",
                new { Id = id }) > 0;
        }
    }
}
