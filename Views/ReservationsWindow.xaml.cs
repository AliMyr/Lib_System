using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReservationsWindow : Window
    {
        private readonly IReservationService _svc;
        public ReservationsWindow()
        {
            InitializeComponent();
            _svc = new ReservationService(new DbService());
            Load();
        }

        private void Load()
            => ResGrid.ItemsSource = _svc.GetAllReservations().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new ReservationEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ResGrid.SelectedItem is Reservation r)
            {
                var win = new ReservationEditWindow(_svc, r);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ResGrid.SelectedItem is Reservation r &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteReservation(r.Id);
                Load();
            }
        }
    }
}
