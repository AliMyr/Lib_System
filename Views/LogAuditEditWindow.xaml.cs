using System;
using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogAuditEditWindow : Window
    {
        private readonly ILogAuditService _svc;
        private readonly ILogUserService _userSvc = new LogUserService(new DbService());
        private readonly LogAudit _model;

        public LogAuditEditWindow(ILogAuditService svc, LogAudit model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new LogAudit();

            var users = _userSvc.GetAllUserDetails().ToList();
            UserBox.ItemsSource = users;

            if (_model.Id != 0)
            {
                UserBox.SelectedValue = (ulong)(_model.UserId ?? 0);
                ActionBox.Text = _model.Action;
                DatePicker.SelectedDate = _model.CreatedAt;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.UserId = (int)(UserBox.SelectedValue ?? 0);
            _model.Action = ActionBox.Text.Trim();
            _model.CreatedAt = DatePicker.SelectedDate ?? DateTime.Now;

            if (_model.Id == 0)
                _model.Id = _svc.CreateAudit(_model);
            else
                _svc.UpdateAudit(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
