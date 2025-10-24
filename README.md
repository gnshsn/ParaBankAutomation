# ParaBank Automation

A simple .NET C# console application that automates customer registration, account creation, and loan processing on the ParaBank demo banking website using Selenium.

## 🎯 What It Does

1. Reads customer data from `ParaBankUsers.csv`
2. Registers each customer on ParaBank
3. Creates a checking account with initial deposit
4. Requests a $10,000 loan with 20% down payment
5. Extracts account details (DOB, Debit Card, CVV)
6. Generates a CSV report with all operations in EUR

## 🚀 Quick Start

```bash
# Install .NET 8.0 SDK, then:
dotnet new console -n ParaBankAutomation
cd ParaBankAutomation

# Install required packages (just 5!)
dotnet add package Selenium.WebDriver
dotnet add package Selenium.WebDriver.ChromeDriver
dotnet add package CsvHelper
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Configuration.Json

# Run the automation
dotnet run
```


## 🏗️ Simple Architecture

- **Models** - Customer and OperationReport
- **Services** - CsvService, WebAutomationService, ReportService
- **Pages** - Selenium Page Objects (BasePage, RegistrationPage, AccountPage, LoanPage)
- **Program.cs** - Main entry point with dependency injection

**Principles:** KISS (Keep It Simple), Dependency Injection, Page Object Model

## 📊 Input/Output

**Input:** `Data/ParaBankUsers.csv` - Customer information  
**Output:** `Data/Reports/ParaBank_Report_[timestamp].csv` - CSV report with results

## 🛠️ Tech Stack

- .NET 8.0 / C#
- Selenium WebDriver
- CsvHelper (CSV I/O)
- Microsoft.Extensions.DependencyInjection

## ⏱️ Estimated Time

~5 hours to build from scratch (simple and focused!)

## 📝 Status

📋 Ready for implementation - See PROJECT_GUIDE.md to get started!

---

**Target Website:** https://parabank.parasoft.com/parabank/index.htm
