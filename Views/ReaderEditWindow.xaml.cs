using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReaderEditWindow : Window
    {
        private readonly IReaderService _svc;
        public Reader Reader { get; private set; }

        public ReaderEditWindow(IReaderService svc, Reader reader = null)
        {
            InitializeComponent();
            _svc = svc;
            Reader = reader != null
                ? new Reader
                {
                    Id = reader.Id,
                    LastName = reader.LastName,
                    FirstName = reader.FirstName,
                    MiddleName = reader.MiddleName,
                    Phone = reader.Phone,
                    Address = reader.Address,
                    RegistrationDate = reader.RegistrationDate
                }
                : new Reader();

            LastNameBox.Text = Reader.LastName;
            FirstNameBox.Text = Reader.FirstName;
            MiddleNameBox.Text = Reader.MiddleName;
            PhoneBox.Text = Reader.Phone;
            AddressBox.Text = Reader.Address;
            RegDateBox.Text = Reader.RegistrationDate?.ToString("yyyy-MM-dd") ?? "";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Reader.LastName = LastNameBox.Text.Trim();
            Reader.FirstName = FirstNameBox.Text.Trim();
            Reader.MiddleName = MiddleNameBox.Text.Trim();
            Reader.Phone = PhoneBox.Text.Trim();
            Reader.Address = AddressBox.Text.Trim();
            Reader.RegistrationDate = DateTime.TryParse(RegDateBox.Text.Trim(), out var dt) ? dt : (DateTime?)null;

            if (Reader.Id == 0)
                Reader.Id = _svc.CreateReader(Reader);
            else
                _svc.UpdateReader(Reader);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
