using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogAuditWindow : Window
    {
        public LogAuditWindow()
        {
            InitializeComponent();
            var svc = new LogAuditService(new DbService());
            AuditGrid.ItemsSource = svc.GetAllAudits();
        }
    }
}
