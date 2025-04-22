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

        public IEnumerable<AuthorBookViewModel> GetAllRelationsDetailed()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<AuthorBookViewModel>(@"
                SELECT 
                  ab.id AS Id,
                  CONCAT(a.last_name,' ',a.first_name) AS AuthorName,
                  b.title AS BookTitle
                FROM MA_author_book ab
                JOIN MA_authors a ON ab.author_id = a.id
                JOIN MA_books   b ON ab.book_id   = b.id");
        }

        public AuthorBook GetRelationById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<AuthorBook>(
                "SELECT id AS Id, author_id AS AuthorId, book_id AS BookId FROM MA_author_book WHERE id = @Id",
                new { Id = id });
        }

        public int CreateRelation(AuthorBook rel)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_author_book (author_id,book_id) VALUES (@AuthorId,@BookId);
                  SELECT LAST_INSERT_ID();", rel);
        }

        public bool UpdateRelation(AuthorBook rel)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                "UPDATE MA_author_book SET author_id=@AuthorId, book_id=@BookId WHERE id=@Id", rel) > 0;
        }

        public bool DeleteRelation(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_author_book WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
