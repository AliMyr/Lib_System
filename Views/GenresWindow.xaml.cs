using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class GenresWindow : Window
    {
        private readonly IGenreService _svc;
        public GenresWindow()
        {
            InitializeComponent();
            _svc = new GenreService(new DbService());
            Load();
        }

        private void Load()
            => GenresGrid.ItemsSource = _svc.GetAllGenres().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new GenreEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre g)
            {
                var win = new GenreEditWindow(_svc, g);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre g &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteGenre(g.Id);
                Load();
            }
        }
    }
}
