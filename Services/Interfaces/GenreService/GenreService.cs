using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class GenreService : IGenreService
    {
        private readonly IDbService _db;
        public GenreService(IDbService db) => _db = db;
        public IEnumerable<Genre> GetAllGenres()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Genre>("SELECT id, title AS Title FROM MA_genres");
        }
    }
}
