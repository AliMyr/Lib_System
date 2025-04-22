using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface ILogSessionService
    {
        IEnumerable<LogSessionViewModel> GetAllSessionDetails();
        LogSession GetSessionById(ulong id);
        ulong CreateSession(LogSession session);
        bool UpdateSession(LogSession session);
        bool DeleteSession(ulong id);
    }
}
