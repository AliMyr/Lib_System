using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LanguagesWindow : Window
    {
        public LanguagesWindow()
        {
            InitializeComponent();
            var db = new DbService();
            ILanguageService svc = new LanguageService(db);
            LanguagesGrid.ItemsSource = svc.GetAllLanguages();
        }
    }
}