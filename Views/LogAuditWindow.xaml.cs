using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogAuditWindow : Window
    {
        private readonly ILogAuditService _svc;
        public LogAuditWindow()
        {
            InitializeComponent();
            _svc = new LogAuditService(new DbService());
            Load();
        }

        private void Load() => AuditGrid.ItemsSource = _svc.GetAllAudits().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new LogAuditEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuditGrid.SelectedItem is LogAudit a)
            {
                var win = new LogAuditEditWindow(_svc, a);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuditGrid.SelectedItem is LogAudit a &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteAudit(a.Id);
                Load();
            }
        }
    }
}