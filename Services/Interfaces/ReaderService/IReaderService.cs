using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IReaderService
    {
        IEnumerable<ReaderViewModel> GetAllReaderDetails();
        IEnumerable<Reader> GetAllReaders();
        Reader GetReaderById(int id);
        int CreateReader(Reader r);
        bool UpdateReader(Reader r);
        bool DeleteReader(int id);
    }
}
