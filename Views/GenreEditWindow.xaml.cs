using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class GenreEditWindow : Window
    {
        private readonly IGenreService _svc;
        public Genre Genre { get; private set; }

        public GenreEditWindow(IGenreService svc, Genre genre = null)
        {
            InitializeComponent();
            _svc = svc;
            Genre = genre != null
                ? new Genre { Id = genre.Id, Title = genre.Title }
                : new Genre();
            TitleBox.Text = Genre.Title;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Genre.Title = TitleBox.Text.Trim();
            if (Genre.Id == 0)
                Genre.Id = _svc.CreateGenre(Genre);
            else
                _svc.UpdateGenre(Genre);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
