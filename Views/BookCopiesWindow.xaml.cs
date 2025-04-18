using System.Windows;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class BookCopiesWindow : Window
    {
        public BookCopiesWindow()
        {
            InitializeComponent();
            var svc = new BookCopyService(new DbService());
            CopiesGrid.ItemsSource = svc.GetAllCopies();
        }
    }
}
