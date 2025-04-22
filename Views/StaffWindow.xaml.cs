using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class StaffWindow : Window
    {
        private readonly IStaffService _svc;
        private readonly CollectionView _view;

        public StaffWindow()
        {
            InitializeComponent();
            _svc = new StaffService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(StaffGrid.ItemsSource);
        }

        private void RefreshGrid() =>
            StaffGrid.ItemsSource = _svc.GetAllStaffDetails().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var s = (StaffViewModel)o;
                return string.IsNullOrEmpty(f)
                    || s.FullName.ToLower().Contains(f)
                    || s.RoleTitle.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new StaffEditWindow(_svc).ShowDialog() == true)
                RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is StaffViewModel vm)
            {
                var model = _svc.GetStaffById(vm.Id);
                if (new StaffEditWindow(_svc, model).ShowDialog() == true)
                    RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is StaffViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteStaff(vm.Id);
                RefreshGrid();
            }
        }
    }
}
