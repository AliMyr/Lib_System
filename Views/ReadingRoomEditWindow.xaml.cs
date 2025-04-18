using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadingRoomEditWindow : Window
    {
        private readonly IReadingRoomService _svc;
        public ReadingRoom Room { get; private set; }

        public ReadingRoomEditWindow(IReadingRoomService svc, ReadingRoom room = null)
        {
            InitializeComponent();
            _svc = svc;
            Room = room != null
                ? new ReadingRoom
                {
                    Id = room.Id,
                    Title = room.Title,
                    Floor = room.Floor,
                    Capacity = room.Capacity,
                    HasWiFi = room.HasWiFi,
                    StaffId = room.StaffId
                }
                : new ReadingRoom();
            TitleBox.Text = Room.Title;
            FloorBox.Text = Room.Floor?.ToString() ?? "";
            CapacityBox.Text = Room.Capacity?.ToString() ?? "";
            HasWiFiBox.IsChecked = Room.HasWiFi == true;
            StaffBox.Text = Room.StaffId?.ToString() ?? "";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Room.Title = TitleBox.Text.Trim();
            Room.Floor = int.TryParse(FloorBox.Text.Trim(), out var f) ? f : (int?)null;
            Room.Capacity = int.TryParse(CapacityBox.Text.Trim(), out var c) ? c : (int?)null;
            Room.HasWiFi = HasWiFiBox.IsChecked == true;
            Room.StaffId = int.TryParse(StaffBox.Text.Trim(), out var s) ? s : (int?)null;
            if (Room.Id == 0)
                Room.Id = _svc.CreateRoom(Room);
            else
                _svc.UpdateRoom(Room);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
