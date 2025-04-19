using System.Linq;
using System.Windows;
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

        public AuthorsWindow()
        {
            InitializeComponent();
            _svc = new AuthorService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(AuthorsGrid.ItemsSource);
        }

        private void RefreshGrid()
            => AuthorsGrid.ItemsSource = _svc.GetAllAuthorDetails().ToList();

        private void FilterBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var a = (AuthorViewModel)o;
                return string.IsNullOrEmpty(f)
                    || a.FullName.ToLower().Contains(f)
                    || (a.PenName?.ToLower().Contains(f) ?? false);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new AuthorEditWindow(_svc);
            if (win.ShowDialog() == true) RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is AuthorViewModel vm)
            {
                var a = _svc.GetAuthorById(vm.Id);
                var win = new AuthorEditWindow(_svc, a);
                if (win.ShowDialog() == true) RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is AuthorViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteAuthor(vm.Id);
                RefreshGrid();
            }
        }
    }
}
