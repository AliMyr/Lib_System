using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RoleEditWindow : Window
    {
        private readonly IRoleService _svc;
        public Role Role { get; private set; }

        public RoleEditWindow(IRoleService svc, Role role = null)
        {
            InitializeComponent();
            _svc = svc;
            Role = role != null
                ? new Role { Id = role.Id, Title = role.Title }
                : new Role();
            TitleBox.Text = Role.Title;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Role.Title = TitleBox.Text.Trim();
            if (Role.Id == 0)
                Role.Id = _svc.CreateRole(Role);
            else
                _svc.UpdateRole(Role);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
