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

        public IEnumerable<Role> GetAllRoles()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Role>("SELECT id, title AS Title FROM MA_roles");
        }
    }
}
