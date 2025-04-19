using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LanguageEditWindow : Window
    {
        private readonly ILanguageService _svc;
        public Language CurrentLanguage { get; private set; }

        public LanguageEditWindow(ILanguageService svc, Language lang = null)
        {
            InitializeComponent();
            _svc = svc;
            CurrentLanguage = lang != null
                ? new Language { Id = lang.Id, Title = lang.Title }
                : new Language();
            TitleBox.Text = CurrentLanguage.Title;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            CurrentLanguage.Title = TitleBox.Text.Trim();
            if (CurrentLanguage.Id == 0)
                CurrentLanguage.Id = _svc.CreateLanguage(CurrentLanguage);
            else
                _svc.UpdateLanguage(CurrentLanguage);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
