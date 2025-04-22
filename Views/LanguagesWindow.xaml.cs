using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LanguagesWindow : Window
    {
        private readonly ILanguageService _svc;
        private readonly CollectionView _view;

        public LanguagesWindow()
        {
            InitializeComponent();
            _svc = new LanguageService(new DbService());
            RefreshGrid();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(LanguagesGrid.ItemsSource);
        }

        private void RefreshGrid()
            => LanguagesGrid.ItemsSource = _svc.GetAllLanguageDetails().ToList();

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var f = FilterBox.Text.Trim().ToLower();
            _view.Filter = o =>
            {
                var x = (LanguageViewModel)o;
                return string.IsNullOrEmpty(f) || x.Title.ToLower().Contains(f);
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new LanguageEditWindow(_svc).ShowDialog() == true) RefreshGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (LanguagesGrid.SelectedItem is LanguageViewModel vm)
            {
                var lang = _svc.GetLanguageById(vm.Id);
                if (new LanguageEditWindow(_svc, lang).ShowDialog() == true) RefreshGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LanguagesGrid.SelectedItem is LanguageViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteLanguage(vm.Id);
                RefreshGrid();
            }
        }
    }
}
