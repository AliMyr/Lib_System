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
                "SELECT id AS Id, book_id AS BookId, reading_rooms_id AS ReadingRoomsId FROM MA_book_copies");
        }

        public int CreateCopy(BookCopy copy)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_book_copies (book_id, reading_rooms_id) 
                  VALUES (@BookId, @ReadingRoomsId);
                  SELECT LAST_INSERT_ID();", copy);
        }

        public bool UpdateCopy(BookCopy copy)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_book_copies 
                  SET book_id=@BookId, reading_rooms_id=@ReadingRoomsId 
                  WHERE id=@Id", copy) > 0;
        }

        public bool DeleteCopy(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_book_copies WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
