using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;
using Dapper;
using Lib_System.Services;

namespace Lib_System.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _auth;
        private readonly ILogSessionService _ss;
        private readonly ILogAuditService _au;
        private readonly IDbService _db;

        public LoginWindow(
            IAuthService auth,
            ILogSessionService sessionSvc,
            ILogAuditService auditSvc,
            IDbService dbService)
        {
            InitializeComponent();
            _auth = auth;
            _ss = sessionSvc;
            _au = auditSvc;
            _db = dbService;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var u = UsernameBox.Text.Trim();
            var p = PasswordBox.Password.Trim();
            if (!_auth.Login(u, p)) { MessageBox.Show("Login failed."); return; }

            var user = _db.GetConnection()
                .QueryFirst<LogUser>(
                    "SELECT id AS Id FROM MA_log_users WHERE username=@u",
                    new { u });

            _ss.CreateSession(new LogSession
            {
                UserId = (int)user.Id,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddHours(8)
            });

            _au.CreateAudit(new LogAudit
            {
                UserId = (int)user.Id,
                Action = "Login",
                CreatedAt = DateTime.Now
            });

            new MainWindow().Show();
            Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var win = new RegisterWindow(_auth, _au, _db);
            win.ShowDialog();
        }
    }
}

