using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookLoansWindow : Window
    {
        private readonly IBookLoanService _svc;
        private readonly CollectionView _view;

        public BookLoansWindow()
        {
            InitializeComponent();
            _svc = new BookLoanService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(LoansGrid.ItemsSource);
        }

        private void RefreshGrid()
        {
            var list = _svc.GetAllLoanDetails().ToList();
            LoansGrid.ItemsSource = list;
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var loan = (BookLoanViewModel)o;
                return string.IsNullOrEmpty(f)
                    || (loan.ReaderName?.ToLower().Contains(f) ?? false)
                    || (loan.BookTitle?.ToLower().Contains(f) ?? false);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new BookLoanEditWindow(_svc);
            if (win.ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (LoansGrid.SelectedItem is BookLoanViewModel vm)
            {
                var model = _svc.GetLoanById(vm.Id);
                var win = new BookLoanEditWindow(_svc, model);
                if (win.ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LoansGrid.SelectedItem is BookLoanViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteLoan(vm.Id);
                RefreshGrid();
            }
        }
    }
}
