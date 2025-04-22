using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookCopiesWindow : Window
    {
        private readonly IBookCopyService _svc;
        private readonly CollectionView _view;
        private readonly System.Collections.Generic.IList<BookCopyViewModel> _list;

        public BookCopiesWindow()
        {
            InitializeComponent();
            _svc = new BookCopyService(new DbService());
            _list = _svc.GetAllCopyDetails().ToList();
            CopiesGrid.ItemsSource = _list;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(_list);

            // заполнить фильтры
            var books = _list
                .Select(x => x.BookTitle)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            books.Insert(0, string.Empty);
            BookFilterBox.ItemsSource = books;

            var rooms = _list
                .Select(x => x.ReadingRoomName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            rooms.Insert(0, string.Empty);
            RoomFilterBox.ItemsSource = rooms;
        }

        private void FilterControlChanged(object sender, RoutedEventArgs e)
        {
            var s = SearchBox.Text.Trim().ToLower();
            var fb = (BookFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;
            var fr = (RoomFilterBox.SelectedItem as string)?.ToLower() ?? string.Empty;

            _view.Filter = o =>
            {
                var vm = (BookCopyViewModel)o;
                var bySearch = string.IsNullOrEmpty(s)
                    || vm.BookTitle.ToLower().Contains(s)
                    || vm.ReadingRoomName.ToLower().Contains(s);
                var byBook = string.IsNullOrEmpty(fb) || vm.BookTitle.ToLower() == fb;
                var byRoom = string.IsNullOrEmpty(fr) || vm.ReadingRoomName.ToLower() == fr;
                return bySearch && byBook && byRoom;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (new BookCopyEditWindow(_svc).ShowDialog() == true)
                Refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (CopiesGrid.SelectedItem is BookCopyViewModel vm)
            {
                var model = _svc.GetCopyById(vm.Id);
                if (new BookCopyEditWindow(_svc, model).ShowDialog() == true)
                    Refresh();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CopiesGrid.SelectedItem is BookCopyViewModel vm
                && MessageBox.Show("Delete?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _svc.DeleteCopy(vm.Id);
                Refresh();
            }
        }

        private void Refresh()
        {
            _list.Clear();
            foreach (var x in _svc.GetAllCopyDetails())
                _list.Add(x);
            _view.Refresh();
        }
    }
}
