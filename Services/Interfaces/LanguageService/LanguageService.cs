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
        public IEnumerable<Language> GetAllLanguages()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Language>("SELECT id, title AS Title FROM MA_languages");
        }
    }
}