using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookCopyEditWindow : Window
    {
        private readonly IBookCopyService _svc;
        public BookCopy Copy { get; private set; }

        public BookCopyEditWindow(IBookCopyService svc, BookCopy copy = null)
        {
            InitializeComponent();
            _svc = svc;
            Copy = copy != null
                ? new BookCopy { Id = copy.Id, BookId = copy.BookId, ReadingRoomsId = copy.ReadingRoomsId }
                : new BookCopy();
            BookBox.Text = Copy.BookId.ToString();
            RoomBox.Text = Copy.ReadingRoomsId?.ToString() ?? "";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Copy.BookId = int.Parse(BookBox.Text.Trim());
            Copy.ReadingRoomsId = int.TryParse(RoomBox.Text.Trim(), out var r) ? r : (int?)null;
            if (Copy.Id == 0) Copy.Id = _svc.CreateCopy(Copy);
            else _svc.UpdateCopy(Copy);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
