using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookEditWindow : Window
    {
        private readonly IBookService _svc;
        public Book Book { get; private set; }

        public BookEditWindow(IBookService svc, Book book = null)
        {
            InitializeComponent();
            _svc = svc;
            Book = book != null
                ? new Book
                {
                    Id = book.Id,
                    Title = book.Title,
                    PublisherId = book.PublisherId,
                    PublicationDate = book.PublicationDate,
                    Pages = book.Pages,
                    GenreId = book.GenreId,
                    LanguageId = book.LanguageId
                }
                : new Book();

            TitleBox.Text = Book.Title;
            PublisherBox.Text = Book.PublisherId.ToString();
            DateBox.Text = Book.PublicationDate?.ToString("yyyy-MM-dd") ?? "";
            PagesBox.Text = Book.Pages.ToString();
            GenreBox.Text = Book.GenreId.ToString();
            LangBox.Text = Book.LanguageId.ToString();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Book.Title = TitleBox.Text.Trim();
            Book.PublisherId = int.Parse(PublisherBox.Text.Trim());
            Book.PublicationDate = DateTime.TryParse(DateBox.Text.Trim(), out var dt) ? dt : (DateTime?)null;
            Book.Pages = int.Parse(PagesBox.Text.Trim());
            Book.GenreId = int.Parse(GenreBox.Text.Trim());
            Book.LanguageId = int.Parse(LangBox.Text.Trim());

            if (Book.Id == 0)
                Book.Id = _svc.CreateBook(Book);
            else
                _svc.UpdateBook(Book);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
