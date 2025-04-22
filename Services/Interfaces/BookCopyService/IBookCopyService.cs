using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IBookCopyService
    {
        IEnumerable<BookCopyViewModel> GetAllCopyDetails();
        BookCopy GetCopyById(int id);
        int CreateCopy(BookCopy copy);
        bool UpdateCopy(BookCopy copy);
        bool DeleteCopy(int id);
    }
}
