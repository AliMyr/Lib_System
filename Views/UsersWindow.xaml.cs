using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class UsersWindow : Window
    {
        private readonly ILogUserService _svc;
        private readonly System.Collections.Generic.IList<LogUserViewModel> _list;
        private readonly CollectionView _view;

        public UsersWindow()
        {
            InitializeComponent();
            _svc = new LogUserService(new DbService());
            _list = _svc.GetAllUserDetails().ToList();
            UsersGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var users = _list
                .Select(x => x.Username)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            users.Insert(0, string.Empty);
            UsernameFilterBox.ItemsSource = users;

            var emails = _list
                .Select(x => x.Email)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            emails.Insert(0, string.Empty);
            EmailFilterBox.ItemsSource = emails;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fu = (UsernameFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fe = (EmailFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var u = (LogUserViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || u.Username.ToLower().Contains(s)
                    || u.Email.ToLower().Contains(s);
                var byUser = string.IsNullOrEmpty(fu) || u.Username.ToLower() == fu;
                var byEmail = string.IsNullOrEmpty(fe) || u.Email.ToLower() == fe;
                return bySearch && byUser && byEmail;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new UserEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is LogUserViewModel vm)
            {
                var model = _svc.GetUserById(vm.Id);
                if (new UserEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is LogUserViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteUser(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllUserDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
