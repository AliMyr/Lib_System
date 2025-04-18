using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorEditWindow : Window
    {
        private readonly IAuthorService _svc;
        public Author Author { get; private set; }

        public AuthorEditWindow(IAuthorService svc, Author author = null)
        {
            InitializeComponent();
            _svc = svc;
            Author = author ?? new Author();
            DataContext = Author;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Author.LastName = LastNameBox.Text.Trim();
            Author.FirstName = FirstNameBox.Text.Trim();
            Author.MiddleName = MiddleNameBox.Text.Trim();
            Author.PenName = PenNameBox.Text.Trim();

            if (Author.Id == 0)
                Author.Id = _svc.CreateAuthor(Author);
            else
                _svc.UpdateAuthor(Author);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
