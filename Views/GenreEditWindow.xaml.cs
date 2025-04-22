using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class GenreEditWindow : Window
    {
        private readonly IGenreService _svc;
        private readonly Genre _model;

        public GenreEditWindow(IGenreService svc, Genre model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Genre();

            TitleBox.Text = _model.Title;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Title = TitleBox.Text.Trim();

            if (_model.Id == 0)
                _model.Id = _svc.CreateGenre(_model);
            else
                _svc.UpdateGenre(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
