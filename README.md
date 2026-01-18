# TransferApp

TransferApp is an ASP.NET Core MVC web application for managing car transfer inquiries and reservations.
The project is built as a **portfolio and learning project**, with a strong focus on clean architecture and future QA automation.

---

## Overview

The application represents a small transfer service website with:
- a public-facing section for clients
- inquiry and reservation forms
- an administrative panel for managing requests

The project is intentionally designed to be easy to extend, test, and improve over time.

---

## Features

### Public Area
- Home page
- Prices page
- About page
- Contacts page
- Multi-language support (BG / EN / RU / FR)
- Contact form
- Transfer request form:
  - inquiry or reservation
  - route selection
  - one-way or round-trip

### Admin Panel
- Admin login
- Separate views for:
  - Inquiries
  - Reservations
- Change request status
- Delete requests
- Logout functionality
- Role-based access (Admin only)

---

## Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server / LocalDB
- Razor Views
- Bootstrap
- Cookie-based Authentication
- Localization with RESX resources

---

## Project Structure

TransferApp/
├── Controllers/
│ ├── HomeController.cs
│ ├── PagesController.cs
│ ├── TransferController.cs
│ ├── AdminTransfersController.cs
│ └── AccountController.cs
│
├── Data/
│ ├── ApplicationDbContext.cs
│ └── DbSeeder.cs
│
├── Models/
│ └── TransferRequest.cs
│
├── ViewModels/
│ ├── ContactFormViewModel.cs
│ └── PricesPublicViewModel.cs
│
├── Services/
│ ├── IEmailSender.cs
│ └── SmtpEmailSender.cs
│
├── Views/
│ ├── Home/
│ ├── Pages/
│ ├── Transfer/
│ ├── AdminTransfers/
│ └── Shared/
│
├── Resources/
│ └── SharedResources.*.resx
│
├── wwwroot/
│ └── css, js, images
│
├── Program.cs
├── appsettings.json
└── TransferApp.csproj


## Database

The application uses **Entity Framework Core** with SQL Server.

Main entity:
- `TransferRequests`
  - customer name
  - phone
  - email
  - route
  - pickup date and time
  - passengers
  - notes
  - status (Inquiry / Reservation)

Price routes are seeded automatically on application startup.

---

## Running the Application

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or newer
- SQL Server LocalDB

### Steps

```bash
git clone https://github.com/Elsaxos/TransferApp.git
cd TransferApp

Open TransferApp.sln in Visual Studio

Restore NuGet packages

Apply migrations if needed:

Update-Database


Run the project using IIS Express or Kestrel

Testing (Planned)

The project is structured to support:

Unit tests

Integration tests

UI tests (Selenium / Playwright)

It is actively used as a base for QA automation practice.

Future Improvements

Real email server integration

Spam protection (CAPTCHA / honeypot)

More advanced pricing logic

Multiple admin accounts

REST API

Automated test coverage

Production deployment

Author

Konstantin Stefanov
https://github.com/Elsaxos

