using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RoleEditWindow : Window
    {
        private readonly IRoleService _svc;
        private readonly Role _model;

        public RoleEditWindow(IRoleService svc, Role model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Role();
            TitleBox.Text = _model.Title;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Title = TitleBox.Text.Trim();
            if (_model.Id == 0)
                _model.Id = _svc.CreateRole(_model);
            else
                _svc.UpdateRole(_model);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
