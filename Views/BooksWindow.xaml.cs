using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BooksWindow : Window
    {
        private readonly IBookService _svc;

        public BooksWindow()
        {
            InitializeComponent();
            _svc = new BookService(new DbService());
            Load();
        }

        private void Load()
            => BooksGrid.ItemsSource = _svc.GetAllBooks().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new BookEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book b)
            {
                var win = new BookEditWindow(_svc, b);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book b &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteBook(b.Id);
                Load();
            }
        }
    }
}
