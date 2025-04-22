using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookLoanEditWindow : Window
    {
        private readonly IBookLoanService _svc;
        private readonly IReaderService _rSvc;
        private readonly IBookCopyService _cSvc;
        private readonly BookLoan _model;

        public BookLoanEditWindow(
            IBookLoanService svc,
            BookLoan model = null)
        {
            InitializeComponent();
            _svc = svc;
            _rSvc = new ReaderService(new DbService());
            _cSvc = new BookCopyService(new DbService());
            _model = model ?? new BookLoan();

            var readers = _rSvc.GetAllReaderDetails().ToList();
            ReaderBox.ItemsSource = readers;

            var copies = _cSvc.GetAllCopyDetails().ToList();
            CopyBox.ItemsSource = copies;

            if (_model.Id != 0)
            {
                ReaderBox.SelectedValue = _model.ReaderId;
                CopyBox.SelectedValue = _model.BookCopiesId;
                LoanDatePicker.SelectedDate = _model.LoanDate;
                ReturnDatePicker.SelectedDate = _model.ReturnDate;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.ReaderId = (int)ReaderBox.SelectedValue;
            _model.BookCopiesId = (int)CopyBox.SelectedValue;
            _model.LoanDate = LoanDatePicker.SelectedDate;
            _model.ReturnDate = ReturnDatePicker.SelectedDate;

            if (_model.Id == 0)
                _model.Id = _svc.CreateLoan(_model);
            else
                _svc.UpdateLoan(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
