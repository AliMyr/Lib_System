using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorBookEditWindow : Window
    {
        private readonly IAuthorBookService _svc;
        public AuthorBook Relation { get; private set; }

        public AuthorBookEditWindow(IAuthorBookService svc, AuthorBook rel = null)
        {
            InitializeComponent();
            _svc = svc;
            Relation = rel != null
                ? new AuthorBook { Id = rel.Id, AuthorId = rel.AuthorId, BookId = rel.BookId }
                : new AuthorBook();
            AuthorBox.Text = Relation.AuthorId.ToString();
            BookBox.Text = Relation.BookId.ToString();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Relation.AuthorId = int.Parse(AuthorBox.Text.Trim());
            Relation.BookId = int.Parse(BookBox.Text.Trim());
            if (Relation.Id == 0)
                Relation.Id = _svc.CreateRelation(Relation);
            else
                _svc.UpdateRelation(Relation);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
