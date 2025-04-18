using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface ILogSessionService
    {
        IEnumerable<LogSession> GetAllSessions();
        ulong CreateSession(LogSession session);
        bool UpdateSession(LogSession session);
        bool DeleteSession(ulong id);
    }
}
