using System.Linq;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookCopyEditWindow : Window
    {
        private readonly IBookCopyService _svc;
        private readonly BookCopy _model;

        public BookCopyEditWindow(IBookCopyService svc, BookCopy model = null)
        {
            InitializeComponent();
            _svc = svc;
            _model = model ?? new BookCopy();

            var books = new BookService(new DbService()).GetAllBooks().ToList();
            BookBox.ItemsSource = books;
            var rooms = new ReadingRoomService(new DbService()).GetAllRoomDetails().ToList();
            RoomBox.ItemsSource = rooms;

            if (_model.Id != 0)
            {
                BookBox.SelectedValue = _model.BookId;
                RoomBox.SelectedValue = _model.ReadingRoomsId;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _model.BookId = (int)BookBox.SelectedValue;
            _model.ReadingRoomsId = (int)RoomBox.SelectedValue;

            if (_model.Id == 0)
                _model.Id = _svc.CreateCopy(_model);
            else
                _svc.UpdateCopy(_model);

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
