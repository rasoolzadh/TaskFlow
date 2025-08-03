# TaskFlow: Modern Field Service Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

**TaskFlow** is a full-stack field service management solution built with the modern .NET 8 ecosystem. It connects office managers with field technicians through a secure, real-time system for job dispatch, tracking, and completion — all powered by ASP.NET MVC, Web API, and .NET MAUI.

---

## 🚀 Overview

TaskFlow streamlines operations for service-based businesses by replacing manual processes with a unified digital workflow. The system consists of three core components:

1.  **Central RESTful API:** Built with **ASP.NET Core Web API** and **Entity Framework Core** to manage data and business logic.
2.  **Manager’s Web Portal:** A responsive, feature-rich dashboard built with **ASP.NET MVC 8** and modern frontend libraries (Bootstrap, jQuery, or optional Razor Components).
3.  **Technician’s Mobile App:** A cross-platform field application built with **.NET MAUI** for real-time job updates and navigation.

This project demonstrates a clean, scalable architecture using the full power of .NET 8 across web, API, and mobile platforms.

---

## 🛠️ Technology Stack

*   **Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core 8, SQL Server
*   **Authentication:** JWT (JSON Web Tokens) with secure cookie or header-based flow
*   **Web Frontend:** ASP.NET MVC 8, HTML5, CSS3, Bootstrap 5, (optional: jQuery or minimal JavaScript for UX)
*   **Mobile Frontend:** .NET MAUI, CommunityToolkit.MVVM
*   **Shared Code:** .NET Class Library (DTOs, Enums)
*   **Architecture:** RESTful API, MVC Pattern (Web), MVVM (Mobile), JWT-based security.

---

## 🌟 Key Features

### For Managers & Admins (MVC Web Portal)
*   **User & Role Management:** Register and manage users with roles (Admin, Technician).
*   **Client Management:** Full CRUD operations for clients via intuitive forms and data tables.
*   **Job Creation & Assignment:** Create service jobs and assign them to technicians from a dropdown list.
*   **Dashboard Overview:** View all jobs with filters by status (New, Assigned, In Progress, Completed).
*   **Responsive Design:** Works seamlessly on desktops and tablets in the office or on the go.

### For Field Technicians (Mobile App)
*   **Personalized Job Feed:** View only the jobs assigned to you.
*   **Detailed Job View:** Access client info, job description, and status.
*   **Real-Time Status Updates:** Mark jobs as "In Progress" or "Completed" directly from the field.
*   **One-Tap Navigation:** Use the "Get Directions" button to launch native maps with the client’s address.
*   **Secure Login:** Biometric or password authentication with secure token storage.

---

## 🧩 Project Structure

The solution is cleanly organized into four projects:

---

## 🚀 Getting Started

### Prerequisites
*   .NET 8 SDK
*   Visual Studio 2022 (with .NET and .NET MAUI workloads) or VS Code
*   SQL Server (Express or Developer Edition)

### Installation & Setup
1.  Clone this repository.
2.  Open `TaskFlow.sln` in Visual Studio 2022.
3.  Restore NuGet packages for all projects.
4.  Update the `ConnectionStrings:DefaultConnection` in `TaskFlow.API/appsettings.json` to point to your SQL Server instance.
5.  In the `TaskFlow.API` project directory, run:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```
6.  Set `TaskFlow.API` as the startup project and run it to initialize the API (Swagger UI will launch).
7.  Set `TaskFlow.WebApp` as the startup project to run the MVC manager portal.
8.  Run `TaskFlow.MobileApp` on an Android/iOS simulator or device.

---

## 🔄 How It Works

1.  **Manager logs in** to the MVC web portal and creates a new job for a client.
2.  The job is stored in the database via the Web API.
3.  A **technician logs in** to the .NET MAUI app and sees the newly assigned job.
4.  The technician **updates the job status** (e.g., "Completed"), which is sent to the API.
5.  The manager sees the updated status **in real time** on the web dashboard.

This creates a seamless, paperless workflow from office to field.

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues or pull requests to improve functionality, UI, or documentation.

---

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
