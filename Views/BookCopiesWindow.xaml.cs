using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookCopiesWindow : Window
    {
        private readonly IBookCopyService _svc;
        public BookCopiesWindow()
        {
            InitializeComponent();
            _svc = new BookCopyService(new DbService());
            Load();
        }

        private void Load() => CopiesGrid.ItemsSource = _svc.GetAllCopies().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new BookCopyEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (CopiesGrid.SelectedItem is BookCopy c)
            {
                var win = new BookCopyEditWindow(_svc, c);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CopiesGrid.SelectedItem is BookCopy c &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteCopy(c.Id);
                Load();
            }
        }
    }
}
