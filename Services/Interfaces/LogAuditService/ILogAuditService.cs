﻿using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface ILogAuditService
    {
        IEnumerable<LogAudit> GetAllAudits();
    }
}
