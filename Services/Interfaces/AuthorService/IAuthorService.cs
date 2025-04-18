using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IAuthorService
    {
        IEnumerable<Author> GetAllAuthors();
        int CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(int id);
    }
}
