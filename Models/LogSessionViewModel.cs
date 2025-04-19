using System;

namespace Lib_System.Models
{
    public class LogSessionViewModel
    {
        public ulong Id { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
