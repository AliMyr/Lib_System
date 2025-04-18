using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LanguagesWindow : Window
    {
        private readonly ILanguageService _svc;
        public LanguagesWindow()
        {
            InitializeComponent();
            _svc = new LanguageService(new DbService());
            Load();
        }

        private void Load()
            => LanguagesGrid.ItemsSource = _svc.GetAllLanguages().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new LanguageEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (LanguagesGrid.SelectedItem is Language l)
            {
                var win = new LanguageEditWindow(_svc, l);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LanguagesGrid.SelectedItem is Language l &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteLanguage(l.Id);
                Load();
            }
        }
    }
}
