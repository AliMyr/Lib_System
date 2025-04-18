using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorBookWindow : Window
    {
        public AuthorBookWindow()
        {
            InitializeComponent();
            var svc = new AuthorBookService(new DbService());
            RelGrid.ItemsSource = svc.GetAllRelations();
        }
    }
}
