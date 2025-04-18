using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class UsersWindow : Window
    {
        private readonly ILogUserService _svc;
        public UsersWindow()
        {
            InitializeComponent();
            _svc = new LogUserService(new DbService());
            Load();
        }

        private void Load() => UsersGrid.ItemsSource = _svc.GetAllUsers().ToList();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var win = new UserEditWindow(_svc);
            if (win.ShowDialog() == true) Load();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is LogUser u)
            {
                var win = new UserEditWindow(_svc, u);
                if (win.ShowDialog() == true) Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is LogUser u &&
                MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteUser(u.Id);
                Load();
            }
        }
    }
}
