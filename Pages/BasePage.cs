using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBankAutomation.Pages;

public class BasePage
{
    protected IWebDriver Driver;
    protected WebDriverWait Wait;

    public BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    }

    protected IWebElement Find(By locator) => Wait.Until(d => d.FindElement(locator));

    protected void Click(By locator) => Find(locator).Click();

    protected void Type(By locator, string text)
    {
        var element = Find(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected string GetText(By locator) => Find(locator).Text;

    protected bool IsElementPresent(By locator)
    {
        try
        {
            Driver.FindElement(locator);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}

