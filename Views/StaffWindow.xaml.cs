using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class StaffWindow : Window
    {
        private readonly IStaffService _svc;
        public StaffWindow()
        {
            InitializeComponent();
            _svc = new StaffService(new DbService());
            Load();
        }

        private void Load()
            => StaffGrid.ItemsSource = _svc.GetAllStaff().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new StaffEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is Staff s)
            {
                var win = new StaffEditWindow(_svc, s);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is Staff s &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteStaff(s.Id);
                Load();
            }
        }
    }
}