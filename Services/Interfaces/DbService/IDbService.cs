using System.Data;

namespace Lib_System.Services.Interfaces
{
    public interface IDbService
    {
        IDbConnection GetConnection();
    }
}
