using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using OfficeOpenXml;
using Lib_System.Services.Interfaces;
using CsvHelper.Configuration;
using System.Globalization;
using System;

namespace Lib_System.Services
{
    public class ReportExportService : IReportExportService
    {
        public void ExportToCsv<T>(IEnumerable<T> data, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(data);
        }

        public IEnumerable<T> ImportFromCsv<T>(string filePath)
        {
            using var reader = new StreamReader(filePath);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var csv = new CsvReader(reader, config);
            try
            {
                return csv.GetRecords<T>().ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Ошибка при разборе CSV: {ex.Message}", ex);
            }
        }

        public void ExportToExcel<T>(IEnumerable<T> data, string filePath)
        {
            // Лицензия уже задана при старте App, здесь просто создаём пакет:
            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Report");
            sheet.Cells["A1"].LoadFromCollection(data, true);
            package.SaveAs(new FileInfo(filePath));
        }
    }
}
