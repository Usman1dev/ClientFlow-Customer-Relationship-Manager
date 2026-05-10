# RUD-CRM (Relationship Understanding & Development CRM)

RUD-CRM is a modern, semester-level Customer Relationship Management system built using Blazor WebAssembly (.NET 10), ASP.NET Core Web API, and Entity Framework Core with SQL Server. It is designed to manage customers, leads, appointments, invoices, payments, and tasks, utilizing advanced web features like file uploads, real-time notifications via SignalR, Chart.js for analytics, and OpenWeatherMap API integration.

## Technologies Used

- **Framework:** Blazor Server (.NET 10), ASP.NET Core
- **Frontend:** Bootstrap 5, HTML/CSS, Chart.js, JSInterop
- **Database:** Entity Framework Core, SQL Server (SSMS Local Database)
- **Authentication:** ASP.NET Core Identity with JWT Bearer / Cookie auth
- **External API:** OpenWeatherMap API

## Features

- **Authentication:** Register, Login, Logout, Role-based authorization (Admin, Employee).
- **Core Entities:** Full CRUD operations for Customers, Leads, Appointments, Invoices, Payments, Tasks, Notifications, and File Uploads.
- **External API Integration:** Dashboard weather widget utilizing OpenWeatherMap API.
- **Advanced Web Features:** 
  - File Uploads (Customer documents with metadata).
  - Interactive Charts (Chart.js for revenue, customer growth, and leads).
  - Real-time Notifications (SignalR built-in via Blazor Server).
  - Local Storage (Theme persistence).
- **UI/UX:** Responsive Bootstrap 5 layout with dark/light mode, sidebar navigation, dynamic dashboard cards, and toast notifications.

## Project Structure

This project has been streamlined into a **single-project Blazor Server architecture**:
- `RUDCRM.Server` - Contains the UI components, Database Context, API Endpoints, and Backend Services all in one place.

## Database Setup (SSMS)

To set up the database using SQL Server Management Studio (SSMS):

1. Ensure SQL Server is running locally (`localhost`).
2. Open **SQL Server Management Studio**.
3. Create a new database named `RUDCRM` if you prefer to run scripts manually, or simply execute the generated SQL script which will apply the migrations:
   - Go to `File > Open > File...` and select `db_setup.sql` located in the root of the project.
   - Click **Execute** to create all tables, relationships, and indices.
4. Alternatively, you can run Entity Framework Core migrations from the command line:
   ```bash
   cd RUDCRM.Server
   dotnet ef database update
   ```

## Running the Application

1. Open a terminal in the root directory.
2. Build the solution to ensure all dependencies are restored:
   ```bash
   dotnet build
   ```
3. Run the backend server project (which will also host the Blazor client):
   ```bash
   cd RUDCRM.Server
   dotnet run
   ```
4. Navigate to the URL provided in the console (typically `https://localhost:5001` or `http://localhost:5000`) to access the RUD-CRM dashboard.

## Important Note

- **OpenWeatherMap API:** The application is pre-configured with the provided OpenWeatherMap API key (`2b24d5f7887b30be81eb9fdbb01fdfe0`).
- **Initial User:** Once the database is created, you can register a new user from the UI to start exploring the system.

## Authors
Developed for the Relationship Understanding & Development (RUD) CRM university project.
