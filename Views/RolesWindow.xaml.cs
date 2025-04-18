using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RolesWindow : Window
    {
        public RolesWindow()
        {
            InitializeComponent();
            var svc = new RoleService(new DbService());
            RolesGrid.ItemsSource = svc.GetAllRoles();
        }
    }
}
