# EventsBook Documentation

- [EventsBook Documentation](#eventsbook-documentation)
  - [Introduction](#introduction)
  - [Project Structure](#project-structure)
    - [Key Folders](#key-folders)
  - [Key Features](#key-features)
  - [Prerequisites](#prerequisites)
  - [Setup and Configuration](#setup-and-configuration)
    - [Configuring SQL Server](#configuring-sql-server)
  - [📦 EF Core Setup \& Database Creation (for .NET 8 Projects)](#-ef-core-setup--database-creation-for-net-8-projects)
    - [✅ Step 1: Install EF Core CLI Tool (Version 8.0.15)](#-step-1-install-ef-core-cli-tool-version-8015)
    - [✅ Step 2: Restore the Project Dependencies](#-step-2-restore-the-project-dependencies)
    - [✅ Step 3: Navigate to the Project Folder](#-step-3-navigate-to-the-project-folder)
    - [✅ Step 4: Apply the Migrations and Create the Database](#-step-4-apply-the-migrations-and-create-the-database)
    - [🧾 Requirements](#-requirements)
    - [🆘 Common Issues \& Fixes](#-common-issues--fixes)
  - [Packages and Dependencies](#packages-and-dependencies)
  - [API Endpoints](#api-endpoints)
    - [Authentication](#authentication)
    - [Event Management](#event-management)
  - [Additional Notes](#additional-notes)
    - [Running the Application](#running-the-application)
  - [Accounts (Admin Account)](#accounts-admin-account)
  - [RunTime Screen shots](#runtime-screen-shots)
    - [Admin](#admin)
    - [Regular User](#regular-user)

---

## Introduction

**EventsBook** is a web application built using ASP.NET Core (.NET 8) that allows users to manage and register for events. It includes features like user authentication, event registration, and JWT-based API authentication.

---

## Project Structure

The project is organized as follows:

```
.gitignore
appsettings.json
eventsBook.sln
Controllers/
Models/
Views/
Migrations/
wwwroot/
Areas/
```

### Key Folders

- **Controllers**: Contains the logic for handling HTTP requests.
- **Models**: Defines the data structure and database context.
- **Views**: Contains Razor views for rendering the UI.
- **Migrations**: Tracks database schema changes.
- **wwwroot**: Contains static files like CSS, JavaScript, and images.

---

## Key Features

- User authentication and authorization using ASP.NET Identity.
- Event management (CRUD operations).
- JWT-based API authentication.
- Role-based access control for administrators.
- Pagination for event listings.

---

## Prerequisites

Before running the application, ensure you have the following installed:

- **.NET 8 SDK**: The application is built using .NET 8.
- **SQL Server**: The application uses SQL Server as its database.

---

## Setup and Configuration

### Configuring SQL Server

The application uses SQL Server as its database. Follow these steps to configure SQL Server to match the appsettings.json file:

1. **Ensure SQL Server is Installed and Running**:
   - Install SQL Server if it’s not already installed.
   - Start the SQL Server service.

2. **Set Authentication**:
   - The connection string in `appsettings.json` uses **Integrated Security**. Ensure your SQL Server is configured to allow Windows Authentication.
  
3. **Don't forget to change The appsetting.json connection string to much the SQL Server you run**
   - ![alt text](image.png)
   - This My SQL Server Name Which Match the appsttings.json so Edit appsettings.json to much your server name
   - ```json
       "ConnectionStrings": {
        "Default": "server=PRIME\\SQLEXPRESS;user=PRIME\\Prime11;password=;Database=areeb;Integrated Security=True;TrustServerCertificate=True;"
      },
     ```

## 📦 EF Core Setup & Database Creation (for .NET 8 Projects)

> 🧠 **Goal:** Apply existing EF Core migrations to create the database — no mistakes, no confusion.

---

### ✅ Step 1: Install EF Core CLI Tool (Version 8.0.15)

```bash
dotnet tool install --global dotnet-ef --version 8.0.15
```

> Already installed? Use this to upgrade:

```bash
dotnet tool update --global dotnet-ef --version 8.0.15
```

---

### ✅ Step 2: Restore the Project Dependencies

In the root directory of the solution:

```bash
dotnet restore
```

---

### ✅ Step 3: Navigate to the Project Folder

Go to the folder containing the `.csproj` file with your DbContext and migrations:

```bash
cd eventsBook
```

---

### ✅ Step 4: Apply the Migrations and Create the Database

```bash
dotnet ef database update
```

---

### 🧾 Requirements

- `.NET 8 SDK` installed
- SQL Server must be running, and the connection string in appsettings.json must be correctly configured

    ```
        ⚠️ Make sure to edit the Server= part in the connection string to match the actual SQL Server instance running on your machine
    ```

- You must have the migrations already committed in the project

---

### 🆘 Common Issues & Fixes

| Issue                      | Solution                                                              |
| -------------------------- | --------------------------------------------------------------------- |
| `dotnet ef` not recognized | Run `dotnet tool install --global dotnet-ef` again                    |
| Wrong .csproj path         | Make sure you `cd` into the correct folder before running `dotnet ef` |



- **Default Connection**: Connects to the main database (`areeb`).
- **JWT Secret Key**: Used for generating JWT tokens. Ensure this key is kept secure.

---

## Packages and Dependencies

The project uses the following NuGet packages:

| Package                                             | Purpose                                |
| --------------------------------------------------- | -------------------------------------- |
| `Microsoft.EntityFrameworkCore.SqlServer`           | EF Core provider for SQL Server        |
| `Microsoft.EntityFrameworkCore.Tools`               | Needed for migrations & CLI commands   |
| `Microsoft.EntityFrameworkCore.Design`              | Enables design-time services for tools |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | If you're using ASP.NET Identity       |
| `Microsoft.AspNetCore.Authentication.JwtBearer`     | If you're using JWT-based auth         |


Ensure these packages are installed and up-to-date. If any issues arise, you can reinstall them using the following commands:

```sh
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.15
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.15
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.15
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.15
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.15
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.15
```

---

## API Endpoints

### Authentication

- **POST `/api/login`**  
  Allows users to log in and receive a JWT token.  
  **Request Body**:

  ```json
  {
    "Email": "user@example.com",
    "Password": "password123"
  }
  ```

  **Response**:

  ```json
  {
    "token": "your-jwt-token"
  }
  ```

### Event Management

- **POST `/api/login`**  
  body Ex. 
  ```json
  {
    "Username":"admin@example.com",
    "Password":"Admin@123",
    "Email":"admin@example.com"
  }
  ```
  Receive:
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI3ODA1NGRlMi1jNzNiLTQwOGItYjJlYS1hMzkyM2NkOGFjODUiLCJlbWFpbCI6WyJhZG1pbkBleGFtcGxlLmNvbSIsImFkbWluQGV4YW1wbGUuY29tIl0sIm5hbWVpZCI6Ijc4MDU0ZGUyLWM3M2ItNDA4Yi1iMmVhLWEzOTIzY2Q4YWM4NSIsInVuaXF1ZV9uYW1lIjoiYWRtaW5AZXhhbXBsZS5jb20iLCJuYmYiOjE3NDcwMjM2NjAsImV4cCI6MTc0NzAyNzI2MCwiaWF0IjoxNzQ3MDIzNjYwfQ.GOv4KraJ0q0mwvXFhKXp3g1KffDDZecZHl1J_i0QrdI"
  }
  ```
  Add the following headers before 
  `Authorization: Bearer <token-from-login>`
- **GET `/api/details`**  
  Retrieves details of a specific event.  
  **Query Parameters**:  
  `i` - Event ID.

- **GET `/api/Book`**  
  Registers the current user for an event.  
  **Query Parameters**:  
  `evId` - Event ID.

- **GET `/api/MyEvents`**  
  Retrieves a paginated list of events the user is registered for.  
  **Query Parameters**:  
  `page` - Page number (default: 1).  
  `pageSize` - Number of items per page (default: 10).

---

## Additional Notes

### Running the Application

1. **Access the Application**:
   Open your browser and navigate to `https://localhost:2003`.


- you can edit the port from appsetting.json
```json
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://localhost:2003"//change to any port you prefer
      }
    }
  },
```

## Accounts (Admin Account)

- There is not Default account for Regular User (you should Register your Account)
- There is a Default account for Admin with the following credintails
    - Email:`admin@example.com`
    - Password:`Admin@123`

## RunTime Screen shots
### Admin

- ![alt text](RunTimeScreen/image.png)
- ![alt text](RunTimeScreen/image-1.png)
- ![alt text](RunTimeScreen/image-2.png)
- ![alt text](RunTimeScreen/image-3.png)
- ![alt text](RunTimeScreen/image-4.png)
- ![alt text](RunTimeScreen/image-5.png)
- ![alt text](RunTimeScreen/image-6.png)
- ![alt text](RunTimeScreen/image-7.png)

### Regular User
- ![alt text](RunTimeScreen/image-8.png)

- on booking ![alt text](RunTimeScreen/image-9.png)
- ![alt text](RunTimeScreen/image-10.png)
- More details ![alt text](RunTimeScreen/image-11.png)
- My Events ![alt text](RunTimeScreen/image-12.png)

> Application Support Events To have Multiple Images This