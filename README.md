# Inventory Management System

A cross-platform inventory management application developed using C# and .NET MAUI. The application provides functionality for managing products and categories, monitoring stock levels, tracking inventory activities, and viewing inventory information through a dashboard.

**Status:** Completed

## Overview

The Inventory Management System is a personal software development project built to provide a practical solution for inventory tracking and management.

The application allows users to manage products and categories, monitor stock quantities, configure low-stock thresholds, record inventory activities, and view important inventory information through a dashboard.

The project demonstrates practical experience with C#, object-oriented programming, application architecture, database management, Entity Framework Core, dependency injection, automated testing, and responsive user interface development.

## Features

### Dashboard
- Displays an overview of inventory information.
- Shows total product and category counts.
- Displays the number of low-stock items.
- Provides a recent inventory activity overview.

### Product Management
- Create, edit, and delete products.
- Associate products with categories.
- Manage product names, prices, and stock quantities.
- Configure individual low-stock thresholds.
- Filter products by stock status.
- Identify products that are in stock, running low, or out of stock.
- Display product images.
- Upload custom product images.
- Use a default image when no product image is selected.
- Store product image paths alongside product information.
- Confirm before deleting products.

### Category Management
- Create, edit, and delete product categories.
- Organise products by category.
- Track categories using unique identifiers.
- Prevent deletion of categories associated with existing products.

### Inventory Management
- Add and remove stock quantities.
- Validate stock adjustments.
- Prevent invalid stock operations.
- Record inventory activities and changes.

### Database Management
- Microsoft SQL Server integration.
- Entity Framework Core for database operations.
- Database context and entity relationships.
- Database migrations for managing schema changes.
- Dependency injection for managing application dependencies.
- Persistent storage of inventory data.

### Validation and Testing
- Business rule validation through the `InventoryService`.
- Input validation for product and category names.
- Maximum name length constraints.
- Automated unit testing using MSTest.
- Entity Framework Core InMemory database for isolated testing.

### User Interface
- Dashboard, product management, and category management pages.
- Responsive layouts for desktop and mobile screen sizes.
- Responsive navigation that adapts to different screen sizes.
- Refined and polished user interface.
- Tabular product information.
- Product images and stock-status indicators.
- Dedicated pages and popups for application operations.
- Confirmation dialogs for destructive actions.

## Technologies Used

| Technology | Purpose |
|---|---|
| **C#** | Application logic and business rules |
| **.NET MAUI** | Cross-platform application framework |
| **XAML** | User interface layout and components |
| **Entity Framework Core** | Object-relational mapping and database operations |
| **Microsoft SQL Server** | Relational database management |
| **Dependency Injection** | Application service and dependency management |
| **MSTest** | Automated unit testing |
| **Git** | Version control |
| **GitHub** | Source code hosting and project management |

## Project Structure

The solution is organised into separate projects to maintain separation of concerns:

```text
Inventory-Mangement-System/
├── Inventory Mangement System/  # Main application and UI
├── InventorySystem.Core/        # Core models and services
├── InventorySystem.Migrations/  # Entity Framework Core migrations
├── InventorySystem.Test/        # Automated unit tests
├── .gitignore
├── .gitattributes
└── Inventory Mangement System.slnx
```

The core project contains the application's models and business logic, keeping them separate from the user interface. Dedicated migration and test projects provide locations for database schema management and automated testing.

## Getting Started

### Prerequisites

Before running the application, ensure you have:

- Windows development environment.
- Visual Studio with the .NET MAUI development workload installed.
- The .NET SDK version required by the solution.
- Microsoft SQL Server.
- A configured SQL Server connection string.

### Installation

1. Clone the repository:

```bash
git clone https://github.com/MarkTilt/Inventory-Mangement-System.git
```

2. Open the cloned repository directory.

3. Open `Inventory Mangement System.slnx` in Visual Studio.

4. Restore the solution's NuGet dependencies.

5. Configure the `INVENTORY_DB_CONNECTION` environment variable with your own SQL Server connection string.

6. Ensure your SQL Server instance is running and accessible.

7. Apply the required Entity Framework Core migrations to create the database schema.

8. Set the main application project as the startup project.

9. Build and run the application using a supported target.

### Database Configuration

**Important:** The application retrieves its SQL Server connection string from an environment variable named `INVENTORY_DB_CONNECTION`.

Database credentials are not included in the repository. Anyone cloning the project must configure this environment variable with their own SQL Server connection string before running the application.

Example connection string format:

```text
Server=YOUR_SERVER;
Database=InventoryDb;
User Id=YOUR_USERNAME;
Password=YOUR_PASSWORD;
TrustServerCertificate=True;
```

Replace the example values with the details of your own SQL Server instance.

The environment variable must be available to the application when it starts. If it is not configured correctly, the application will not be able to establish its database connection.

### Running Tests

The solution includes a dedicated test project containing automated unit tests.

To execute the tests from the solution directory, run:

```bash
dotnet test
```

The tests use the Entity Framework Core InMemory provider to create isolated database instances for testing without requiring a connection to a running SQL Server database.

## Development Notes

The application was developed as a personal learning and portfolio project, with an emphasis on applying software development concepts in a practical environment.

The development process included implementing application logic, designing the database structure, integrating Entity Framework Core, writing automated tests, debugging, and refining the user interface.

The application supports both desktop and mobile layouts, with responsive navigation and layouts that adapt to different screen sizes.

Although the main application is complete, additional features and improvements may be introduced in the future.

## Project Purpose

This project demonstrates practical experience with:

- C# and object-oriented programming.
- Application architecture and separation of concerns.
- Service-based application design.
- Database design and relational data management.
- Entity Framework Core and database migrations.
- Dependency injection.
- Business logic and data validation.
- Automated unit testing.
- File handling and image management.
- Cross-platform development using .NET MAUI.
- Responsive user interface development.
- Git and GitHub version control.

The project represents a practical application of the software development concepts learned while building toward a career in C# and .NET development.

## Author

**MarkTilt**

GitHub: [@MarkTilt](https://github.com/MarkTilt)

Repository: [Inventory Management System](https://github.com/MarkTilt/Inventory-Mangement-System)
