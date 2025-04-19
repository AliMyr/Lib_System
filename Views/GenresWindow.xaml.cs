using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class GenresWindow : Window
    {
        private readonly IGenreService _svc;
        private readonly CollectionView _view;

        public GenresWindow()
        {
            InitializeComponent();
            _svc = new GenreService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(GenresGrid.ItemsSource);
        }

        private void RefreshGrid()
        {
            var list = _svc.GetAllGenreDetails().ToList();
            GenresGrid.ItemsSource = list;
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filter = FilterBox.Text.Trim().ToLower();
            _view.Filter = obj =>
            {
                var g = (GenreViewModel)obj;
                return string.IsNullOrEmpty(filter)
                    || g.Title.ToLower().Contains(filter);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new GenreEditWindow(_svc);
            if (win.ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is GenreViewModel vm)
            {
                var genre = _svc.GetGenreById(vm.Id);
                var win = new GenreEditWindow(_svc, genre);
                if (win.ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is GenreViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteGenre(vm.Id);
                RefreshGrid();
            }
        }
    }
}
