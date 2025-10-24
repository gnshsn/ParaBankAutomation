using OpenQA.Selenium;
using ParaBankAutomation.Models;

namespace ParaBankAutomation.Pages;

public class RegistrationPage : BasePage
{
    public RegistrationPage(IWebDriver driver) : base(driver) { }

    public void Navigate(string url)
    {
        Driver.Navigate().GoToUrl(url);
        Click(By.LinkText("Register"));
    }

    public void Register(Customer customer)
    {
        try
        {
            void SafeType(By locator, string? value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    Type(locator, value);
                else
                    Console.WriteLine($"Skipping empty field: {locator}");
            }

            SafeType(By.Name("customer.firstName"), customer.FirstName);
            SafeType(By.Name("customer.lastName"), customer.LastName);
            SafeType(By.Name("customer.address.street"), customer.Address);
            SafeType(By.Name("customer.address.city"), customer.City);
            SafeType(By.Name("customer.address.state"), customer.State);
            SafeType(By.Name("customer.address.zipCode"), customer.ZipCode);
            SafeType(By.Name("customer.phoneNumber"), customer.PhoneNumber);
            SafeType(By.Name("customer.ssn"), customer.SSN);
            SafeType(By.Name("customer.username"), customer.Username);
            SafeType(By.Name("customer.password"), customer.Password);
            SafeType(By.Name("repeatedPassword"), customer.Password);

            Click(By.CssSelector("input[value='Register']"));

            Wait.Until(d => d.FindElement(By.ClassName("title")));
        }
        catch (NoSuchElementException ex)
        {
            throw new Exception($"Registration failed - element not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Registration failed: {ex.Message}");
        }
    }
}