using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogAuditWindow : Window
    {
        private readonly ILogAuditService _svc;
        private readonly CollectionView _view;

        public LogAuditWindow()
        {
            InitializeComponent();
            _svc = new LogAuditService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource
                        .GetDefaultView(AuditGrid.ItemsSource);
        }

        private void RefreshGrid()
            => AuditGrid.ItemsSource = _svc
                        .GetAllAuditDetails()
                        .ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var a = (LogAuditViewModel)o;
                return string.IsNullOrEmpty(f)
                    || a.Username.ToLower().Contains(f)
                    || a.Action.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new LogAuditEditWindow(_svc);
            if (win.ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuditGrid.SelectedItem is LogAuditViewModel vm)
            {
                var model = _svc.GetAuditById(vm.Id);
                var win = new LogAuditEditWindow(_svc, model);
                if (win.ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuditGrid.SelectedItem is LogAuditViewModel vm
                && MessageBox.Show("Delete?", "",
                       MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteAudit(vm.Id);
                RefreshGrid();
            }
        }
    }
}
