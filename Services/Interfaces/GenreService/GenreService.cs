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

        public IEnumerable<GenreViewModel> GetAllGenreDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<GenreViewModel>(
                "SELECT id AS Id, title AS Title FROM MA_genres");
        }

        public Genre GetGenreById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<Genre>(
                "SELECT id AS Id, title AS Title FROM MA_genres WHERE id = @Id",
                new { Id = id });
        }

        public int CreateGenre(Genre genre)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_genres (title) VALUES (@Title);
                  SELECT LAST_INSERT_ID();", genre);
        }

        public bool UpdateGenre(Genre genre)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                "UPDATE MA_genres SET title=@Title WHERE id=@Id", genre) > 0;
        }

        public bool DeleteGenre(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_genres WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
