using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorBookWindow : Window
    {
        private readonly IAuthorBookService _svc;
        public AuthorBookWindow()
        {
            InitializeComponent();
            _svc = new AuthorBookService(new DbService());
            Load();
        }

        private void Load()
            => RelGrid.ItemsSource = _svc.GetAllRelations().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new AuthorBookEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RelGrid.SelectedItem is AuthorBook r)
            {
                var win = new AuthorBookEditWindow(_svc, r);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RelGrid.SelectedItem is AuthorBook r &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRelation(r.Id);
                Load();
            }
        }
    }
}
