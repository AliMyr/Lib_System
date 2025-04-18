using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class StaffWindow : Window
    {
        public StaffWindow()
        {
            InitializeComponent();
            var svc = new StaffService(new DbService());
            StaffGrid.ItemsSource = svc.GetAllStaff();
        }
    }
}
