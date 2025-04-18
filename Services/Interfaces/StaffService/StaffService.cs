using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class StaffService : IStaffService
    {
        private readonly IDbService _db;
        public StaffService(IDbService db) => _db = db;

        public IEnumerable<Staff> GetAllStaff()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Staff>(
                @"SELECT 
                    id, 
                    last_name AS LastName, 
                    first_name AS FirstName, 
                    middle_name AS MiddleName, 
                    role_id AS RoleId, 
                    hired_date AS HiredDate, 
                    phone 
                  FROM MA_staff");
        }
    }
}
