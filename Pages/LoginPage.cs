using OpenQA.Selenium;

namespace ParaBankAutomation.Pages;

public class LoginPage : BasePage
{
    public LoginPage(IWebDriver driver) : base(driver) { }

    public void Login(string username, string password)
    {
        Type(By.Name("username"), username);
        Type(By.Name("password"), password);
        Click(By.CssSelector("input[value='Log In']"));

        Wait.Until(d => d.FindElement(By.LinkText("Accounts Overview")));
    }
}

