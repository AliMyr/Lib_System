using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class PublishersWindow : Window
    {
        private readonly IPublisherService _svc;
        private readonly System.Collections.Generic.IList<PublisherViewModel> _list;
        private readonly CollectionView _view;

        public PublishersWindow()
        {
            InitializeComponent();
            _svc = new PublisherService(new DbService());
            _list = _svc.GetAllPublisherDetails().ToList();
            Grid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var titles = _list.Select(x => x.Title).Distinct().OrderBy(x => x).ToList();
            titles.Insert(0, string.Empty);
            TitleFilterBox.ItemsSource = titles;

            var countries = _list.Select(x => x.Country).Distinct().OrderBy(x => x).ToList();
            countries.Insert(0, string.Empty);
            CountryFilterBox.ItemsSource = countries;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var ft = (TitleFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fc = (CountryFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var p = (PublisherViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || p.Title.ToLower().Contains(s)
                    || p.Country.ToLower().Contains(s);
                var byTitle = string.IsNullOrEmpty(ft) || p.Title.ToLower() == ft;
                var byCountry = string.IsNullOrEmpty(fc) || p.Country.ToLower() == fc;
                return bySearch && byTitle && byCountry;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new PublisherEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is PublisherViewModel vm)
            {
                var model = _svc.GetPublisherById(vm.Id);
                if (new PublisherEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is PublisherViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeletePublisher(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllPublisherDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
