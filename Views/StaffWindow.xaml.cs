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
        private readonly System.Collections.Generic.IList<StaffViewModel> _list;
        private readonly CollectionView _view;

        public StaffWindow()
        {
            InitializeComponent();
            _svc = new StaffService(new DbService());
            _list = _svc.GetAllStaffDetails().ToList();
            StaffGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var roles = _list
                .Select(x => x.RoleTitle)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            roles.Insert(0, string.Empty);
            RoleFilterBox.ItemsSource = roles;

            var dates = _list
                .Select(x => x.HiredDate?.ToString("yyyy-MM-dd") ?? string.Empty)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            dates.Insert(0, string.Empty);
            DateFilterBox.ItemsSource = dates;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fr = (RoleFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fd = (DateFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var vm = (StaffViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.FullName.ToLower().Contains(s)
                    || vm.RoleTitle.ToLower().Contains(s)
                    || (vm.HiredDate?.ToString("yyyy-MM-dd").ToLower().Contains(s) ?? false)
                    || vm.Phone.ToLower().Contains(s);
                var byRole = string.IsNullOrEmpty(fr)
                    || vm.RoleTitle.ToLower() == fr;
                var byDate = string.IsNullOrEmpty(fd)
                    || (vm.HiredDate?.ToString("yyyy-MM-dd").ToLower() == fd);
                return bySearch && byRole && byDate;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new StaffEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is StaffViewModel vm)
            {
                var model = _svc.GetStaffById(vm.Id);
                if (new StaffEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (StaffGrid.SelectedItem is StaffViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteStaff(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllStaffDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
