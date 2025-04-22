using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReservationsWindow : Window
    {
        private readonly IReservationService _svc;
        private readonly CollectionView _view;

        public ReservationsWindow()
        {
            InitializeComponent();
            _svc = new ReservationService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(ResGrid.ItemsSource);
        }

        private void RefreshGrid() =>
            ResGrid.ItemsSource = _svc.GetAllReservationDetails().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var r = (ReservationViewModel)o;
                return string.IsNullOrEmpty(f)
                    || r.ReaderName.ToLower().Contains(f)
                    || r.RoomTitle.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new ReservationEditWindow(_svc).ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ResGrid.SelectedItem is ReservationViewModel vm)
            {
                var model = _svc.GetReservationById(vm.Id);
                if (new ReservationEditWindow(_svc, model).ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ResGrid.SelectedItem is ReservationViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteReservation(vm.Id);
                RefreshGrid();
            }
        }
    }
}
