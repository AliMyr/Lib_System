using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogSessionsWindow : Window
    {
        private readonly ILogSessionService _svc;
        private readonly CollectionView _view;

        public LogSessionsWindow()
        {
            InitializeComponent();
            _svc = new LogSessionService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource
                        .GetDefaultView(SessionsGrid.ItemsSource);
        }

        private void RefreshGrid()
            => SessionsGrid.ItemsSource = _svc
                        .GetAllSessionDetails()
                        .ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var s = (LogSessionViewModel)o;
                return string.IsNullOrEmpty(f)
                    || s.Username.ToLower().Contains(f)
                    || s.Token.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new LogSessionEditWindow(_svc);
            if (win.ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is LogSessionViewModel vm)
            {
                var model = _svc.GetSessionById(vm.Id);
                var win = new LogSessionEditWindow(_svc, model);
                if (win.ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is LogSessionViewModel vm
                && MessageBox.Show("Delete?", "",
                       MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteSession(vm.Id);
                RefreshGrid();
            }
        }
    }
}
