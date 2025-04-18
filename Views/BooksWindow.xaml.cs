using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BooksWindow : Window
    {
        public BooksWindow()
        {
            InitializeComponent();
            var db = new DbService();
            IBookService svc = new BookService(db);
            BooksGrid.ItemsSource = svc.GetAllBooks();
        }
    }
}
