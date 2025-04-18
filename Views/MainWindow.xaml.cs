using System.Windows;
using Lib_System.Views;
namespace Lib_System
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void ShowAuthors_Click(object sender, RoutedEventArgs e)
        {
            new Views.AuthorsWindow().Show();
        }
        private void ShowBooks_Click(object sender, RoutedEventArgs e)
            => new BooksWindow().Show();
    }
}
