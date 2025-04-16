using System.Windows;
namespace Lib_System
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void ShowAuthors_Click(object sender, RoutedEventArgs e)
        {
            new Views.AuthorsWindow().Show();
        }
    }
}
