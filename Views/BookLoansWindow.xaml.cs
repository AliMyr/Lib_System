using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookLoansWindow : Window
    {
        public BookLoansWindow()
        {
            InitializeComponent();
            var svc = new BookLoanService(new DbService());
            LoansGrid.ItemsSource = svc.GetAllLoans();
        }
    }
}
