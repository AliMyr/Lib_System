using System.Linq;
using System.Windows;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadersWindow : Window
    {
        private readonly IReaderService _svc;
        private readonly CollectionView _view;

        public ReadersWindow()
        {
            InitializeComponent();
            _svc = new ReaderService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(ReadersGrid.ItemsSource);
        }

        private void RefreshGrid()
            => ReadersGrid.ItemsSource = _svc.GetAllReaderDetails().ToList();

        private void FilterBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var r = (ReaderViewModel)o;
                return string.IsNullOrEmpty(f)
                    || r.FullName.ToLower().Contains(f)
                    || (r.Phone?.ToLower().Contains(f) ?? false)
                    || (r.Address?.ToLower().Contains(f) ?? false);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new ReaderEditWindow(_svc);
            if (win.ShowDialog() == true) RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersGrid.SelectedItem is ReaderViewModel vm)
            {
                var r = _svc.GetReaderById(vm.Id);
                var win = new ReaderEditWindow(_svc, r);
                if (win.ShowDialog() == true) RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersGrid.SelectedItem is ReaderViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteReader(vm.Id);
                RefreshGrid();
            }
        }
    }
}
