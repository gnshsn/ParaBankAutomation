using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParaBankAutomation.Models;
using ParaBankAutomation.Pages;

namespace ParaBankAutomation.Services;

public interface IWebAutomationService : IDisposable
{
    Task<OperationReport> ProcessCustomer(Customer customer);
}

public class WebAutomationService : IWebAutomationService
{
    private IWebDriver? _driver;
    private readonly IConfiguration _config;

    public WebAutomationService(IConfiguration config)
    {
        _config = config;
        InitializeDriver();
    }

    private void InitializeDriver()
    {
        var options = new ChromeOptions();

        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    public async Task<OperationReport> ProcessCustomer(Customer customer)
    {
        var report = new OperationReport { Username = customer.Username };

        try
        {
            Console.WriteLine($"   Registering customer...");
            var registrationPage = new RegistrationPage(_driver!);
            registrationPage.Navigate(_config["ParaBank:BaseUrl"]!);
            registrationPage.Register(customer);

            // Wait a moment for the session to be established
            await Task.Delay(1000);

            Console.WriteLine($"   Creating account...");
            // 3. Create account
            var accountPage = new AccountPage(_driver!);
            customer.AccountNumber = accountPage.CreateAccount(customer.InitialDeposit);

            Console.WriteLine($"   Requesting loan...");
            // 4. Request loan
            var loanPage = new LoanPage(_driver!);
            var loanAmount = decimal.Parse(_config["Loan:Amount"]!);
            var downPaymentPercent = decimal.Parse(_config["Loan:DownPaymentPercent"]!);
            var downPayment = customer.InitialDeposit * (downPaymentPercent / 100);

            customer.LoanAccountNumber = loanPage.RequestLoan(
                loanAmount,
                downPayment,
                customer.AccountNumber!
            );

            Console.WriteLine($"   Extracting account details...");
            // 5. Get card details (from account page)
            customer.DebitCard = accountPage.GetDebitCardNumber();
            customer.CVV = accountPage.GetCVV();
            customer.DOB = accountPage.GetDOB();

            // 6. Calculate EUR total
            var conversionRate = decimal.Parse(_config["CurrencyConversion:UsdToEur"]!);
            var totalUSD = loanAmount + downPayment;

            Console.WriteLine($"   Logging out...");
            // 7. Logout
            accountPage.Logout();

            // Build report
            report.FullName = $"{customer.FirstName} {customer.LastName}";
            report.DOB = customer.DOB;
            report.AccountNumber = customer.AccountNumber;
            report.DebitCard = customer.DebitCard;
            report.CVV = customer.CVV;
            report.LoanAmount = loanAmount;
            report.DownPayment = downPayment;
            report.TotalEUR = totalUSD * conversionRate;
            report.Status = "SUCCESS";
        }
        catch (Exception ex)
        {
            report.Status = "FAILED";
            report.ErrorMessage = ex.Message;

            Console.WriteLine($"   Error: {ex.Message}");

            // Take screenshot
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var projectRoot = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
                var reportsDir = Path.Combine(projectRoot, "Data", "Reports");
                Directory.CreateDirectory(reportsDir);
                var screenshot = ((ITakesScreenshot)_driver!).GetScreenshot();
                var screenshotPath = Path.Combine(reportsDir, $"error_{customer.Username}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(screenshotPath);
                Console.WriteLine($"   Screenshot saved: {screenshotPath}");
            }
            catch
            {
                // Ignore screenshot errors
            }
        }

        return await Task.FromResult(report);
    }

    public void Dispose()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }
}

