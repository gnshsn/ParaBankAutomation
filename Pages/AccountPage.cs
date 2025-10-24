using OpenQA.Selenium;

namespace ParaBankAutomation.Pages;

public class AccountPage : BasePage
{
    public AccountPage(IWebDriver driver) : base(driver) { }

    public string CreateAccount(decimal initialDeposit)
    {
        Click(By.LinkText("Open New Account"));

        Click(By.CssSelector("input[value='Open New Account']"));

        Wait.Until(d => d.FindElement(By.Id("newAccountId")));
        return GetText(By.Id("newAccountId"));
    }

    public string GetDebitCardNumber()
    {
        Click(By.LinkText("Accounts Overview"));

        var accountLink = Find(By.CssSelector("table.ng-scope tbody tr:first-child td:first-child a"));
        accountLink.Click();

        try
        {
            Wait.Until(d => d.FindElement(By.Id("accountDetails")));

            return "N/A - Check account details";
        }
        catch
        {
            return "N/A";
        }
    }

    public string GetCVV()
    {
        return "N/A";
    }

    public string GetDOB()
    {
        try
        {
            Click(By.LinkText("Update Contact Info"));
            Wait.Until(d => d.FindElement(By.Id("customer.firstName")));

            return "N/A - Check profile";
        }
        catch
        {
            return "N/A";
        }
    }

    public void Logout()
    {
        Click(By.LinkText("Log Out"));
    }
}

