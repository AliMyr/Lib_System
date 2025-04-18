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
            IDbService db = new DbService();
            IPasswordHasher hasher = new PasswordHasher();
            IAuthService auth = new AuthService(db, hasher);
            ILogSessionService ss = new LogSessionService(db);
            ILogAuditService au = new LogAuditService(db);

            new LoginWindow(auth, ss, au, db).Show();
        }
    }
}
