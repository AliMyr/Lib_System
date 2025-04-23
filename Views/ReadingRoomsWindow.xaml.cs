using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadingRoomsWindow : Window
    {
        private readonly IReadingRoomService _svc;
        private readonly System.Collections.Generic.IList<ReadingRoomViewModel> _list;
        private readonly CollectionView _view;

        public ReadingRoomsWindow()
        {
            InitializeComponent();
            _svc = new ReadingRoomService(new DbService());
            _list = _svc.GetAllRoomDetails().ToList();
            RoomsGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var titles = _list.Select(x => x.Title)
                              .Distinct()
                              .OrderBy(x => x)
                              .ToList();
            titles.Insert(0, string.Empty);
            TitleFilterBox.ItemsSource = titles;

            var staff = _list.Select(x => x.StaffName ?? string.Empty)
                             .Distinct()
                             .OrderBy(x => x)
                             .ToList();
            staff.Insert(0, string.Empty);
            StaffFilterBox.ItemsSource = staff;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var ft = (TitleFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fs = (StaffFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var r = (ReadingRoomViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || r.Title.ToLower().Contains(s)
                    || (r.StaffName?.ToLower().Contains(s) ?? false);
                var byTitle = string.IsNullOrEmpty(ft)
                    || r.Title.ToLower() == ft;
                var byStaff = string.IsNullOrEmpty(fs)
                    || (r.StaffName?.ToLower() == fs);
                return bySearch && byTitle && byStaff;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new ReadingRoomEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is ReadingRoomViewModel vm)
            {
                var model = _svc.GetRoomById(vm.Id);
                if (new ReadingRoomEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is ReadingRoomViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRoom(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllRoomDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
