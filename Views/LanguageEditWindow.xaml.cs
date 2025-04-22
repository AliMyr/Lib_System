using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LanguageEditWindow : Window
    {
        private readonly ILanguageService _svc;
        private readonly Language _model;

        public LanguageEditWindow(ILanguageService svc, Language model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new Language();

            TitleBox.Text = _model.Title;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.Title = TitleBox.Text.Trim();

            if (_model.Id == 0)
                _model.Id = _svc.CreateLanguage(_model);
            else
                _svc.UpdateLanguage(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
