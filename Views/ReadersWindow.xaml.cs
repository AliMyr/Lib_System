using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReadersWindow : Window
    {
        private readonly IReaderService _svc;

        public ReadersWindow()
        {
            InitializeComponent();
            _svc = new ReaderService(new DbService());
            Load();
        }

        private void Load()
            => ReadersGrid.ItemsSource = _svc.GetAllReaders().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new ReaderEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersGrid.SelectedItem is Reader r)
            {
                var win = new ReaderEditWindow(_svc, r);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersGrid.SelectedItem is Reader r &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteReader(r.Id);
                Load();
            }
        }
    }
}
