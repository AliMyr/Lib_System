using System;

namespace Lib_System.Models
{
    public class LogSession
    {
        public ulong Id { get; set; }
        public int? UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
