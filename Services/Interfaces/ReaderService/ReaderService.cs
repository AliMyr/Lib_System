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
                @"SELECT 
                    id, 
                    last_name AS LastName, 
                    first_name AS FirstName, 
                    middle_name AS MiddleName, 
                    phone, 
                    address, 
                    registration_date AS RegistrationDate 
                  FROM MA_readers");
        }
    }
}
