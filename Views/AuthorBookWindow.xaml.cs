using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorBookWindow : Window
    {
        private readonly IAuthorBookService _svc;
        private readonly CollectionView _view;
        private readonly IList<AuthorBookViewModel> _list;

        public AuthorBookWindow()
        {
            InitializeComponent();
            _svc = new AuthorBookService(new DbService());
            _list = _svc.GetAllRelationsDetailed().ToList();
            AuthorBookGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var authors = _list.Select(x => x.AuthorName).Distinct().OrderBy(x => x).ToList();
            authors.Insert(0, string.Empty);
            AuthorFilterBox.ItemsSource = authors;

            var books = _list.Select(x => x.BookTitle).Distinct().OrderBy(x => x).ToList();
            books.Insert(0, string.Empty);
            BookFilterBox.ItemsSource = books;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fa = (AuthorFilterBox.SelectedItem as string)?.ToLower() ?? "";
            var fb = (BookFilterBox.SelectedItem as string)?.ToLower() ?? "";

            _view.Filter = o =>
            {
                var vm = (AuthorBookViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.AuthorName.ToLower().Contains(s)
                    || vm.BookTitle.ToLower().Contains(s);
                var byAuthor = string.IsNullOrEmpty(fa)
                    || vm.AuthorName.ToLower() == fa;
                var byBook = string.IsNullOrEmpty(fb)
                    || vm.BookTitle.ToLower() == fb;
                return bySearch && byAuthor && byBook;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new AuthorBookEditWindow(_svc).ShowDialog() == true) Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorBookGrid.SelectedItem is AuthorBookViewModel vm)
            {
                var model = _svc.GetRelationById(vm.Id);
                if (new AuthorBookEditWindow(_svc, model).ShowDialog() == true) Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorBookGrid.SelectedItem is AuthorBookViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRelation(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllRelationsDetailed()) _list.Add(x);
            _view.Refresh();
        }
    }
}
