using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookCopiesWindow : Window
    {
        private readonly IBookCopyService _svc;
        private readonly CollectionView _view;

        public BookCopiesWindow()
        {
            InitializeComponent();
            _svc = new BookCopyService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(CopiesGrid.ItemsSource);
        }

        private void RefreshGrid()
            => CopiesGrid.ItemsSource = _svc.GetAllCopyDetails().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var x = (BookCopyViewModel)o;
                return string.IsNullOrEmpty(f)
                    || x.BookTitle.ToLower().Contains(f)
                    || x.ReadingRoomName.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new BookCopyEditWindow(_svc).ShowDialog() == true) RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (CopiesGrid.SelectedItem is BookCopyViewModel vm)
            {
                var c = _svc.GetCopyById(vm.Id);
                if (new BookCopyEditWindow(_svc, c).ShowDialog() == true) RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CopiesGrid.SelectedItem is BookCopyViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteCopy(vm.Id);
                RefreshGrid();
            }
        }
    }
}
