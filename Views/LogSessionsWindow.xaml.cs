using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogSessionsWindow : Window
    {
        public LogSessionsWindow()
        {
            InitializeComponent();
            var svc = new LogSessionService(new DbService());
            SessionsGrid.ItemsSource = svc.GetAllSessions();
        }
    }
}
