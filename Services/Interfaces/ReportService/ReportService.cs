using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class ReportService : IReportService
    {
        private readonly IDbService _db;
        public ReportService(IDbService db) => _db = db;

        public IEnumerable<LoansByMonthViewModel> GetLoansByMonth()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<LoansByMonthViewModel>(@"
                SELECT 
                  DATE_FORMAT(loan_date, '%Y-%m') AS Month,
                  COUNT(*)                      AS LoansCount
                FROM MA_book_loans
                GROUP BY Month
                ORDER BY Month");
        }

        public IEnumerable<PopularBookViewModel> GetMostPopularBooks(int topCount)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<PopularBookViewModel>(@"
                SELECT 
                  b.title        AS Title,
                  COUNT(*)       AS BorrowCount
                FROM MA_books b
                JOIN MA_book_copies bc ON b.id = bc.book_id
                JOIN MA_book_loans  bl ON bc.id = bl.book_copies_id
                GROUP BY b.title
                ORDER BY BorrowCount DESC
                LIMIT @TopCount", new { TopCount = topCount });
        }
    }
}
