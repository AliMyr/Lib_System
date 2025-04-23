using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IAuthService _auth;
        private readonly ILogAuditService _auditSvc;
        private readonly IDbService _db;

        public RegisterWindow(
            IAuthService auth,
            ILogAuditService auditSvc,
            IDbService dbService)
        {
            InitializeComponent();
            _auth = auth;
            _auditSvc = auditSvc;
            _db = dbService;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var user = new LogUser
            {
                Username = UsernameBox.Text.Trim(),
                Email = EmailBox.Text.Trim()
            };
            var pwd = PasswordBox.Password.Trim();

            if (_auth.Register(user, pwd))
            {
                _auditSvc.CreateAudit(new LogAudit
                {
                    UserId = user.Id,
                    Action = "Register",
                    CreatedAt = DateTime.Now
                });

                MessageBox.Show("Registration successful.");
                Close();
            }
            else
            {
                MessageBox.Show("Registration failed.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
