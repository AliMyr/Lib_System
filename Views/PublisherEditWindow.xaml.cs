using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class PublisherEditWindow : Window
    {
        private readonly IPublisherService _svc;
        private readonly Publisher _model;

        public PublisherEditWindow(IPublisherService svc, Publisher model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Publisher();

            TitleBox.Text = _model.Title;
            CountryBox.Text = _model.Country;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Title = TitleBox.Text.Trim();
            _model.Country = CountryBox.Text.Trim();

            if (_model.Id == 0)
                _model.Id = _svc.CreatePublisher(_model);
            else
                _svc.UpdatePublisher(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
