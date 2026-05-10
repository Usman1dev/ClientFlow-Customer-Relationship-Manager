# ClientFlow-CRM

ClientFlow-CRM is a modern, semester-level Customer Relationship Management system built using Blazor Server (.NET 10), ASP.NET Core Web API, and Entity Framework Core with SQL Server. It is designed to manage customers, leads, appointments, invoices, payments, and tasks.

## Tech Stack

| Layer      | Technology                                 |
|------------|--------------------------------------------|
| Frontend   | Blazor Server (.NET 10)                    |
| Backend    | ASP.NET Core (.NET 10)                     |
| Database   | SQL Server (via SSMS)                      |
| ORM        | Entity Framework Core 10                   |
| Auth       | ASP.NET Core Identity + JWT Bearer         |
| Real-time  | SignalR                                    |
| Charts     | Chart.js 4                                 |
| UI         | Bootstrap 5 + Bootstrap Icons              |

## Features

- **Authentication:** Register, Login, Logout, Role-based authorization (Admin, Employee).
- **Core Entities:** Full CRUD operations for Customers, Leads, Appointments, Invoices, Payments, Tasks, Notifications, and File Uploads.
- **External API Integration:** Dashboard weather widget utilizing OpenWeatherMap API.
- **Advanced Web Features:** File Uploads, Interactive Charts, Real-time Notifications, Local Storage.
- **UI/UX:** Responsive Bootstrap 5 layout with dark/light mode, sidebar navigation, dynamic dashboard cards.

## Database Setup (SSMS)

To set up the database using SQL Server Management Studio (SSMS):

1. Ensure SQL Server is running locally.
2. Open **SQL Server Management Studio**.
3. Create a new database named RUDCRM.
4. Open db_setup.sql in SSMS and execute it to create all tables.

## Running the Application

1. Open a terminal in the root directory.
2. Run the backend server project:
   ``bash
   cd ClientFlowCRM.Server
   dotnet run
   ``
3. Navigate to the URL provided in the console to access the ClientFlow-CRM dashboard.

## Default Login Credentials

| Role     | Email                   | Password     |
|----------|-------------------------|--------------|
| Admin    | admin@rudcrm.com        | Admin@123    |
| Employee | employee@rudcrm.com     | Employee@123 |

*(Note: While the CRM is called ClientFlow-CRM, the database is named RUDCRM and default emails use @rudcrm.com as per configuration).*

