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
                @"SELECT 
                    id, 
                    reader_id AS ReaderId, 
                    book_copies_id AS BookCopiesId, 
                    loan_date AS LoanDate, 
                    return_date AS ReturnDate 
                  FROM MA_book_loans");
        }
    }
}
