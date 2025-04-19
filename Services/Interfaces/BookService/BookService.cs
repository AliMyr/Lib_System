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

        public IEnumerable<BookViewModel> GetAllBookDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<BookViewModel>(@"
                SELECT 
                    b.id               AS Id,
                    b.title            AS Title,
                    p.title            AS PublisherName,
                    g.title            AS GenreTitle,
                    l.title            AS LanguageTitle,
                    b.publication_date AS PublicationDate,
                    b.pages            AS Pages
                FROM MA_books b
                LEFT JOIN MA_publishers p ON b.publisher_id = p.id
                LEFT JOIN MA_genres     g ON b.genre_id     = g.id
                LEFT JOIN MA_languages  l ON b.language_id  = l.id");
        }

        public IEnumerable<Book> GetAllBooks()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Book>(@"
                SELECT 
                    id AS Id, 
                    title AS Title, 
                    publisher_id AS PublisherId, 
                    publication_date AS PublicationDate, 
                    pages AS Pages, 
                    genre_id AS GenreId, 
                    language_id AS LanguageId 
                  FROM MA_books");
        }

        public Book GetBookById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<Book>(@"
                SELECT 
                    id AS Id, 
                    title AS Title, 
                    publisher_id AS PublisherId, 
                    publication_date AS PublicationDate, 
                    pages AS Pages, 
                    genre_id AS GenreId, 
                    language_id AS LanguageId 
                  FROM MA_books
                  WHERE id = @Id", new { Id = id });
        }

        public int CreateBook(Book book)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(@"
                INSERT INTO MA_books 
                    (title, publisher_id, publication_date, pages, genre_id, language_id)
                  VALUES 
                    (@Title,@PublisherId,@PublicationDate,@Pages,@GenreId,@LanguageId);
                  SELECT LAST_INSERT_ID();", book);
        }

        public bool UpdateBook(Book book)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(@"
                UPDATE MA_books SET
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
