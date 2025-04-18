using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class PublishersWindow : Window
    {
        public PublishersWindow()
        {
            InitializeComponent();
            var db = new DbService();
            IPublisherService svc = new PublisherService(db);
            PublishersGrid.ItemsSource = svc.GetAllPublishers();
        }
    }
}