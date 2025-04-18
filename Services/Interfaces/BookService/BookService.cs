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
                    id, 
                    title, 
                    publisher_id AS PublisherId, 
                    publication_date AS PublicationDate, 
                    pages, 
                    genre_id AS GenreId, 
                    language_id AS LanguageId 
                  FROM MA_books");
        }
    }
}
