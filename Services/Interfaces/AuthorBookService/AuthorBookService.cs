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
                "SELECT id AS Id, author_id AS AuthorId, book_id AS BookId FROM MA_author_book");
        }

        public int CreateRelation(AuthorBook rel)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_author_book (author_id, book_id)
                  VALUES (@AuthorId,@BookId);
                  SELECT LAST_INSERT_ID();", rel);
        }

        public bool UpdateRelation(AuthorBook rel)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_author_book
                  SET author_id=@AuthorId, book_id=@BookId
                  WHERE id=@Id", rel) > 0;
        }

        public bool DeleteRelation(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_author_book WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
