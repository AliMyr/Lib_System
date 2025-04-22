using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BooksWindow : Window
    {
        private readonly IBookService _svc;
        private readonly System.Collections.Generic.IList<BookViewModel> _list;
        private readonly CollectionView _view;

        public BooksWindow()
        {
            InitializeComponent();
            _svc = new BookService(new DbService());
            _list = _svc.GetAllBookDetails().ToList();
            BooksGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var pubs = _list.Select(x => x.PublisherName).Distinct().OrderBy(x => x).ToList();
            pubs.Insert(0, string.Empty);
            PublisherFilterBox.ItemsSource = pubs;

            var gens = _list.Select(x => x.GenreTitle).Distinct().OrderBy(x => x).ToList();
            gens.Insert(0, string.Empty);
            GenreFilterBox.ItemsSource = gens;

            var langs = _list.Select(x => x.LanguageTitle).Distinct().OrderBy(x => x).ToList();
            langs.Insert(0, string.Empty);
            LanguageFilterBox.ItemsSource = langs;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fp = (PublisherFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fg = (GenreFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fl = (LanguageFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var b = (BookViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || b.Title.ToLower().Contains(s)
                    || b.PublisherName.ToLower().Contains(s)
                    || b.GenreTitle.ToLower().Contains(s)
                    || b.LanguageTitle.ToLower().Contains(s);
                var byPub = string.IsNullOrEmpty(fp) || b.PublisherName.ToLower() == fp;
                var byGen = string.IsNullOrEmpty(fg) || b.GenreTitle.ToLower() == fg;
                var byLan = string.IsNullOrEmpty(fl) || b.LanguageTitle.ToLower() == fl;
                return bySearch && byPub && byGen && byLan;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new BookEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is BookViewModel vm)
            {
                var model = _svc.GetBookById(vm.Id);
                if (new BookEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is BookViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteBook(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllBookDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
