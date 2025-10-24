using System.Globalization;
using CsvHelper;
using ParaBankAutomation.Models;

namespace ParaBankAutomation.Services;

public interface IReportService
{
    void GenerateCsvReport(List<OperationReport> reports, string outputPath);
}

public class ReportService : IReportService
{
    public void GenerateCsvReport(List<OperationReport> reports, string outputPath)
    {
        var fileName = $"ParaBank_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

        var baseDir = AppContext.BaseDirectory;

        var projectRoot = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

        var reportsFolder = Path.Combine(projectRoot, outputPath.TrimStart('.', '/', '\\'));

        if (!Directory.Exists(reportsFolder))
        {
            Directory.CreateDirectory(reportsFolder);
        }

        var fullPath = Path.Combine(reportsFolder, fileName);
        using var writer = new StreamWriter(fullPath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        // Write records - CsvHelper handles headers automatically
        csv.WriteRecords(reports);

        Console.WriteLine($"✓ Report generated: {fullPath}");
    }
}