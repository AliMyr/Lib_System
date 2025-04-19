using System.Linq;
using System.Windows;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class PublishersWindow : Window
    {
        private readonly IPublisherService _svc;
        private readonly CollectionView _view;
        public PublishersWindow()
        {
            InitializeComponent();
            _svc = new PublisherService(new DbService());
            Refresh();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Grid.ItemsSource);
        }

        void Refresh() => Grid.ItemsSource = _svc.GetAllPublisherDetails().ToList();

        private void FilterBox_TextChanged(object s, System.Windows.Controls.TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var p = (PublisherViewModel)o;
                return string.IsNullOrEmpty(f)
                    || p.Title.ToLower().Contains(f)
                    || p.Country.ToLower().Contains(f);
            };
        }

        private void Add_Click(object s, RoutedEventArgs e)
        { if (new PublisherEditWindow(_svc).ShowDialog() == true) Refresh(); }
        private void Edit_Click(object s, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is PublisherViewModel vm)
            {
                var p = _svc.GetPublisherById(vm.Id);
                if (new PublisherEditWindow(_svc, p).ShowDialog() == true) Refresh();
            }
        }
        private void Delete_Click(object s, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is PublisherViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeletePublisher(vm.Id);
                Refresh();
            }
        }
    }
}
