using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class GenresWindow : Window
    {
        public GenresWindow()
        {
            InitializeComponent();
            var db = new DbService();
            IGenreService svc = new GenreService(db);
            GenresGrid.ItemsSource = svc.GetAllGenres();
        }
    }
}