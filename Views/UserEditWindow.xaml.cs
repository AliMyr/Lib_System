using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class UserEditWindow : Window
    {
        private readonly ILogUserService _svc;
        public LogUser User { get; private set; }

        public UserEditWindow(ILogUserService svc, LogUser user = null)
        {
            InitializeComponent();
            _svc = svc;
            User = user != null
                ? new LogUser
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
                : new LogUser { CreatedAt = DateTime.Now };
            UsernameBox.Text = User.Username;
            EmailBox.Text = User.Email;
            HashBox.Text = User.PasswordHash;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            User.Username = UsernameBox.Text.Trim();
            User.Email = EmailBox.Text.Trim();
            User.PasswordHash = HashBox.Text.Trim();
            User.UpdatedAt = DateTime.Now;
            if (User.Id == 0) User.Id = _svc.CreateUser(User);
            else _svc.UpdateUser(User);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
