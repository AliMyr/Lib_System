using System;
using System.Windows;
using System.Windows.Controls;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReportsWindow : Window
    {
        private readonly IReportService _svc;

        public ReportsWindow()
        {
            InitializeComponent();
            _svc = new ReportService(new DbService());
            ReportTypeBox.SelectedIndex = 0;
        }

        private void ReportTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isTop5 = (ReportTypeBox.SelectedIndex == 1);
            TopNBox.IsEnabled = isTop5;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            switch (ReportTypeBox.SelectedIndex)
            {
                case 0:
                    ReportGrid.ItemsSource = _svc.GetLoansByMonth();
                    break;
                case 1:
                    if (!int.TryParse(TopNBox.Text, out int n)) n = 5;
                    ReportGrid.ItemsSource = _svc.GetMostPopularBooks(n);
                    break;
            }
        }
    }
}
