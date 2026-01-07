# Course Management System 📚

**A robust backend system for managing educational courses, students, and instructors.** This project marks my second milestone in the ASP.NET Core Backend Path, focusing on modern software architecture and clean coding standards.

The system is designed using a decoupled architecture, splitting logic across a Console interface, a Web API, and a Core Logic layer.

---

## 🚀 Technologies & Tools

- **Framework:** .NET 10 (Latest)
- **IDE:** Visual Studio 2026
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core (Code First Approach)
- **API Testing:** Postman
- **Architecture:** N-Tier Architecture / Repository Pattern

---

## 🏗️ Project Structure

The solution consists of three main projects:

### 1. Core Logic (Project Library)
The backbone of the application containing:
- **Entities & Enums:** Defined models for Courses, Students, Instructors, and Enrollments
- **Data Configuration:** Fluent API configurations for database schema mapping
- **Services & Queries:** Business logic implementation and data retrieval logic
- **Migrations:** Database version control and history

### 2. ProjectApi (Web API)
A RESTful service that enables communication with the system via HTTP:
- **Controllers:** Handling API endpoints for all entities
- **DTOs (Data Transfer Objects):** Ensuring secure and efficient data exposure
- **Dependency Injection:** Managing service lifetimes for optimal performance

### 3. ConsoleApp
A command-line interface (CLI) for manual management, featuring:
- Structured menus for all entities
- Direct interaction with the service layer

---

## 🛠️ Key Technical Features

- **SOLID Principles:** Applied to ensure the code is maintainable and scalable
- **Dependency Injection (DI):** Heavily utilized to decouple components and improve testability
- **Clean Code & DRY:** Written with a focus on readability and minimizing logic duplication
- **Guard Clauses:** Implementation of a Guard class to handle validation and prevent runtime errors early
- **Repository Pattern:** Using Interfaces (ICourseRepository, etc.) to abstract data access logic

---

## 📸 Database & API Overview

- **Entities:** Students, Courses, Instructors, Enrollments
- **Relationships:** Many-to-Many and One-to-Many relations managed via EF Core
- **Endpoints:** Full CRUD (Create, Read, Update, Delete) operations tested and verified via Postman

---

## ⚙️ Getting Started

**1. Clone the repository:**
```bash
git clone https://github.com/your-username/CourseManagementSystem.git
```

**2. Configure Database:**
- Copy `ProjectApi/appsettings.Template.json` and rename it to `appsettings.json`
- Update the connection string in `appsettings.json` within both the **ProjectApi** and **ConsoleApp**

**3. Apply Migrations:**

Run the following command in the Package Manager Console:
```powershell
Update-Database
```

Or using .NET CLI:
```bash
dotnet ef database update --project Project
```

**4. Run:**
- Set either **ProjectApi** or **ConsoleApp** as the startup project and run

---

## 👨‍💻 About Me

I am a passionate **Backend Developer** student. This project reflects my progress in building modular, professional systems using ASP.NET Core. My goal is to master high-performance web architectures.

---

## 📄 License

This project is licensed under the MIT License.

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the issues page.
