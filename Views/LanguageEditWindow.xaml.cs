using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LanguageEditWindow : Window
    {
        private readonly ILanguageService _svc;
        public Language Language { get; private set; }

        public LanguageEditWindow(ILanguageService svc, Language lang = null)
        {
            InitializeComponent();
            _svc = svc;
            Language = lang != null
                ? new Language { Id = lang.Id, Title = lang.Title }
                : new Language();
            TitleBox.Text = Language.Title;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Language.Title = TitleBox.Text.Trim();
            if (Language.Id == 0)
                Language.Id = _svc.CreateLanguage(Language);
            else
                _svc.UpdateLanguage(Language);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
