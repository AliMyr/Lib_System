using System.Windows;
using System.Windows.Controls;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class AuthorEditWindow : Window
    {
        private readonly IAuthorService _svc;
        private readonly Author _model;

        public AuthorEditWindow(IAuthorService svc, Author model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Author();

            LastBox.Text = _model.LastName;
            FirstBox.Text = _model.FirstName;
            MiddleBox.Text = _model.MiddleName;
            PenBox.Text = _model.PenName;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.LastName = LastBox.Text.Trim();
            _model.FirstName = FirstBox.Text.Trim();
            _model.MiddleName = MiddleBox.Text.Trim();
            _model.PenName = PenBox.Text.Trim();

            if (_model.Id == 0)
                _model.Id = _svc.CreateAuthor(_model);
            else
                _svc.UpdateAuthor(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
