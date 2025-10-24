using OpenQA.Selenium;

namespace ParaBankAutomation.Pages;

public class LoanPage : BasePage
{
    public LoanPage(IWebDriver driver) : base(driver) { }

    public string RequestLoan(decimal loanAmount, decimal downPayment, string fromAccountNumber)
    {
        Click(By.LinkText("Request Loan"));

        Type(By.Id("amount"), loanAmount.ToString("0"));
        Type(By.Id("downPayment"), downPayment.ToString("0"));

        Click(By.CssSelector("input[value='Apply Now']"));

        Wait.Until(d => d.FindElement(By.Id("loanStatus")));

        var status = GetText(By.Id("loanStatus"));
        if (status.Contains("Approved") || status.Contains("approved"))
        {
            try
            {
                return GetText(By.Id("newAccountId"));
            }
            catch
            {
                return "Loan Approved";
            }
        }
        else
        {
            throw new Exception($"Loan was not approved: {status}");
        }
    }
}

