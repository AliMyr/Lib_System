using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IAuthService
    {
        bool Register(LogUser user, string plainPassword);
        bool Login(string username, string plainPassword);
    }
}
