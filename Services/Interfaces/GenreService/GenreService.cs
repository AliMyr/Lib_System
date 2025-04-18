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
            return c.Query<Genre>("SELECT id AS Id, title AS Title FROM MA_genres");
        }

        public int CreateGenre(Genre g)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_genres (title)
                  VALUES (@Title);
                  SELECT LAST_INSERT_ID();", g);
        }

        public bool UpdateGenre(Genre g)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                "UPDATE MA_genres SET title=@Title WHERE id=@Id", g) > 0;
        }

        public bool DeleteGenre(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_genres WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}