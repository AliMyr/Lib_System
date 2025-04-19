using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<GenreViewModel> GetAllGenreDetails();
        Genre GetGenreById(int id);
        int CreateGenre(Genre genre);
        bool UpdateGenre(Genre genre);
        bool DeleteGenre(int id);
    }
}
