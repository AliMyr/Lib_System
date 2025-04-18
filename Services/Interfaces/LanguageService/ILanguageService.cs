using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface ILanguageService
    {
        IEnumerable<Language> GetAllLanguages();
        int CreateLanguage(Language lang);
        bool UpdateLanguage(Language lang);
        bool DeleteLanguage(int id);
    }
}
