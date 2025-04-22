using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookEditWindow : Window
    {
        private readonly IBookService _svc;
        private readonly IPublisherService _pubSvc;
        private readonly IGenreService _genreSvc;
        private readonly ILanguageService _langSvc;
        private readonly Book _model;

        public BookEditWindow(IBookService svc, Book model = null)
        {
            InitializeComponent();
            _svc = svc;
            _pubSvc = new PublisherService(new DbService());
            _genreSvc = new GenreService(new DbService());
            _langSvc = new LanguageService(new DbService());
            _model = model ?? new Book();

            TitleBox.Text = _model.Title;
            DatePicker.SelectedDate = _model.PublicationDate;
            PagesBox.Text = _model.Pages.ToString();

            PubBox.ItemsSource = _pubSvc.GetAllPublisherDetails().ToList();
            GenreBox.ItemsSource = _genreSvc.GetAllGenreDetails().ToList();
            LangBox.ItemsSource = _langSvc.GetAllLanguageDetails().ToList();

            if (_model.Id != 0)
            {
                PubBox.SelectedValue = _model.PublisherId;
                GenreBox.SelectedValue = _model.GenreId;
                LangBox.SelectedValue = _model.LanguageId;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Title = TitleBox.Text.Trim();
            _model.PublicationDate = DatePicker.SelectedDate;
            _model.Pages = int.Parse(PagesBox.Text);
            _model.PublisherId = (int)PubBox.SelectedValue;
            _model.GenreId = (int)GenreBox.SelectedValue;
            _model.LanguageId = (int)LangBox.SelectedValue;

            if (_model.Id == 0)
                _model.Id = _svc.CreateBook(_model);
            else
                _svc.UpdateBook(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
