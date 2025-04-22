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

        public IEnumerable<StaffViewModel> GetAllStaffDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<StaffViewModel>(@"
                SELECT s.id AS Id,
                       CONCAT(s.last_name,' ',s.first_name) AS FullName,
                       r.title AS RoleTitle,
                       s.hired_date AS HiredDate,
                       s.phone AS Phone
                  FROM MA_staff s
                  LEFT JOIN MA_roles r ON s.role_id = r.id");
        }

        public Staff GetStaffById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<Staff>(
                "SELECT id AS Id, last_name AS LastName, first_name AS FirstName, middle_name AS MiddleName, role_id AS RoleId, hired_date AS HiredDate, phone AS Phone FROM MA_staff WHERE id=@Id",
                new { Id = id });
        }

        public int CreateStaff(Staff staff)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(@"
                INSERT INTO MA_staff (last_name,first_name,middle_name,role_id,hired_date,phone)
                VALUES (@LastName,@FirstName,@MiddleName,@RoleId,@HiredDate,@Phone);
                SELECT LAST_INSERT_ID();", staff);
        }

        public bool UpdateStaff(Staff staff)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(@"
                UPDATE MA_staff SET
                  last_name=@LastName,
                  first_name=@FirstName,
                  middle_name=@MiddleName,
                  role_id=@RoleId,
                  hired_date=@HiredDate,
                  phone=@Phone
                WHERE id=@Id", staff) > 0;
        }

        public bool DeleteStaff(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_staff WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}