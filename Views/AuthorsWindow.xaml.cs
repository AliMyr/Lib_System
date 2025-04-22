using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorsWindow : Window
    {
        private readonly IAuthorService _svc;
        private readonly CollectionView _view;
        private readonly System.Collections.Generic.IList<AuthorViewModel> _list;

        public AuthorsWindow()
        {
            InitializeComponent();
            _svc = new AuthorService(new DbService());
            _list = _svc.GetAllAuthorDetails().ToList();
            AuthorsGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            // наполняем комбобоксы фильтров
            var names = _list
                .Select(x => x.FullName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            names.Insert(0, string.Empty);
            NameFilterBox.ItemsSource = names;

            var pens = _list
                .Select(x => x.PenName ?? string.Empty)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            pens.Insert(0, string.Empty);
            PenFilterBox.ItemsSource = pens;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fn = (NameFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fp = (PenFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var vm = (AuthorViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.FullName.ToLower().Contains(s)
                    || (vm.PenName?.ToLower().Contains(s) ?? false);
                var byName = string.IsNullOrEmpty(fn) || vm.FullName.ToLower() == fn;
                var byPen = string.IsNullOrEmpty(fp) || (vm.PenName?.ToLower() == fp);
                return bySearch && byName && byPen;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new AuthorEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is AuthorViewModel vm)
            {
                var model = _svc.GetAuthorById(vm.Id);
                if (new AuthorEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is AuthorViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteAuthor(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllAuthorDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
