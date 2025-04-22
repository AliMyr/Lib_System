using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class StaffEditWindow : Window
    {
        private readonly IStaffService _svc;
        private readonly IRoleService _roleSvc = new RoleService(new DbService());
        private readonly Staff _model;

        public StaffEditWindow(IStaffService svc, Staff model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Staff();

            LastBox.Text = _model.LastName;
            FirstBox.Text = _model.FirstName;
            MidBox.Text = _model.MiddleName;
            PhoneBox.Text = _model.Phone;
            DatePicker.SelectedDate = _model.HiredDate;

            var roles = _roleSvc.GetAllRoleDetails().ToList();
            RoleBox.ItemsSource = roles;
            if (_model.RoleId.HasValue)
                RoleBox.SelectedValue = _model.RoleId.Value;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.LastName = LastBox.Text.Trim();
            _model.FirstName = FirstBox.Text.Trim();
            _model.MiddleName = MidBox.Text.Trim();
            _model.Phone = PhoneBox.Text.Trim();
            _model.HiredDate = DatePicker.SelectedDate;
            _model.RoleId = RoleBox.SelectedValue as int?;

            if (_model.Id == 0)
                _model.Id = _svc.CreateStaff(_model);
            else
                _svc.UpdateStaff(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
