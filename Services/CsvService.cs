using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ParaBankAutomation.Models;

namespace ParaBankAutomation.Services;

public interface ICsvService
{
    List<Customer> ReadCustomers(string filePath);
}

public class CsvService : ICsvService
{
    public List<Customer> ReadCustomers(string filePath)
    {
        var baseDir = AppContext.BaseDirectory;

        var projectRoot = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;

        var fullPath = Path.Combine(projectRoot, filePath.TrimStart('.', '/', '\\'));

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"CSV file not found at: {fullPath}");
        }
        using var reader = new StreamReader(fullPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // Map CSV columns to Customer properties
        csv.Context.RegisterClassMap<CustomerMap>();
        return csv.GetRecords<Customer>().ToList();
    }
}

// Map CSV columns (handle spaces in column names)
public class CustomerMap : ClassMap<Customer>
{
    public CustomerMap()
    {
        Map(m => m.FirstName).Name("First Name");
        Map(m => m.LastName).Name("Last Name");
        Map(m => m.Address).Name("Address");
        Map(m => m.City).Name("City");
        Map(m => m.State).Name("State");
        Map(m => m.ZipCode).Name("Zip Code");
        Map(m => m.PhoneNumber).Name("Phone Number");
        Map(m => m.SSN).Name("SSN");
        Map(m => m.Username).Name("Username");
        Map(m => m.Password).Name("Password");
        Map(m => m.AccountType).Name("Account Type");
        Map(m => m.InitialDeposit).Name("Initial Deposit").Default(0m);
        Map(m => m.DOB).Name("DOB").Optional();
        Map(m => m.DebitCard).Name("Debit Card").Optional();
        Map(m => m.CVV).Name("CVV").Optional();
    }
}

