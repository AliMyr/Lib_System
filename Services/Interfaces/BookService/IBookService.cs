using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        int CreateBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(int id);
    }
}
