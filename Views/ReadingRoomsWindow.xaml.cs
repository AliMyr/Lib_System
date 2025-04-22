using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadingRoomsWindow : Window
    {
        private readonly IReadingRoomService _svc;
        private readonly CollectionView _view;

        public ReadingRoomsWindow()
        {
            InitializeComponent();
            _svc = new ReadingRoomService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(RoomsGrid.ItemsSource);
        }

        private void RefreshGrid() =>
            RoomsGrid.ItemsSource = _svc.GetAllRoomDetails().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var r = (ReadingRoomViewModel)o;
                return string.IsNullOrEmpty(f)
                    || r.Title.ToLower().Contains(f)
                    || (r.StaffName?.ToLower().Contains(f) ?? false);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new ReadingRoomEditWindow(_svc).ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is ReadingRoomViewModel vm)
            {
                var model = _svc.GetRoomById(vm.Id);
                if (new ReadingRoomEditWindow(_svc, model).ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is ReadingRoomViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRoom(vm.Id);
                RefreshGrid();
            }
        }
    }
}
