using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class RoleService : IRoleService
    {
        private readonly IDbService _db;
        public RoleService(IDbService db) => _db = db;

        public IEnumerable<RoleViewModel> GetAllRoleDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<RoleViewModel>("SELECT id AS Id, title AS Title FROM MA_roles");
        }

        public Role GetRoleById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<Role>("SELECT id AS Id, title AS Title FROM MA_roles WHERE id=@Id", new { Id = id });
        }

        public int CreateRole(Role role)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(@"INSERT INTO MA_roles (title) VALUES (@Title); SELECT LAST_INSERT_ID();", role);
        }

        public bool UpdateRole(Role role)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("UPDATE MA_roles SET title=@Title WHERE id=@Id", role) > 0;
        }

        public bool DeleteRole(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_roles WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}