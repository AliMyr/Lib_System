using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class BookLoanService : IBookLoanService
    {
        private readonly IDbService _db;
        public BookLoanService(IDbService db) => _db = db;

        public IEnumerable<BookLoan> GetAllLoans()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<BookLoan>(
                @"SELECT id AS Id, reader_id AS ReaderId, book_copies_id AS BookCopiesId,
                         loan_date AS LoanDate, return_date AS ReturnDate
                  FROM MA_book_loans");
        }

        public int CreateLoan(BookLoan loan)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_book_loans (reader_id, book_copies_id, loan_date, return_date)
                  VALUES (@ReaderId,@BookCopiesId,@LoanDate,@ReturnDate);
                  SELECT LAST_INSERT_ID();", loan);
        }

        public bool UpdateLoan(BookLoan loan)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_book_loans
                  SET reader_id=@ReaderId, book_copies_id=@BookCopiesId, loan_date=@LoanDate, return_date=@ReturnDate
                  WHERE id=@Id", loan) > 0;
        }

        public bool DeleteLoan(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_book_loans WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
