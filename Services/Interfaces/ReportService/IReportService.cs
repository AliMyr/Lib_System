using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IReportService
    {
        IEnumerable<LoansByMonthViewModel> GetLoansByMonth();
        IEnumerable<PopularBookViewModel> GetMostPopularBooks(int topCount);
    }
}