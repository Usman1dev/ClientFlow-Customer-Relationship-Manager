# RUD-CRM — Relationship Understanding & Development

A modern full-stack CRM application built with **Blazor WebAssembly (.NET 10)**, **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**.

Developed as a semester project — Visual Programming Lab, Spring 2026.

---

## Tech Stack

| Layer      | Technology                                 |
|------------|--------------------------------------------|
| Frontend   | Blazor WebAssembly (.NET 10)               |
| Backend    | ASP.NET Core Web API (.NET 10)             |
| Database   | SQL Server (via SSMS)                      |
| ORM        | Entity Framework Core 10                   |
| Auth       | ASP.NET Core Identity + JWT Bearer         |
| Real-time  | SignalR                                    |
| Charts     | Chart.js 4                                 |
| UI         | Bootstrap 5 + Bootstrap Icons              |
| Storage    | Blazored.LocalStorage (theme + session)    |
| Weather    | OpenWeatherMap API                         |

---

## Features

- **Authentication** — Register, Login, Logout with JWT + Role-based (Admin / Employee)
- **Customers** — Full CRUD with search
- **Leads** — Pipeline management (New → Contacted → Qualified → Converted / Lost)
- **Appointments** — Schedule with customer association
- **Invoices** — Create and track (Draft / Sent / Paid / Overdue)
- **Payments** — Record payments linked to invoices
- **Tasks** — Priority-based task management
- **Notifications** — Real-time via SignalR; mark as read / delete
- **File Uploads** — Upload documents per customer (PDF, Word, Excel, images)
- **Dashboard** — KPI cards + Chart.js analytics + live weather widget
- **Dark/Light Mode** — Persisted in localStorage
- **User Management** — Admin-only role & status management
- **Announcements** — Admin broadcasts via SignalR to all users

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (preview.4+)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (any edition, or LocalDB)
- [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (optional, for DB inspection)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) v17.9+ or VS Code with C# extension

---

## Setup & Run

### 1. Clone / Extract the Project

```
cd your-projects-folder
```

### 2. Verify Connection String

Open `RUDCRM.Server/appsettings.json` and confirm:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=RUDCRM;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Change `Server=localhost` to your SQL Server instance name if needed (e.g. `Server=.\SQLEXPRESS`).

### 3. Apply Database Migrations

The migration is already included. Run from the solution root:

```bash
cd RUDCRM.Server
dotnet ef database update
```

Or let the app auto-migrate on startup (already configured in `Program.cs`).

### 4. Run the Application

```bash
cd RUDCRM.Server
dotnet run
```

The server will:
- Apply migrations automatically
- Seed admin and employee accounts
- Seed sample customers, leads, tasks, and invoices
- Serve the Blazor WASM client

Open your browser at: **https://localhost:7xxx** (check console output for exact port)

---

## Default Login Credentials

| Role     | Email                   | Password     |
|----------|-------------------------|--------------|
| Admin    | admin@rudcrm.com        | Admin@123    |
| Employee | employee@rudcrm.com     | Employee@123 |

---

## Project Structure

```
RUDCRM/
├── RUDCRM.sln
├── RUDCRM.Client/          ← Blazor WebAssembly frontend
│   ├── Auth/               ← CustomAuthStateProvider (JWT)
│   ├── Components/         ← Reusable components (ConfirmDialog, LoadingSpinner)
│   ├── Layout/             ← MainLayout + NavMenu (sidebar)
│   ├── Pages/
│   │   ├── Authenticated/  ← Dashboard, Customers, Leads, etc.
│   │   └── Public/         ← Home, Login, Register, About
│   ├── Services/           ← HTTP client services (AuthService, CrmServices)
│   └── wwwroot/
│       ├── css/app.css     ← RUD-CRM design system
│       ├── js/charts.js    ← Chart.js integration
│       ├── js/interop.js   ← JS interop helpers (toast, sidebar)
│       ├── js/theme.js     ← Dark/light theme manager
│       └── index.html      ← SPA entry point
│
├── RUDCRM.Server/          ← ASP.NET Core Web API host
│   ├── Controllers/        ← REST API endpoints
│   ├── Data/               ← ApplicationDbContext + SeedData
│   ├── Hubs/               ← NotificationHub (SignalR)
│   ├── Migrations/         ← EF Core migration (InitialCreate)
│   ├── Services/           ← WeatherService (OpenWeatherMap)
│   ├── Uploads/            ← File upload storage directory
│   └── appsettings.json    ← Connection strings, JWT, Weather API
│
└── RUDCRM.Shared/          ← Shared Models and DTOs
    ├── Models/             ← ApplicationUser, Customer, Lead, etc.
    └── DTOs/               ← Data Transfer Objects for API communication
```

---

## API Endpoints

| Controller     | Base Route          | Auth     |
|----------------|---------------------|----------|
| Auth           | /api/auth           | Mixed    |
| Customers      | /api/customers      | Required |
| Leads          | /api/leads          | Required |
| Appointments   | /api/appointments   | Required |
| Invoices       | /api/invoices       | Required |
| Payments       | /api/payments       | Required |
| Tasks          | /api/tasks          | Required |
| Notifications  | /api/notifications  | Required |
| FileUpload     | /api/fileupload     | Required |
| Dashboard      | /api/dashboard      | Required |
| Weather        | /api/weather/{city} | Required |
| Admin          | /api/admin          | Admin    |

Swagger UI: `https://localhost:{port}/swagger`

---

## Database — RUDCRM

### Tables

- **AspNetUsers** (ApplicationUser + Identity tables)
- **Customers**
- **Leads**
- **Appointments**
- **Invoices**
- **Payments**
- **TaskItems**
- **Notifications**
- **UploadedDocuments**

---

## Key Configuration

### JWT Settings (appsettings.json)

```json
"JwtSettings": {
  "SecretKey": "RudCrmSuperSecretKey2024StudentProject!@#WithExtraLengthForSecurity$%^",
  "Issuer": "RUDCRM",
  "Audience": "RUDCRM",
  "ExpiryInDays": 7
}
```

### OpenWeatherMap API

```json
"WeatherApi": {
  "ApiKey": "2b24d5f7887b30be81eb9fdbb01fdfe0",
  "BaseUrl": "https://api.openweathermap.org/data/2.5"
}
```

API Key name: `rudcrm`  
Default city displayed on dashboard: **Islamabad**

---

## Troubleshooting

| Problem                              | Solution                                                             |
|--------------------------------------|----------------------------------------------------------------------|
| DB connection error                  | Check SQL Server is running; update `Server=` in connection string  |
| Migration fails                      | Run `dotnet ef database update` manually from `RUDCRM.Server/`      |
| Port conflict                        | Change `applicationUrl` in `Properties/launchSettings.json`         |
| Weather widget shows unavailable     | Check API key validity at openweathermap.org                         |
| Login fails after DB reset           | Re-run the app to re-seed admin credentials                          |

---

## License

This project is a semester/academic project. Not intended for production deployment.
