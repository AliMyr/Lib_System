using System;
using System.Windows;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IAuthService _auth;
        private readonly ILogAuditService _audit;
        private readonly IDbService _db;

        public RegisterWindow(
            IAuthService authService,
            ILogAuditService auditService,
            IDbService dbService)
        {
            InitializeComponent();
            _auth = authService;
            _audit = auditService;
            _db = dbService;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var email = EmailBox.Text.Trim();
            var plain = PasswordBox.Password.Trim();

            var user = new LogUser
            {
                Username = username,
                Email = email,
                CreatedAt = DateTime.Now
            };

            if (!_auth.Register(user, plain))
            {
                MessageBox.Show("Registration failed.");
                return;
            }

            var created = _db.GetConnection()
                .QuerySingle<LogUser>(
                    "SELECT id AS Id FROM MA_log_users WHERE username = @Username",
                    new { Username = username });

            _audit.CreateAudit(new LogAudit
            {
                UserId = (int)created.Id,
                Action = "Register",
                CreatedAt = DateTime.Now
            });

            MessageBox.Show("Registration successful.");
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}