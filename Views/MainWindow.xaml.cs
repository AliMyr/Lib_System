using System.Windows;
using Lib_System.Views;

namespace Lib_System
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void ShowAuthors_Click(object sender, RoutedEventArgs e)
            => new AuthorsWindow().Show();

        private void ShowBooks_Click(object sender, RoutedEventArgs e)
            => new BooksWindow().Show();

        private void ShowReaders_Click(object sender, RoutedEventArgs e)
            => new ReadersWindow().Show();

        private void ShowGenres_Click(object sender, RoutedEventArgs e)
            => new GenresWindow().Show();

        private void ShowLanguages_Click(object sender, RoutedEventArgs e)
            => new LanguagesWindow().Show();

        private void ShowPublishers_Click(object sender, RoutedEventArgs e)
            => new PublishersWindow().Show();

    }
}
