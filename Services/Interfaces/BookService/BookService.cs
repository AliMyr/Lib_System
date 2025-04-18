using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class BookService : IBookService
    {
        private readonly IDbService _db;
        public BookService(IDbService db) => _db = db;

        public IEnumerable<Book> GetAllBooks()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Book>(
                @"SELECT 
                    id AS Id, 
                    title AS Title, 
                    publisher_id AS PublisherId, 
                    publication_date AS PublicationDate, 
                    pages AS Pages, 
                    genre_id AS GenreId, 
                    language_id AS LanguageId 
                  FROM MA_books");
        }

        public int CreateBook(Book book)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_books 
                    (title, publisher_id, publication_date, pages, genre_id, language_id)
                  VALUES 
                    (@Title,@PublisherId,@PublicationDate,@Pages,@GenreId,@LanguageId);
                  SELECT LAST_INSERT_ID();", book);
        }

        public bool UpdateBook(Book book)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_books SET
                    title=@Title,
                    publisher_id=@PublisherId,
                    publication_date=@PublicationDate,
                    pages=@Pages,
                    genre_id=@GenreId,
                    language_id=@LanguageId
                  WHERE id=@Id", book) > 0;
        }

        public bool DeleteBook(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_books WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
