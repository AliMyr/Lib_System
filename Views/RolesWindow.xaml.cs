using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class RolesWindow : Window
    {
        private readonly IRoleService _svc;
        private readonly System.Collections.Generic.IList<RoleViewModel> _list;
        private readonly CollectionView _view;

        public RolesWindow()
        {
            InitializeComponent();
            _svc = new RoleService(new DbService());
            _list = _svc.GetAllRoleDetails().ToList();
            RolesGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            var titles = _list
                .Select(x => x.Title)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            titles.Insert(0, string.Empty);
            TitleFilterBox.ItemsSource = titles;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var ft = (TitleFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var vm = (RoleViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.Title.ToLower().Contains(s);
                var byTitle = string.IsNullOrEmpty(ft)
                    || vm.Title.ToLower() == ft;
                return bySearch && byTitle;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new RoleEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RolesGrid.SelectedItem is RoleViewModel vm)
            {
                var model = _svc.GetRoleById(vm.Id);
                if (new RoleEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RolesGrid.SelectedItem is RoleViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteRole(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllRoleDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
