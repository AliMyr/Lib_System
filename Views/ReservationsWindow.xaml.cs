using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReservationsWindow : Window
    {
        public ReservationsWindow()
        {
            InitializeComponent();
            var svc = new ReservationService(new DbService());
            ResGrid.ItemsSource = svc.GetAllReservations();
        }
    }
}
