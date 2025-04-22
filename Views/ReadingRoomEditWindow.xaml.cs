using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadingRoomEditWindow : Window
    {
        private readonly IReadingRoomService _svc;
        private readonly IStaffService _staffSvc;
        private readonly ReadingRoom _model;

        public ReadingRoomEditWindow(
            IReadingRoomService svc,
            ReadingRoom model = null)
        {
            InitializeComponent();
            _svc = svc;
            _staffSvc = new StaffService(new DbService());
            _model = model ?? new ReadingRoom();

            TitleBox.Text = _model.Title;
            FloorBox.Text = _model.Floor?.ToString();
            CapBox.Text = _model.Capacity?.ToString();
            WifiBox.IsChecked = _model.HasWiFi;

            var staff = _staffSvc.GetAllStaffDetails().ToList();
            StaffBox.ItemsSource = staff;
            if (_model.StaffId.HasValue)
                StaffBox.SelectedValue = _model.StaffId.Value;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Title = TitleBox.Text.Trim();

            if (int.TryParse(FloorBox.Text, out var f))
                _model.Floor = f;
            else
                _model.Floor = null;

            if (int.TryParse(CapBox.Text, out var c))
                _model.Capacity = c;
            else
                _model.Capacity = null;

            _model.HasWiFi = WifiBox.IsChecked == true;
            _model.StaffId = StaffBox.SelectedValue as int?;

            if (_model.Id == 0)
                _model.Id = _svc.CreateRoom(_model);
            else
                _svc.UpdateRoom(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
