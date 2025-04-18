using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class PublishersWindow : Window
    {
        private readonly IPublisherService _svc;
        public PublishersWindow()
        {
            InitializeComponent();
            _svc = new PublisherService(new DbService());
            Load();
        }

        private void Load()
            => PublishersGrid.ItemsSource = _svc.GetAllPublishers().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new PublisherEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (PublishersGrid.SelectedItem is Publisher p)
            {
                var win = new PublisherEditWindow(_svc, p);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (PublishersGrid.SelectedItem is Publisher p &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeletePublisher(p.Id);
                Load();
            }
        }
    }
}
