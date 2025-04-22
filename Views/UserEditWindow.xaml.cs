using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class UserEditWindow : Window
    {
        private readonly ILogUserService _svc;
        private readonly IPasswordHasher _hasher = new PasswordHasher();
        private readonly LogUser _model;

        public UserEditWindow(ILogUserService svc, LogUser model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new LogUser();

            UserBox.Text = _model.Username;
            EmailBox.Text = _model.Email;
            CreatedPicker.SelectedDate = _model.CreatedAt;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Username = UserBox.Text.Trim();
            _model.Email = EmailBox.Text.Trim();
            if (!string.IsNullOrEmpty(PassBox.Password))
                _model.PasswordHash = _hasher.Hash(PassBox.Password);
            _model.CreatedAt = CreatedPicker.SelectedDate ?? DateTime.Now;

            if (_model.Id == 0)
                _model.Id = _svc.CreateUser(_model);
            else
                _svc.UpdateUser(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
