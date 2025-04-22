using System.Windows;
using System.Windows.Controls;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReaderEditWindow : Window
    {
        private readonly IReaderService _svc;
        private readonly Reader _model;

        public ReaderEditWindow(IReaderService svc, Reader model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Reader();

            LastBox.Text = _model.LastName;
            FirstBox.Text = _model.FirstName;
            MiddleBox.Text = _model.MiddleName;
            PhoneBox.Text = _model.Phone;
            AddressBox.Text = _model.Address;
            DatePicker.SelectedDate = _model.RegistrationDate;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.LastName = LastBox.Text.Trim();
            _model.FirstName = FirstBox.Text.Trim();
            _model.MiddleName = MiddleBox.Text.Trim();
            _model.Phone = PhoneBox.Text.Trim();
            _model.Address = AddressBox.Text.Trim();
            _model.RegistrationDate = DatePicker.SelectedDate;

            if (_model.Id == 0)
                _model.Id = _svc.CreateReader(_model);
            else
                _svc.UpdateReader(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
