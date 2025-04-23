using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
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
        }

        private void ReportTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TopNBox.IsEnabled = ReportTypeBox.SelectedIndex == 1;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (ReportTypeBox.SelectedIndex == 0)
                ReportGrid.ItemsSource = _reportSvc.GetLoansByMonth();
            else
            {
                if (!int.TryParse(TopNBox.Text, out var n)) n = 5;
                ReportGrid.ItemsSource = _reportSvc.GetMostPopularBooks(n);
            }
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            // Если таблица пуста — выходим
            if (ReportGrid.ItemsSource == null) return;

            // Сохраняем путь
            var dlg = new SaveFileDialog { Filter = "CSV|*.csv" };
            if (dlg.ShowDialog() != true) return;
            var path = dlg.FileName;

            // Собираем все строки в список object
            var list = ReportGrid.ItemsSource.Cast<object>().ToList();
            if (!list.Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Определяем тип первой записи
                var first = list.First();

                if (first is LoansByMonthViewModel)
                {
                    // Кастим весь список и экспортируем
                    var data = list.Cast<LoansByMonthViewModel>();
                    _exportSvc.ExportToCsv(data, path);
                }
                else if (first is PopularBookViewModel)
                {
                    var data = list.Cast<PopularBookViewModel>();
                    _exportSvc.ExportToCsv(data, path);
                }
                else
                {
                    MessageBox.Show("Неподдерживаемый формат данных для CSV-экспорта.",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show("Экспорт в CSV выполнен успешно.",
                                "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте CSV:\n{ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            if (ReportGrid.ItemsSource == null) return;
            var dlg = new SaveFileDialog { Filter = "Excel|*.xlsx" };
            if (dlg.ShowDialog() != true) return;
            var data = ReportGrid.ItemsSource.Cast<object>();
            if (ReportTypeBox.SelectedIndex == 0)
                _exportSvc.ExportToExcel(data.Cast<LoansByMonthViewModel>(), dlg.FileName);
            else
                _exportSvc.ExportToExcel(data.Cast<PopularBookViewModel>(), dlg.FileName);
        }

        private void ImportCsv_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "CSV|*.csv" };
            if (dlg.ShowDialog() != true) return;

            try
            {
                if (ReportTypeBox.SelectedIndex == 0)
                {
                    var data = _exportSvc.ImportFromCsv<LoansByMonthViewModel>(dlg.FileName);
                    ReportGrid.ItemsSource = data;
                }
                else
                {
                    var data = _exportSvc.ImportFromCsv<PopularBookViewModel>(dlg.FileName);
                    ReportGrid.ItemsSource = data;
                }
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
