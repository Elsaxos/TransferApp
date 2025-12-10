# TransferApp

ASP.NET Core MVC application for managing **car transfers**, **reservations**, and an **administrative panel**.  
Built as a portfolio project and training ground for QA automation (unit, integration, and UI tests).

---

## 🚗 Features

- Manage a **fleet of cars** (автопарк)
- Create **transfer reservations** (pickup, dropoff, datetime, passengers)
- Simple **pricing logic** (base example: fixed price)
- **Admin panel** with a list of all transfer requests
- SQL Server database via **Entity Framework Core**
- Clean **MVC architecture**:
  - Controllers
  - Models
  - Views

---

## 🧱 Architecture

**Main components:**

- `Data/ApplicationDbContext.cs` – EF Core DbContext (Cars, Drivers, TransferRequests)
- `Models/Car.cs` – car entity (make, model, registration, seats, image URL)
- `Models/Driver.cs` – driver entity (name, phone, notes)
- `Models/TransferRequest.cs` – transfer booking entity (customer, phone, route, datetime, passengers, status, notes)
- `Controllers/HomeController.cs` – landing page and navigation
- `Controllers/TransferController.cs` – create transfer requests and “Thank you” page
- `Controllers/AdminController.cs` – view all transfer requests (admin list)
- `Views/*` – Razor views for Home, Transfer, Admin

---

## 📂 Project Structure

```text
TransferApp/
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── Car.cs
│   ├── Driver.cs
│   └── TransferRequest.cs
├── Services/
│   ├── IEmailSender.cs
│   └── SmtpEmailSender.cs      (extensible for future email notifications)
├── Controllers/
│   ├── HomeController.cs
│   ├── TransferController.cs
│   └── AdminController.cs
├── Views/
│   ├── Home/
│   ├── Transfer/
│   └── Admin/
├── wwwroot/
│   └── (static content: CSS, JS, images)
├── appsettings.json
├── Program.cs
└── TransferApp.csproj
```

---

## 🗄 Database

The app uses **Entity Framework Core** with **SQL Server LocalDB**.

Example migration flow:

```
Add-Migration InitialCreate
Update-Database
```

This creates:

- Cars table
- Drivers table
- TransferRequests table

---

## ▶ Running the Application

### Prerequisites

- **.NET 8 SDK**
- **Visual Studio 2022** (or Rider / VS Code with C# extension)
- **SQL Server LocalDB** (installed with Visual Studio)

### Steps

Clone the repository:

```bash
git clone https://github.com/Elsaxos/TransferApp.git
cd TransferApp
```

Open `TransferApp.sln` in Visual Studio.

Restore NuGet packages (VS usually does this automatically).

Apply EF Core migrations if needed:

```powershell
Update-Database
```

Run the app:

- via IIS Express or
- via Kestrel (`dotnet run` from the project folder)

Navigate in the browser to the base URL (e.g. `https://localhost:xxxx/`).

---

## 🔍 Quality & Testing (QA Focus)

This project is intentionally designed to be extended with **automated tests**:

### Planned test types

#### ✅ Unit tests
- Validation of models (`TransferRequest`, `Car`)
- Simple pricing logic
- Controller actions logic (e.g. redirection, model state)

#### ✅ Integration tests
- EF Core in-memory / test database
- Full flow: create transfer → verify it is saved → visible in `/Admin/Index`

#### ✅ UI tests (end-to-end)
Using Selenium / Playwright:
- Open `/Transfer/Create`
- Fill form
- Submit
- Assert “Thank you” page and presence of the request in the admin list

---

### Suggested test project structure

```
tests/
├── TransferApp.UnitTests/
│   ├── Models/
│   └── Controllers/
└── TransferApp.IntegrationTests/
    ├── Database/
    └── EndToEnd/
```

### Example (xUnit) unit test snippet

```csharp
public class TransferRequestTests
{
    [Fact]
    public void New_Request_Should_Have_Default_Status_New()
    {
        var req = new TransferRequest();

        Assert.Equal("Нова", req.Status);
    }
}
```

---

## 🧪 How to run tests (planned)

Once test projects are added:

```bash
dotnet test
```

This will run:
- Unit tests
- Integration tests
- (Later) UI tests if configured

---

## 📌 Roadmap / Future Improvements

- Implement real pricing logic (distance-based, time-based, surcharges)
- Additional validation (e.g. date in the future, phone format, required fields)
- Authentication & Authorization for admin panel
- Email notifications on new transfer requests (using `SmtpEmailSender`)
- Better UI/UX with Bootstrap or Tailwind CSS
- API endpoints (REST) for mobile or external integrations
- Full test coverage of all critical paths
