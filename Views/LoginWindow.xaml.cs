using System;
using System.Windows;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _auth;
        private readonly ILogSessionService _sessionSvc;
        private readonly ILogAuditService _auditSvc;
        private readonly IDbService _db;

        public LoginWindow(
            IAuthService auth,
            ILogSessionService sessionSvc,
            ILogAuditService auditSvc,
            IDbService dbService)
        {
            InitializeComponent();
            _auth = auth;
            _sessionSvc = sessionSvc;
            _auditSvc = auditSvc;
            _db = dbService;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameBox.Text.Trim();
            var password = PasswordBox.Password.Trim();

            if (!_auth.Login(username, password))
            {
                MessageBox.Show("Login failed.");
                return;
            }

            // получаем ID только что залогиненного
            var user = _db.GetConnection()
                .QueryFirst<LogUser>(
                    "SELECT id AS Id FROM MA_log_users WHERE username = @Username",
                    new { Username = username });

            // создаём сессию
            _sessionSvc.CreateSession(new LogSession
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddHours(8)
            });

            // записываем в аудит
            _auditSvc.CreateAudit(new LogAudit
            {
                UserId = user.Id,
                Action = "Login",
                CreatedAt = DateTime.Now
            });

            new MainWindow().Show();
            Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // передаём все нужные сервисы, чтобы из окна регистрации сразу писать аудит
            var win = new RegisterWindow(_auth, _auditSvc, _db);
            win.ShowDialog();
        }
    }
}
