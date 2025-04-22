using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IReaderService
    {
        IEnumerable<ReaderViewModel> GetAllReaderDetails();
        Reader GetReaderById(int id);
        int CreateReader(Reader reader);
        bool UpdateReader(Reader reader);
        bool DeleteReader(int id);
    }
}