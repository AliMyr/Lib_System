using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IDbService _db;
        public LanguageService(IDbService db) => _db = db;

        public IEnumerable<LanguageViewModel> GetAllLanguageDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<LanguageViewModel>(
                "SELECT id AS Id, title AS Title FROM MA_languages");
        }

        public Language GetLanguageById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<Language>(
                "SELECT id AS Id, title AS Title FROM MA_languages WHERE id = @Id",
                new { Id = id });
        }

        public int CreateLanguage(Language lang)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_languages (title) VALUES (@Title);
                  SELECT LAST_INSERT_ID();", lang);
        }

        public bool UpdateLanguage(Language lang)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                "UPDATE MA_languages SET title=@Title WHERE id=@Id", lang) > 0;
        }

        public bool DeleteLanguage(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                "DELETE FROM MA_languages WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
