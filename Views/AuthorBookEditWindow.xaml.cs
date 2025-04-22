using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorBookEditWindow : Window
    {
        private readonly IAuthorBookService _svc;
        private readonly AuthorBook _model;

        public AuthorBookEditWindow(IAuthorBookService svc, AuthorBook model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new AuthorBook();

            var authors = new AuthorService(new DbService()).GetAllAuthors().ToList();
            AuthorBox.ItemsSource = authors;
            var books = new BookService(new DbService()).GetAllBooks().ToList();
            BookBox.ItemsSource = books;

            if (_model.Id != 0)
            {
                AuthorBox.SelectedValue = _model.AuthorId;
                BookBox.SelectedValue = _model.BookId;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.AuthorId = (int)AuthorBox.SelectedValue;
            _model.BookId = (int)BookBox.SelectedValue;

            if (_model.Id == 0)
                _model.Id = _svc.CreateRelation(_model);
            else
                _svc.UpdateRelation(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
