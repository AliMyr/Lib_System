using System.Collections.Generic;

namespace Lib_System.Services.Interfaces
{
    public interface IReportExportService
    {
        void ExportToCsv<T>(IEnumerable<T> data, string filePath);
        void ExportToExcel<T>(IEnumerable<T> data, string filePath);
        IEnumerable<T> ImportFromCsv<T>(string filePath);
    }
}
