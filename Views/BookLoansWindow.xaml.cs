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
        private readonly System.Collections.Generic.IList<BookLoanViewModel> _list;

        public BookLoansWindow()
        {
            InitializeComponent();
            _svc = new BookLoanService(new DbService());
            _list = _svc.GetAllLoanDetails().ToList();
            LoansGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var readers = _list
                .Select(x => x.ReaderName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            readers.Insert(0, string.Empty);
            ReaderFilterBox.ItemsSource = readers;

            var books = _list
                .Select(x => x.BookTitle)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            books.Insert(0, string.Empty);
            BookFilterBox.ItemsSource = books;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fr = (ReaderFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fb = (BookFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var vm = (BookLoanViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.ReaderName.ToLower().Contains(s)
                    || vm.BookTitle.ToLower().Contains(s);
                var byReader = string.IsNullOrEmpty(fr)
                    || vm.ReaderName.ToLower() == fr;
                var byBook = string.IsNullOrEmpty(fb)
                    || vm.BookTitle.ToLower() == fb;
                return bySearch && byReader && byBook;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new BookLoanEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (LoansGrid.SelectedItem is BookLoanViewModel vm)
            {
                var model = _svc.GetLoanById(vm.Id);
                if (new BookLoanEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LoansGrid.SelectedItem is BookLoanViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteLoan(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllLoanDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
