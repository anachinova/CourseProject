# CourseProject

Course project for the discipline **Object-Oriented Programming**.

## Topic

**Advertisement system for buying, selling and exchanging goods**

## Description

CourseProject is an ASP.NET Core MVC web application that allows users to browse, search, create, edit and delete advertisements.

The project demonstrates the use of object-oriented programming principles, MVC architecture, SOLID principles, unit testing, JSON serialization and local SQLite storage.

## Main Features

- Browse advertisements
- View advertisement details
- Search advertisements by keyword
- Filter advertisements by category, city and price
- Sort advertisements by price and date
- User login and registration pages
- Create, edit and delete advertisements
- Admin panel
- View users in admin panel
- Manage advertisements in admin panel
- Session-based authentication
- Role-based access
- Unit testing with MSTest

## User Roles

### Guest

- View advertisements
- View advertisement details
- Search and filter advertisements
- Open registration page

### Registered User

- Log in
- Create advertisements
- Edit own advertisements
- Delete own advertisements
- View personal advertisements

### Administrator

- Log in as administrator
- Open admin panel
- View system statistics
- View all advertisements
- Delete advertisements
- View registered users

## Technologies

- C#
- .NET 8
- ASP.NET Core MVC
- Razor Views
- HTML
- CSS
- Bootstrap
- LINQ
- JSON
- SQLite
- MSTest
- GitHub

## Object-Oriented Programming

The project demonstrates:

- Encapsulation
- Static class members
- Interfaces
- Abstract classes
- Inheritance
- Polymorphism
- Delegates and events
- Serialization and deserialization
- Generic collections
- LINQ queries

## SOLID Principles

The project demonstrates SOLID principles:

- **SRP** — each class has one responsibility
- **OCP** — storage logic can be extended through interfaces
- **LSP** — `Admin` and `RegisteredUser` can be used as `User`
- **ISP** — interfaces are separated by functionality
- **DIP** — services depend on abstractions, not concrete implementations

## Project Structure

```text
CourseProject/
│
├── Controllers/
│   ├── AdminController.cs
│   ├── AdvertisementsController.cs
│   ├── AuthController.cs
│   └── HomeController.cs
│
├── Domain/
│   ├── DTOs/
│   ├── Enums/
│   ├── Interfaces/
│   ├── Models/
│   ├── Services/
│   └── Storage/
│
├── Views/
│   ├── Admin/
│   ├── Advertisements/
│   ├── Auth/
│   ├── Home/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
│
├── CourseProject.Tests/
│   ├── AdvertisementManagerTests.cs
│   ├── DomainValidationTests.cs
│   └── JsonStorageTests.cs
│
└── README.md

# Main Pages

- Home page
- Advertisement details page
- Login page
- Registration page
- My advertisements page
- Create advertisement page
- Edit advertisement page
- Admin panel
- Admin advertisements page
- Admin users page

---

# Data Storage

The project uses JSON serialization through `JsonStorage`.

SQLite database is also used as a lightweight local database stored in a `.db` file.

SQLite is suitable for this course project because it does not require a separate database server and is easy to integrate into a small ASP.NET Core MVC application.

---

# Unit Testing

Unit tests were created using **MSTest**.

Tested functionality:

- Advertisement creation
- Advertisement deletion
- Search functionality
- Price filtering
- Domain validation
- JSON storage
- Serialization and deserialization

---

# GitHub Workflow

GitHub was used for:

- Version control
- Commit history
- Milestones
- Issues
- Tasklists
- Project planning

---

# How to Run

## 1. Clone repository

```bash
git clone https://github.com/anachinova/CourseProject.git
