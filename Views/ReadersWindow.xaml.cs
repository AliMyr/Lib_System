using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadersWindow : Window
    {
        private readonly IReaderService _svc;
        private readonly System.Collections.Generic.IList<ReaderViewModel> _list;
        private readonly CollectionView _view;

        public ReadersWindow()
        {
            InitializeComponent();
            _svc = new ReaderService(new DbService());
            _list = _svc.GetAllReaderDetails().ToList();
            ReadersGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var names = _list
                .Select(x => x.FullName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            names.Insert(0, string.Empty);
            NameFilterBox.ItemsSource = names;

            var dates = _list
                .Select(x => x.RegistrationDate?.ToString("yyyy-MM-dd") ?? string.Empty)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            dates.Insert(0, string.Empty);
            DateFilterBox.ItemsSource = dates;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fn = (NameFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fd = (DateFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var r = (ReaderViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || r.FullName.ToLower().Contains(s)
                    || r.Phone.ToLower().Contains(s)
                    || r.Address.ToLower().Contains(s)
                    || (r.RegistrationDate?.ToString("yyyy-MM-dd").ToLower().Contains(s) ?? false);
                var byName = string.IsNullOrEmpty(fn)
                    || r.FullName.ToLower() == fn;
                var byDate = string.IsNullOrEmpty(fd)
                    || (r.RegistrationDate?.ToString("yyyy-MM-dd").ToLower() == fd);
                return bySearch && byName && byDate;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new ReaderEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersGrid.SelectedItem is ReaderViewModel vm)
            {
                var model = _svc.GetReaderById(vm.Id);
                if (new ReaderEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersGrid.SelectedItem is ReaderViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteReader(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllReaderDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
