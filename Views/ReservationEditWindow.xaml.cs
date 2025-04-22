using System;
using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReservationEditWindow : Window
    {
        private readonly IReservationService _svc;
        private readonly IReaderService _rSvc = new ReaderService(new DbService());
        private readonly IReadingRoomService _rrSvc = new ReadingRoomService(new DbService());
        private readonly Reservation _model;

        public ReservationEditWindow(IReservationService svc, Reservation model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Reservation();

            var readers = _rSvc.GetAllReaderDetails().ToList();
            ReaderBox.ItemsSource = readers;
            var rooms = _rrSvc.GetAllRoomDetails().ToList();
            RoomBox.ItemsSource = rooms;

            if (_model.Id != 0)
            {
                ReaderBox.SelectedValue = _model.ReaderId;
                RoomBox.SelectedValue = _model.RoomId;
                DatePicker.SelectedDate = _model.ReservationDate;
                StartBox.Text = _model.StartTime.ToString();
                EndBox.Text = _model.EndTime.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.ReaderId = (int)ReaderBox.SelectedValue;
            _model.RoomId = (int)RoomBox.SelectedValue;
            _model.ReservationDate = DatePicker.SelectedDate ?? DateTime.Today;
            _model.StartTime = TimeSpan.Parse(StartBox.Text);
            _model.EndTime = TimeSpan.Parse(EndBox.Text);

            if (_model.Id == 0)
                _model.Id = _svc.CreateReservation(_model);
            else
                _svc.UpdateReservation(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
