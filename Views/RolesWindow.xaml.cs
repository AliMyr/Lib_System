using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RolesWindow : Window
    {
        private readonly IRoleService _svc;
        private readonly CollectionView _view;

        public RolesWindow()
        {
            InitializeComponent();
            _svc = new RoleService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(RolesGrid.ItemsSource);
        }

        private void RefreshGrid() =>
            RolesGrid.ItemsSource = _svc.GetAllRoleDetails().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var r = (RoleViewModel)o;
                return string.IsNullOrEmpty(f) || r.Title.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new RoleEditWindow(_svc).ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RolesGrid.SelectedItem is RoleViewModel vm)
            {
                var model = _svc.GetRoleById(vm.Id);
                if (new RoleEditWindow(_svc, model).ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RolesGrid.SelectedItem is RoleViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRole(vm.Id);
                RefreshGrid();
            }
        }
    }
}
