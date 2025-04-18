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

        private void ShowAuthorBook_Click(object sender, RoutedEventArgs e)
            => new Views.AuthorBookWindow().Show();

        private void ShowCopies_Click(object sender, RoutedEventArgs e)
            => new Views.BookCopiesWindow().Show();

        private void ShowLoans_Click(object sender, RoutedEventArgs e)
            => new Views.BookLoansWindow().Show();

        private void ShowRooms_Click(object sender, RoutedEventArgs e)
            => new Views.ReadingRoomsWindow().Show();

        private void ShowReservations_Click(object sender, RoutedEventArgs e)
            => new Views.ReservationsWindow().Show();

        private void ShowRoles_Click(object sender, RoutedEventArgs e)
            => new Views.RolesWindow().Show();

        private void ShowStaff_Click(object sender, RoutedEventArgs e)
            => new Views.StaffWindow().Show();

        private void ShowAudit_Click(object sender, RoutedEventArgs e)
            => new Views.LogAuditWindow().Show();

        private void ShowSessions_Click(object sender, RoutedEventArgs e)
            => new Views.LogSessionsWindow().Show();

    }
}
