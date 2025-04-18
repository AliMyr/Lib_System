using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadingRoomsWindow : Window
    {
        private readonly IReadingRoomService _svc;
        public ReadingRoomsWindow()
        {
            InitializeComponent();
            _svc = new ReadingRoomService(new DbService());
            Load();
        }

        private void Load()
            => RoomsGrid.ItemsSource = _svc.GetAllRooms().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new ReadingRoomEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is ReadingRoom r)
            {
                var win = new ReadingRoomEditWindow(_svc, r);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is ReadingRoom r &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRoom(r.Id);
                Load();
            }
        }
    }
}
