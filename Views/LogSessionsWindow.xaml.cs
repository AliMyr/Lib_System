using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogSessionsWindow : Window
    {
        private readonly ILogSessionService _svc;
        public LogSessionsWindow()
        {
            InitializeComponent();
            _svc = new LogSessionService(new DbService());
            Load();
        }

        private void Load() => SessionsGrid.ItemsSource = _svc.GetAllSessions().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new LogSessionEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is LogSession s)
            {
                var win = new LogSessionEditWindow(_svc, s);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is LogSession s &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteSession(s.Id);
                Load();
            }
        }
    }
}
