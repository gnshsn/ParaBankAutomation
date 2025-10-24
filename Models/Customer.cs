namespace ParaBankAutomation.Models;

public class Customer
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public decimal InitialDeposit { get; set; }

    // Filled after operations
    public string? DOB { get; set; }
    public string? DebitCard { get; set; }
    public string? CVV { get; set; }
    public string? AccountNumber { get; set; }
    public string? LoanAccountNumber { get; set; }
}

