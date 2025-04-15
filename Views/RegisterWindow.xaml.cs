using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IAuthService _authService;
        public RegisterWindow(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var user = new LogUser
            {
                Username = UsernameBox.Text,
                Email = EmailBox.Text
            };
            if (_authService.Register(user, PasswordBox.Password))
            {
                MessageBox.Show("Registration successful.");
                Close();
            }
            else
            {
                MessageBox.Show("Registration failed.");
            }
        }
    }
}
