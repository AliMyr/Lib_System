using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RolesWindow : Window
    {
        private readonly IRoleService _svc;
        public RolesWindow()
        {
            InitializeComponent();
            _svc = new RoleService(new DbService());
            Load();
        }

        private void Load()
            => RolesGrid.ItemsSource = _svc.GetAllRoles().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new RoleEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RolesGrid.SelectedItem is Role r)
            {
                var win = new RoleEditWindow(_svc, r);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RolesGrid.SelectedItem is Role r &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRole(r.Id);
                Load();
            }
        }
    }
}
