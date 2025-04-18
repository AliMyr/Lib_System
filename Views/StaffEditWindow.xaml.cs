// Views/StaffEditWindow.xaml.cs
using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class StaffEditWindow : Window
    {
        private readonly IStaffService _svc;
        public Staff Staff { get; private set; }

        public StaffEditWindow(IStaffService svc, Staff staff = null)
        {
            InitializeComponent();
            _svc = svc;
            Staff = staff != null
                ? new Staff
                {
                    Id = staff.Id,
                    LastName = staff.LastName,
                    FirstName = staff.FirstName,
                    MiddleName = staff.MiddleName,
                    RoleId = staff.RoleId,
                    HiredDate = staff.HiredDate,
                    Phone = staff.Phone
                }
                : new Staff();

            LastNameBox.Text = Staff.LastName;
            FirstNameBox.Text = Staff.FirstName;
            MiddleNameBox.Text = Staff.MiddleName;
            RoleIdBox.Text = Staff.RoleId?.ToString() ?? "";
            HiredDateBox.Text = Staff.HiredDate?.ToString("yyyy-MM-dd") ?? "";
            PhoneBox.Text = Staff.Phone;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Staff.LastName = LastNameBox.Text.Trim();
            Staff.FirstName = FirstNameBox.Text.Trim();
            Staff.MiddleName = MiddleNameBox.Text.Trim();
            Staff.RoleId = int.TryParse(RoleIdBox.Text.Trim(), out var ri) ? ri : (int?)null;
            Staff.HiredDate = DateTime.TryParse(HiredDateBox.Text.Trim(), out var hd) ? hd : (DateTime?)null;
            Staff.Phone = PhoneBox.Text.Trim();

            if (Staff.Id == 0)
                Staff.Id = _svc.CreateStaff(Staff);
            else
                _svc.UpdateStaff(Staff);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
