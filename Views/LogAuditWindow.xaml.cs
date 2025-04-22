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
        private readonly System.Collections.Generic.IList<LogAuditViewModel> _list;
        private readonly CollectionView _view;

        public LogAuditWindow()
        {
            InitializeComponent();
            _svc = new LogAuditService(new DbService());
            _list = _svc.GetAllAuditDetails().ToList();
            AuditGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var users = _list
                .Select(x => x.Username)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            users.Insert(0, string.Empty);
            UserFilterBox.ItemsSource = users;

            var actions = _list
                .Select(x => x.Action)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            actions.Insert(0, string.Empty);
            ActionFilterBox.ItemsSource = actions;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fu = (UserFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fa = (ActionFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var vm = (LogAuditViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.Username.ToLower().Contains(s)
                    || vm.Action.ToLower().Contains(s);
                var byUser = string.IsNullOrEmpty(fu)
                    || vm.Username.ToLower() == fu;
                var byAction = string.IsNullOrEmpty(fa)
                    || vm.Action.ToLower() == fa;
                return bySearch && byUser && byAction;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new LogAuditEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuditGrid.SelectedItem is LogAuditViewModel vm)
            {
                var model = _svc.GetAuditById(vm.Id);
                if (new LogAuditEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuditGrid.SelectedItem is LogAuditViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteAudit(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllAuditDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
