using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadingRoomsWindow : Window
    {
        public ReadingRoomsWindow()
        {
            InitializeComponent();
            var svc = new ReadingRoomService(new DbService());
            RoomsGrid.ItemsSource = svc.GetAllRooms();
        }
    }
}
