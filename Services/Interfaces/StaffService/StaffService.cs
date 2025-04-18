﻿using System.Collections.Generic;
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
                    id AS Id, 
                    last_name AS LastName, 
                    first_name AS FirstName, 
                    middle_name AS MiddleName, 
                    role_id AS RoleId, 
                    hired_date AS HiredDate, 
                    phone AS Phone 
                  FROM MA_staff");
        }

        public int CreateStaff(Staff s)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_staff 
                    (last_name, first_name, middle_name, role_id, hired_date, phone)
                  VALUES 
                    (@LastName,@FirstName,@MiddleName,@RoleId,@HiredDate,@Phone);
                  SELECT LAST_INSERT_ID();", s);
        }

        public bool UpdateStaff(Staff s)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_staff SET
                    last_name=@LastName,
                    first_name=@FirstName,
                    middle_name=@MiddleName,
                    role_id=@RoleId,
                    hired_date=@HiredDate,
                    phone=@Phone
                  WHERE id=@Id", s) > 0;
        }

        public bool DeleteStaff(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_staff WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}