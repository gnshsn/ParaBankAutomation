using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParaBankAutomation.Models;
using ParaBankAutomation.Services;

namespace ParaBankAutomation;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Setup DI
        var services = new ServiceCollection()
            .AddSingleton<IConfiguration>(config)
            .AddTransient<ICsvService, CsvService>()
            .AddTransient<IReportService, ReportService>()
            .BuildServiceProvider();

        try
        {
            // Create output directory if it doesn't exist
            var reportsFolder = config["Files:ReportsFolder"]!;
            Directory.CreateDirectory(reportsFolder);

            // Read customers
            var csvService = services.GetRequiredService<ICsvService>();
            var customers = csvService.ReadCustomers(config["Files:InputCsv"]!);
            Console.WriteLine($" Found {customers.Count} customers in CSV file\n");

            // Process each customer
            var reports = new List<OperationReport>();

            foreach (var customer in customers)
            {
                Console.WriteLine($"Processing: {customer.Username} ({customer.FirstName} {customer.LastName})");

                // Skip invalid customers (e.g., missing data)
                if (string.IsNullOrEmpty(customer.Address) || string.IsNullOrEmpty(customer.LastName) || string.IsNullOrEmpty(customer.FirstName) ||
        customer.InitialDeposit < 100 || string.IsNullOrEmpty(customer.Username) ||
        string.IsNullOrEmpty(customer.Password))
                {
                    Console.WriteLine($" Skipped - Invalid data (missing name, address, username, or deposit)\n");

                    var skippedReport = new OperationReport
                    {
                        Username = customer.Username,
                        FullName = $"{customer.FirstName} {customer.LastName}".Trim(),
                        Status = "SKIPPED",
                        ErrorMessage = "Invalid or missing registration data"
                    };
                    reports.Add(skippedReport);
                    continue;
                }

                try
                {
                    using var automation = new WebAutomationService(config);
                    var report = await automation.ProcessCustomer(customer);
                    reports.Add(report);

                    Console.WriteLine($" {report.Status}");
                    if (report.Status == "SUCCESS")
                    {
                        Console.WriteLine($"    Account: {report.AccountNumber}");
                        Console.WriteLine($"    Loan: ${report.LoanAmount:N2} (Down: ${report.DownPayment:N2})");
                        Console.WriteLine($"    Total EUR: €{report.TotalEUR:N2}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Failed: {ex.Message}\n");

                    reports.Add(new OperationReport
                    {
                        Username = customer.Username,
                        FullName = $"{customer.FirstName} {customer.LastName}".Trim(),
                        Status = "FAILED",
                        ErrorMessage = ex.Message
                    });

                    // Continue to next customer
                    continue;
                }
 
                await Task.Delay(2000);
            }

            // Generate report
            Console.WriteLine("Generating final report...");
            var reportService = services.GetRequiredService<IReportService>();
            reportService.GenerateCsvReport(reports, reportsFolder);

            // Summary
            Console.WriteLine();
            Console.WriteLine("----  Summary Report ----");
            Console.WriteLine($"Total Customers: {reports.Count}");
            Console.WriteLine($"Successful: {reports.Count(r => r.Status == "SUCCESS")}");
            Console.WriteLine($"Failed: {reports.Count(r => r.Status == "FAILED")}");
            Console.WriteLine($"Skipped: {reports.Count(r => r.Status == "SKIPPED")}");
            Console.WriteLine();
            Console.WriteLine("Automation Complete!");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($" Fatal Error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Environment.Exit(1);
        }
    }
}