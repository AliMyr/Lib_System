using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IBookService
    {
        IEnumerable<BookViewModel> GetAllBookDetails();
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        int CreateBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(int id);
    }
}
