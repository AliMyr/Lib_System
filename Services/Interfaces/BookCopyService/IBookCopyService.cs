using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IBookCopyService
    {
        IEnumerable<BookCopy> GetAllCopies();
        int CreateCopy(BookCopy copy);
        bool UpdateCopy(BookCopy copy);
        bool DeleteCopy(int id);
    }
}
