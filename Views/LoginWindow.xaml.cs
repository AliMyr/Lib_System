using System.Windows;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _authService;
        public LoginWindow(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (_authService.Login(UsernameBox.Text, PasswordBox.Password))
            {
                new MainWindow().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Login failed.");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var regWindow = new RegisterWindow(_authService);
            regWindow.ShowDialog();
        }
    }
}
