using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadersWindow : Window
    {
        public ReadersWindow()
        {
            InitializeComponent();
            var db = new DbService();
            IReaderService svc = new ReaderService(db);
            ReadersGrid.ItemsSource = svc.GetAllReaders();
        }
    }
}
