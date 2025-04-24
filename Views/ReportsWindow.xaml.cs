// Views/ReportsWindow.xaml.cs
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using LiveCharts;
using LiveCharts.Wpf;
using Lib_System.Models;
using Lib_System.Services;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class ReportsWindow : Window
    {
        private readonly IReportService _reportSvc;
        private readonly IReportExportService _exportSvc;

        public ReportsWindow()
        {
            InitializeComponent();
            _reportSvc = new ReportService(new DbService());
            _exportSvc = new ReportExportService();

            ReportTypeBox.SelectedIndex = 0;
            InitializeChart();
        }

        private void InitializeChart()
        {
            // Гарантируем наличие хотя бы одной оси X и Y, чтобы LiveCharts не падал
            ReportChart.Series = new SeriesCollection();
            ReportChart.AxisX.Clear();
            ReportChart.AxisX.Add(new Axis
            {
                Labels = Array.Empty<string>(),
                Separator = new LiveCharts.Wpf.Separator { Step = 1, IsEnabled = false }
            });
            ReportChart.AxisY.Clear();
            ReportChart.AxisY.Add(new Axis
            {
                Title = "Count"
            });
        }

        private void ReportTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => TopNBox.IsEnabled = ReportTypeBox.SelectedIndex == 1;

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (ReportTypeBox.SelectedIndex == 0)
            {
                var data = _reportSvc.GetLoansByMonth().ToList();
                ReportGrid.ItemsSource = data;
                RenderLoansByMonth(data);
            }
            else
            {
                if (!int.TryParse(TopNBox.Text, out var n)) n = 5;
                var data = _reportSvc.GetMostPopularBooks(n).ToList();
                ReportGrid.ItemsSource = data;
                RenderPopularBooks(data);
            }
        }

        private void RenderLoansByMonth(System.Collections.Generic.List<LoansByMonthViewModel> data)
        {
            ReportChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title  = "Loans",
                    Values = new ChartValues<int>(data.Select(x => x.LoansCount))
                }
            };
            ReportChart.AxisX[0].Labels = data.Select(x => x.Month).ToArray();
        }

        private void RenderPopularBooks(System.Collections.Generic.List<PopularBookViewModel> data)
        {
            ReportChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title  = "BorrowCount",
                    Values = new ChartValues<int>(data.Select(x => x.BorrowCount))
                }
            };
            ReportChart.AxisX[0].Labels = data.Select(x => x.Title).ToArray();
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            if (ReportGrid.ItemsSource == null) return;
            var dlg = new SaveFileDialog { Filter = "CSV|*.csv" };
            if (dlg.ShowDialog() != true) return;
            var path = dlg.FileName;
            var list = ReportGrid.ItemsSource.Cast<object>().ToList();
            if (!list.Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var first = list.First();
                if (first is LoansByMonthViewModel)
                    _exportSvc.ExportToCsv(list.Cast<LoansByMonthViewModel>(), path);
                else if (first is PopularBookViewModel)
                    _exportSvc.ExportToCsv(list.Cast<PopularBookViewModel>(), path);
                else
                    throw new InvalidOperationException("Неподдерживаемый формат данных.");

                MessageBox.Show("Экспорт в CSV выполнен успешно.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте CSV:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            if (ReportGrid.ItemsSource == null) return;
            var dlg = new SaveFileDialog { Filter = "Excel|*.xlsx" };
            if (dlg.ShowDialog() != true) return;
            var path = dlg.FileName;
            var list = ReportGrid.ItemsSource.Cast<object>().ToList();
            if (!list.Any()) return;

            try
            {
                var first = list.First();
                if (first is LoansByMonthViewModel)
                    _exportSvc.ExportToExcel(list.Cast<LoansByMonthViewModel>(), path);
                else if (first is PopularBookViewModel)
                    _exportSvc.ExportToExcel(list.Cast<PopularBookViewModel>(), path);
                else
                    throw new InvalidOperationException("Неподдерживаемый формат данных.");

                MessageBox.Show("Экспорт в Excel выполнен успешно.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте Excel:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportCsv_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "CSV|*.csv" };
            if (dlg.ShowDialog() != true) return;

            try
            {
                if (ReportTypeBox.SelectedIndex == 0)
                    ReportGrid.ItemsSource = _exportSvc.ImportFromCsv<LoansByMonthViewModel>(dlg.FileName);
                else
                    ReportGrid.ItemsSource = _exportSvc.ImportFromCsv<PopularBookViewModel>(dlg.FileName);
            }
            catch (InvalidDataException ide)
            {
                MessageBox.Show(ide.Message, "Ошибка импорта", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось импортировать CSV:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
