using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class AuthorBookService : IAuthorBookService
    {
        private readonly IDbService _db;
        public AuthorBookService(IDbService db) => _db = db;

        public IEnumerable<AuthorBook> GetAllRelations()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<AuthorBook>(
                "SELECT id, author_id AS AuthorId, book_id AS BookId FROM MA_author_book");
        }
    }
}
