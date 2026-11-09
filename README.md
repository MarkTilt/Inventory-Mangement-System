# Inventory Management System

A desktop inventory management application built with C# and .NET MAUI. The application focuses on product and category management, stock monitoring, database persistence, and presenting inventory information through a dashboard.

**Status:** In development

## Overview

The Inventory Management System is a personal software development project designed to simplify inventory tracking and management.

It provides a graphical interface for managing products and categories, monitoring stock levels, recording inventory activities, and viewing inventory information.

The project applies C# programming, object-oriented programming, application architecture, database management, UI development, dependency injection, and automated testing in a practical application.

## Features

### Dashboard
- Overview of inventory information.
- Summary counts for products, categories, and low-stock items.
- Recent activity display.

### Product Management
- Create, edit, and delete inventory products.
- Associate products with categories.
- Track product prices and stock quantities.
- Configure low-stock thresholds for individual products.
- Filter products by stock status.
- Confirm before deleting products.
- Identify products that are in stock, running low, or out of stock.

### Category Management
- Create, edit, and delete product categories.
- Organize products by category.
- Track category information using unique identifiers.
- Prevent deletion of categories that are associated with existing products.

### Inventory Management
- Add and remove stock quantities.
- Validate stock operations.
- Prevent invalid stock adjustments.
- Track inventory activities and changes.

### Database Management
- SQL Server database integration.
- Entity Framework Core for database operations.
- Database context and entity relationships.
- Dependency injection for managing application dependencies.
- Persistent storage for inventory data.

### User Interface
- Navigation between the dashboard, products, and categories.
- Tabular product information.
- Stock-status indicators.
- Dedicated pages and popups for different application operations.

## Technologies Used

- **C#** — application logic and business rules.
- **.NET MAUI** — application UI framework.
- **XAML** — UI layout and component definitions.
- **Entity Framework Core** — ORM and database operations.
- **Microsoft SQL Server** — relational database management.
- **Dependency Injection** — managing application dependencies.
- **MSTest** — automated testing.
- **Git and GitHub** — version control and source code management.

## Project Structure

The solution is organized into separate projects:

```text
Inventory-Mangement-System/
├── Inventory Mangement System/  # Main application and UI
├── InventorySystem.Core/        # Core models and services
├── InventorySystem.Test/        # Automated tests
├── .gitignore
├── .gitattributes
└── Inventory Mangement System.slnx
```

The core project separates important application logic from the user interface, while the test project provides a dedicated place for automated tests.

## Getting Started

### Prerequisites

- Windows development environment.
- Visual Studio with the .NET MAUI development workload installed.
- The .NET SDK version required by the solution.
- Microsoft SQL Server.
- A configured database connection string.

### Installation

1. Clone the repository:

```bash
git clone https://github.com/MarkTilt/Inventory-Mangement-System.git
```

2. Open the cloned project directory.

3. Open `Inventory Mangement System.slnx` in a compatible version of Visual Studio.

4. Restore the solution's NuGet dependencies.

5. Configure your SQL Server database connection string.

6. Ensure the database is created and the required Entity Framework Core migrations have been applied.

7. Set the main application project as the startup project.

8. Build and run the application using a supported target.

### Running Tests

The solution includes a dedicated test project. To run its tests from the solution directory, use:

```bash
dotnet test
```

Ensure the required .NET SDKs and workloads are installed before building the application.

## Current Development

The project is actively being developed, with planned improvements including:

- Improving the layout for mobile screen sizes.
- Displaying product images.
- Allowing users to select and upload images when creating products.
- Further improving the UI and user experience.

These improvements will be introduced as development progresses.

## Purpose

This project demonstrates practical application development with C#, including:

- Object-oriented programming and data modelling.
- Service-based application architecture.
- Database design and Entity Framework Core.
- Dependency injection.
- Inventory-related business logic and validation.
- Automated testing.
- Desktop application development with .NET MAUI.

It is an ongoing portfolio project intended to grow as additional technologies and software engineering practices are learned and applied.

## Author

**MarkTilt**

GitHub: [@MarkTilt](https://github.com/MarkTilt)
