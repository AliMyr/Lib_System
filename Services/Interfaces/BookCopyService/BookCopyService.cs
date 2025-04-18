using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class BookCopyService : IBookCopyService
    {
        private readonly IDbService _db;
        public BookCopyService(IDbService db) => _db = db;

        public IEnumerable<BookCopy> GetAllCopies()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<BookCopy>(
                @"SELECT 
                    id, 
                    book_id AS BookId, 
                    reading_rooms_id AS ReadingRoomsId 
                  FROM MA_book_copies");
        }
    }
}
