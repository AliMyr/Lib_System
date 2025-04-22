using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface ILogAuditService
    {
        IEnumerable<LogAuditViewModel> GetAllAuditDetails();
        LogAudit GetAuditById(ulong id);
        ulong CreateAudit(LogAudit audit);
        bool UpdateAudit(LogAudit audit);
        bool DeleteAudit(ulong id);
    }
}
