using System;
using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogSessionEditWindow : Window
    {
        private readonly ILogSessionService _svc;
        private readonly ILogUserService _userSvc = new LogUserService(new DbService());
        private readonly LogSession _model;

        public LogSessionEditWindow(ILogSessionService svc, LogSession model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new LogSession();

            var users = _userSvc.GetAllUserDetails().ToList();
            UserBox.ItemsSource = users;

            if (_model.Id != 0)
            {
                UserBox.SelectedValue = (ulong)(_model.UserId ?? 0);
                TokenBox.Text = _model.Token;
                CreatedPicker.SelectedDate = _model.CreatedAt;
                ExpiresPicker.SelectedDate = _model.ExpiresAt;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.UserId = (int)(UserBox.SelectedValue ?? 0);
            _model.Token = TokenBox.Text.Trim();
            _model.CreatedAt = CreatedPicker.SelectedDate ?? DateTime.Now;
            _model.ExpiresAt = ExpiresPicker.SelectedDate ?? DateTime.Now;

            if (_model.Id == 0)
                _model.Id = _svc.CreateSession(_model);
            else
                _svc.UpdateSession(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
