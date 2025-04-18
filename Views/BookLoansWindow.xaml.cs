using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookLoansWindow : Window
    {
        private readonly IBookLoanService _svc;
        public BookLoansWindow()
        {
            InitializeComponent();
            _svc = new BookLoanService(new DbService());
            Load();
        }

        private void Load() => LoansGrid.ItemsSource = _svc.GetAllLoans().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new BookLoanEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (LoansGrid.SelectedItem is BookLoan l)
            {
                var win = new BookLoanEditWindow(_svc, l);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LoansGrid.SelectedItem is BookLoan l &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteLoan(l.Id);
                Load();
            }
        }
    }
}
