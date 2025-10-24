namespace ParaBankAutomation.Models;

public class OperationReport
{
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? DOB { get; set; }
    public string? AccountNumber { get; set; }
    public string? DebitCard { get; set; }
    public string? CVV { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal DownPayment { get; set; }
    public decimal TotalEUR { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}

