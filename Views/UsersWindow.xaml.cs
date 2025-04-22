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
        private readonly CollectionView _view;

        public UsersWindow()
        {
            InitializeComponent();
            _svc = new LogUserService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(UsersGrid.ItemsSource);
        }

        private void RefreshGrid() =>
            UsersGrid.ItemsSource = _svc.GetAllUserDetails().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var u = (LogUserViewModel)o;
                return string.IsNullOrEmpty(f)
                    || u.Username.ToLower().Contains(f)
                    || u.Email.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new UserEditWindow(_svc).ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is LogUserViewModel vm)
            {
                var model = _svc.GetUserById(vm.Id);
                if (new UserEditWindow(_svc, model).ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is LogUserViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteUser(vm.Id);
                RefreshGrid();
            }
        }
    }
}
