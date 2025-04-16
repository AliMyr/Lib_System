using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IDbService _dbService;
        public AuthorService(IDbService dbService) => _dbService = dbService;

        public IEnumerable<Author> GetAllAuthors()
        {
            using (IDbConnection conn = _dbService.GetConnection())
            {
                conn.Open();
                return conn.Query<Author>(
                    "SELECT id, last_name as LastName, first_name as FirstName, middle_name as MiddleName, pen_name as PenName FROM MA_authors");
            }
        }
    }
}
