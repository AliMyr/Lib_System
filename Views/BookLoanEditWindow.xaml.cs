using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookLoanEditWindow : Window
    {
        private readonly IBookLoanService _svc;
        public BookLoan Loan { get; private set; }

        public BookLoanEditWindow(IBookLoanService svc, BookLoan loan = null)
        {
            InitializeComponent();
            _svc = svc;
            Loan = loan != null
                ? new BookLoan
                {
                    Id = loan.Id,
                    ReaderId = loan.ReaderId,
                    BookCopiesId = loan.BookCopiesId,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate
                }
                : new BookLoan();
            ReaderBox.Text = Loan.ReaderId.ToString();
            CopyBox.Text = Loan.BookCopiesId.ToString();
            LoanDateBox.Text = Loan.LoanDate?.ToString("yyyy-MM-dd") ?? "";
            ReturnDateBox.Text = Loan.ReturnDate?.ToString("yyyy-MM-dd") ?? "";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Loan.ReaderId = int.Parse(ReaderBox.Text.Trim());
            Loan.BookCopiesId = int.Parse(CopyBox.Text.Trim());
            Loan.LoanDate = DateTime.TryParse(LoanDateBox.Text.Trim(), out var ld) ? ld : (DateTime?)null;
            Loan.ReturnDate = DateTime.TryParse(ReturnDateBox.Text.Trim(), out var rd) ? rd : (DateTime?)null;
            if (Loan.Id == 0) Loan.Id = _svc.CreateLoan(Loan);
            else _svc.UpdateLoan(Loan);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
