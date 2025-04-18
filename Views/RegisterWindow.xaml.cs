// RegisterWindow.xaml.cs
using System;
using System.Windows;
using Dapper;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IAuthService _auth;
        private readonly ILogAuditService _au;
        private readonly IDbService _db;

        public RegisterWindow(
            IAuthService auth,
            ILogAuditService auditSvc,
            IDbService dbService)
        {
            InitializeComponent();
            _auth = auth;
            _au = auditSvc;
            _db = dbService;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var user = new LogUser
            {
                Username = UsernameBox.Text.Trim(),
                Email = EmailBox.Text.Trim(),
                CreatedAt = DateTime.Now
            };
            if (!_auth.Register(user, PasswordBox.Password.Trim()))
            {
                MessageBox.Show("Registration failed.");
                return;
            }

            var u = _db.GetConnection()
                .QueryFirst<LogUser>(
                    "SELECT id AS Id FROM MA_log_users WHERE username=@Username",
                    new { user.Username });

            _au.CreateAudit(new LogAudit
            {
                UserId = (int)u.Id,
                Action = "Register",
                CreatedAt = DateTime.Now
            });

            MessageBox.Show("Registration successful.");
            Close();
        }
    }
}
