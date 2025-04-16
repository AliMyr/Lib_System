using System.Windows;
using Lib_System.Services.Interfaces;
using Lib_System.Services;

namespace Lib_System.Views
{
    public partial class AuthorsWindow : Window
    {
        private readonly IAuthorService _authorService;
        public AuthorsWindow()
        {
            InitializeComponent();
            var dbService = new DbService();
            _authorService = new AuthorService(dbService);
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            AuthorsDataGrid.ItemsSource = _authorService.GetAllAuthors();
        }
    }
}
