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
        private readonly System.Collections.Generic.IList<GenreViewModel> _list;
        private readonly CollectionView _view;

        public GenresWindow()
        {
            InitializeComponent();
            _svc = new GenreService(new DbService());
            _list = _svc.GetAllGenreDetails().ToList();
            GenresGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var titles = _list
                .Select(x => x.Title)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            titles.Insert(0, string.Empty);
            TitleFilterBox.ItemsSource = titles;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var ft = (TitleFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var g = (GenreViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || g.Title.ToLower().Contains(s);
                var byTitle = string.IsNullOrEmpty(ft)
                    || g.Title.ToLower() == ft;
                return bySearch && byTitle;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new GenreEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is GenreViewModel vm)
            {
                var model = _svc.GetGenreById(vm.Id);
                if (new GenreEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is GenreViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteGenre(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllGenreDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
