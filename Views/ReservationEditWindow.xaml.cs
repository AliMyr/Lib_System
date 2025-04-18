using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReservationEditWindow : Window
    {
        private readonly IReservationService _svc;
        public Reservation Reservation { get; private set; }

        public ReservationEditWindow(IReservationService svc, Reservation res = null)
        {
            InitializeComponent();
            _svc = svc;
            Reservation = res != null
                ? new Reservation
                {
                    Id = res.Id,
                    ReaderId = res.ReaderId,
                    RoomId = res.RoomId,
                    ReservationDate = res.ReservationDate,
                    StartTime = res.StartTime,
                    EndTime = res.EndTime
                }
                : new Reservation();
            ReaderBox.Text = Reservation.ReaderId.ToString();
            RoomBox.Text = Reservation.RoomId.ToString();
            DateBox.Text = Reservation.ReservationDate.ToString("yyyy-MM-dd");
            StartBox.Text = Reservation.StartTime.ToString();
            EndBox.Text = Reservation.EndTime.ToString();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Reservation.ReaderId = int.Parse(ReaderBox.Text.Trim());
            Reservation.RoomId = int.Parse(RoomBox.Text.Trim());
            Reservation.ReservationDate = DateTime.Parse(DateBox.Text.Trim());
            Reservation.StartTime = TimeSpan.Parse(StartBox.Text.Trim());
            Reservation.EndTime = TimeSpan.Parse(EndBox.Text.Trim());
            if (Reservation.Id == 0)
                Reservation.Id = _svc.CreateReservation(Reservation);
            else
                _svc.UpdateReservation(Reservation);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
