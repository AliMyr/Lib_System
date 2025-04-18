using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class PublisherEditWindow : Window
    {
        private readonly IPublisherService _svc;
        public Publisher Pub { get; private set; }

        public PublisherEditWindow(IPublisherService svc, Publisher pub = null)
        {
            InitializeComponent();
            _svc = svc;
            Pub = pub != null
                ? new Publisher { Id = pub.Id, Title = pub.Title, Country = pub.Country }
                : new Publisher();
            TitleBox.Text = Pub.Title;
            CountryBox.Text = Pub.Country;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Pub.Title = TitleBox.Text.Trim();
            Pub.Country = CountryBox.Text.Trim();
            if (Pub.Id == 0)
                Pub.Id = _svc.CreatePublisher(Pub);
            else
                _svc.UpdatePublisher(Pub);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
