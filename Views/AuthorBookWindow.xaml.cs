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

        public AuthorBookWindow()
        {
            InitializeComponent();
            _svc = new AuthorBookService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(AuthorBookGrid.ItemsSource);
        }

        private void RefreshGrid()
            => AuthorBookGrid.ItemsSource = _svc.GetAllRelationsDetailed().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var x = (AuthorBookViewModel)o;
                return string.IsNullOrEmpty(f)
                    || x.AuthorName.ToLower().Contains(f)
                    || x.BookTitle.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new AuthorBookEditWindow(_svc).ShowDialog() == true) RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorBookGrid.SelectedItem is AuthorBookViewModel vm)
            {
                var rel = _svc.GetRelationById(vm.Id);
                if (new AuthorBookEditWindow(_svc, rel).ShowDialog() == true) RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorBookGrid.SelectedItem is AuthorBookViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRelation(vm.Id);
                RefreshGrid();
            }
        }
    }
}
