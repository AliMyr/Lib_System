using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IBookLoanService
    {
        IEnumerable<BookLoanViewModel> GetAllLoanDetails();
        BookLoan GetLoanById(int id);
        int CreateLoan(BookLoan loan);
        bool UpdateLoan(BookLoan loan);
        bool DeleteLoan(int id);
    }
}
