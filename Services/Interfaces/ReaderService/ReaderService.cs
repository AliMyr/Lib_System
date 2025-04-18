using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IDbService _db;
        public ReaderService(IDbService db) => _db = db;

        public IEnumerable<Reader> GetAllReaders()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Reader>(
                @"SELECT id AS Id, last_name AS LastName, first_name AS FirstName, 
                     middle_name AS MiddleName, phone AS Phone, 
                     address AS Address, registration_date AS RegistrationDate 
              FROM MA_readers");
        }

        public int CreateReader(Reader reader)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_readers 
                (last_name, first_name, middle_name, phone, address, registration_date)
              VALUES (@LastName,@FirstName,@MiddleName,@Phone,@Address,@RegistrationDate);
              SELECT LAST_INSERT_ID();", reader);
        }

        public bool UpdateReader(Reader reader)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_readers SET
                last_name=@LastName,
                first_name=@FirstName,
                middle_name=@MiddleName,
                phone=@Phone,
                address=@Address,
                registration_date=@RegistrationDate
              WHERE id=@Id", reader) > 0;
        }

        public bool DeleteReader(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_readers WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
