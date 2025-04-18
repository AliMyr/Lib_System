using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IBookLoanService
    {
        IEnumerable<BookLoan> GetAllLoans();
        int CreateLoan(BookLoan loan);
        bool UpdateLoan(BookLoan loan);
        bool DeleteLoan(int id);
    }
}
