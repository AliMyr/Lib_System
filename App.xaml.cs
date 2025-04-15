using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;
using Lib_System.Views;

namespace Lib_System
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IDbService dbService = new DbService();
            IPasswordHasher hasher = new PasswordHasher();
            IAuthService authService = new AuthService(dbService, hasher);
            new LoginWindow(authService).Show();
        }
    }
}
