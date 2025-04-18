using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorsWindow : Window
    {
        private readonly IAuthorService _svc;

        public AuthorsWindow()
        {
            InitializeComponent();
            _svc = new AuthorService(new DbService());
            Load();
        }

        private void Load()
            => AuthorsDataGrid.ItemsSource = _svc.GetAllAuthors().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new AuthorEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author a)
            {
                var win = new AuthorEditWindow(_svc, a);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author a &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo)== MessageBoxResult.Yes)
            {
                _svc.DeleteAuthor(a.Id);
                Load();
            }
        }
    }
}
