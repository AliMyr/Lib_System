using System.Linq;
using System.Windows;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BooksWindow : Window
    {
        private readonly IBookService _svc;
        private readonly CollectionView _view;

        public BooksWindow()
        {
            InitializeComponent();
            _svc = new BookService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(BooksGrid.ItemsSource);
        }

        private void RefreshGrid()
        {
            var vmList = _svc.GetAllBookDetails().ToList();
            BooksGrid.ItemsSource = vmList;
        }

        private void FilterBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var b = (BookViewModel)o;
                return string.IsNullOrEmpty(f) || b.Title.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new BookEditWindow(_svc);
            if (win.ShowDialog() == true) RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is BookViewModel vm)
            {
                var book = _svc.GetBookById(vm.Id);
                var win = new BookEditWindow(_svc, book);
                if (win.ShowDialog() == true) RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is BookViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteBook(vm.Id);
                RefreshGrid();
            }
        }
    }
}
