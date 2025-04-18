using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IDbService _db;
        public AuthorService(IDbService db) => _db = db;

        public IEnumerable<Author> GetAllAuthors()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Author>(
                "SELECT id AS Id, last_name AS LastName, first_name AS FirstName, middle_name AS MiddleName, pen_name AS PenName FROM MA_authors");
        }

        public int CreateAuthor(Author author)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_authors (last_name, first_name, middle_name, pen_name)
                  VALUES (@LastName,@FirstName,@MiddleName,@PenName);
                  SELECT LAST_INSERT_ID();", author);
        }

        public bool UpdateAuthor(Author author)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_authors 
                  SET last_name=@LastName, first_name=@FirstName, middle_name=@MiddleName, pen_name=@PenName 
                  WHERE id=@Id", author) > 0;
        }

        public bool DeleteAuthor(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_authors WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
