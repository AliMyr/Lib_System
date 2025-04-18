using System;

namespace Lib_System.Models
{
    public class LogAudit
    {
        public ulong Id { get; set; }
        public int? UserId { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
