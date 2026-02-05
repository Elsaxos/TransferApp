```md
# TransferApp
This is a real personal client project – the application is built for actual planned usage by me, not as a demo or tutorial.

TransferApp is an ASP.NET Core MVC application for managing car transfer inquiries and reservations.  
The project is built as a **portfolio project** and also serves as a **base for QA automation work** (unit, integration and UI tests).

The application includes a public website and a lightweight admin panel for managing transfer requests.

---

## Features

### Public site
- Home page with business-style layout
- Pages:
  - Prices
  - About
  - Contacts
- Multilingual UI:
  - Bulgarian (bg)
  - English (en)
  - Russian (ru)
  - French (fr)
- Transfer request form:
  - Inquiry vs Reservation (status-based)
  - Email field (required)
  - Route-based pricing (price is loaded automatically)
  - Thank-you page after submission

---

### Admin panel (role: Admin)
- Separate admin area under `/Admin`
- Login / Logout with cookie authentication
- Lists:
  - Inquiries (`Status = "Запитване"`)
  - Reservations (`Status = "Резервация"`)
- Admin actions:
  - View details
  - Change status (Inquiry ⇄ Reservation)
  - Delete requests
- Admin navigation visible only inside admin area

---

## Data & Infrastructure
- SQL Server with Entity Framework Core
- Code-first migrations
- Seed data for transfer price routes
- Simple SMTP email service (ready for real mailbox)
- Localization using `.resx` files

---

## Tech Stack
- .NET 8
- ASP.NET Core MVC
- Entity Framework Core (SQL Server)
- Razor Views + Bootstrap
- Cookie authentication
- Role-based authorization
- Localization with `IStringLocalizer`

---

## Project Structure

```text
TransferApp/
└── TransferApp/
    ├── Program.cs
    ├── TransferApp.csproj
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── appsettings.Production.json
    ├── CHANGELOG.md
    ├── admin-hashes.txt
    │
    ├── Controllers/
    ├── Data/
    ├── Infrastructure/
    ├── Migrations/
    ├── Models/
    ├── Options/
    ├── Services/
    ├── Tools/
    ├── ViewModels/
    ├── Views/
    └── wwwroot/
Note: The TransferApp/TransferApp nesting is intentional
(solution root + actual web project).

Running the application
Prerequisites
.NET 8 SDK

SQL Server (LocalDB or full SQL Server)

Visual Studio 2022 / Rider / VS Code

Steps
Clone the repository

Update the connection string in appsettings.json if needed

Apply migrations:

dotnet tool restore
dotnet tool run dotnet-ef database update
Run the project:

Visual Studio: IIS Express / Kestrel

CLI:


dotnet run
Admin access
Admin area: /Admin

Login page: /Account/Login

Authentication: Cookie-based, Admin role

Admin credentials are kept simple for local development and will be changed after deployment.

Testing direction (planned)
This project is intended to be extended with automated tests:

Unit tests:

Controllers

ViewModels

Integration tests:

Database flows

Admin flows

UI / E2E tests:

Transfer request flow

Admin workflows

Roadmap
Real SMTP mailbox configuration

Anti-spam protection for contact form

Admin filters and search

Minor UI improvements

Deployment-ready configuration

Version
v1.0.0
Initial public release with core functionality.
