# TransferApp

ASP.NET Core MVC application for managing car transfer inquiries and reservations, with a lightweight admin panel.
Built as a portfolio project and a base for QA automation work (unit/integration/UI tests).

## Features

### Public site
- Home page with business-style layout
- Pages: **Prices**, **About**, **Contacts**
- Multilingual UI (bg/en/ru/fr) via **.resx** localization
- Transfer request form with:
  - Inquiry vs Reservation (status-based)
  - Email field (required)
  - Route-based pricing (price loaded automatically from predefined routes)

### Admin panel (role: Admin)
- Separate lists:
  - **Inquiries** (Status = "Запитване")
  - **Reservations** (Status = "Резервация")
- Open details per item (view)
- Change status (Inquiry ⇄ Reservation)
- Delete inquiries/reservations
- Admin navigation shown only inside `/Admin`
- Login/Logout with cookie authentication

### Data & infrastructure
- SQL Server + Entity Framework Core
- Seed data for price routes (`DbSeeder.SeedPriceItems`)
- Services layer (email sender interface + SMTP implementation ready for real mailbox)

## Tech Stack
- .NET 8, ASP.NET Core MVC
- Entity Framework Core (SQL Server)
- Razor Views + Bootstrap
- Localization: `IStringLocalizer` + `Resources/SharedResources.*.resx`
- Cookie authentication + role-based authorization

## Project Structure

```text
TransferApp/
└── TransferApp/
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── PagesController.cs
    │   ├── TransferController.cs
    │   ├── AdminTransfersController.cs
    │   └── AccountController.cs
    ├── Data/
    │   ├── ApplicationDbContext.cs
    │   └── DbSeeder.cs
    ├── Models/
    │   └── TransferRequest.cs
    ├── ViewModels/
    │   ├── ContactFormViewModel.cs
    │   └── PricesPublicViewModel.cs
    ├── Services/
    │   ├── IEmailSender.cs
    │   └── SmtpEmailSender.cs
    ├── Resources/
    │   └── SharedResources.*.resx
    ├── Views/
    │   ├── Home/
    │   ├── Pages/
    │   ├── Transfer/
    │   ├── AdminTransfers/
    │   └── Shared/
    ├── wwwroot/
    │   └── css, js, images
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── README.md
    └── CHANGELOG.md
Note: The TransferApp/TransferApp nesting is intentional (solution root + project folder).

Running the app
Prerequisites
.NET 8 SDK

SQL Server (LocalDB or full SQL Server)

Visual Studio 2022 / Rider / VS Code

Steps
Clone the repository

Update appsettings.json connection string if needed

Apply migrations (if not already applied):

Using CLI:

dotnet tool restore
dotnet tool run dotnet-ef database update
Run the project:

Visual Studio: IIS Express / Kestrel

CLI:

dotnet run
Admin access
The admin area is under /Admin.

Login: /Account/Login

Authorization: Admin role (cookie auth)

Credentials are currently kept simple for local development.

QA / Testing direction (planned)
This project is designed to be extended with automated tests:

Unit tests:

Model validation and basic logic

Integration tests:

Database flows (create → save → list in admin)

UI tests (E2E):

Transfer form flows, admin flows (open/change/delete)

Roadmap
Real mailbox for domain and working SMTP

Anti-spam protection for contact form (honeypot/reCAPTCHA)

FAQ block on Contacts page

Better admin details view + filters/search

Deploy-ready configuratio
