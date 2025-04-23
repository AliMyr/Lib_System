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
        private readonly System.Collections.Generic.IList<LogSessionViewModel> _list;
        private readonly CollectionView _view;

        public LogSessionsWindow()
        {
            InitializeComponent();
            _svc = new LogSessionService(new DbService());
            _list = _svc.GetAllSessionDetails().ToList();
            SessionsGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var users = _list
                .Select(x => x.Username)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            users.Insert(0, string.Empty);
            UserFilterBox.ItemsSource = users;

            var tokens = _list
                .Select(x => x.Token)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            tokens.Insert(0, string.Empty);
            TokenFilterBox.ItemsSource = tokens;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fu = (UserFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var ft = (TokenFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var vm = (LogSessionViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.Username.ToLower().Contains(s)
                    || vm.Token.ToLower().Contains(s);
                var byUser = string.IsNullOrEmpty(fu)
                    || vm.Username.ToLower() == fu;
                var byToken = string.IsNullOrEmpty(ft)
                    || vm.Token.ToLower() == ft;
                return bySearch && byUser && byToken;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new LogSessionEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is LogSessionViewModel vm)
            {
                var model = _svc.GetSessionById(vm.Id);
                if (new LogSessionEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SessionsGrid.SelectedItem is LogSessionViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteSession(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllSessionDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
