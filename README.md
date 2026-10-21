# Inventory Management System

A desktop inventory management application built with C# and .NET MAUI. This project focuses on product and category management, stock monitoring, and presenting inventory information through a dashboard.

**Status:** In development

## Overview

The Inventory Management System is a personal software development project designed to simplify inventory tracking and management. It provides a graphical interface for managing products and categories, monitoring stock levels, and viewing inventory information.

The project is also an opportunity to apply C# programming, object-oriented programming, application architecture, UI development, and automated testing in a practical application.

## Features

### Dashboard
- Overview of inventory information.
- Summary counts for inventory data.
- Recent activity display.

### Product Management
- Manage inventory products.
- Associate products with categories.
- Track product prices and stock quantities.
- Configure low-stock thresholds for individual products.
- Filter products by stock status.
- Confirm before deleting products.
- Identify products that are in stock, running low, or out of stock.

### Category Management
- Create and manage product categories.
- Organize products by category.
- Track category information using unique identifiers.

### User Interface
- Navigation between the dashboard, products, and categories.
- Tabular product information.
- Stock-status indicators.
- Dedicated pages for different areas of the application.

## Technologies Used

- **C#** — application logic and business rules.
- **.NET MAUI** — cross-platform application UI framework.
- **XAML** — UI layout and component definitions.
- **.NET** — application platform and tooling.
- **Git and GitHub** — version control and source code management.
- **Automated testing** — a dedicated test project for verifying application logic.

## Project Structure

The solution is organized into separate projects:


Inventory-Mangement-System/
├── Inventory Mangement System/  # Main application and UI
├── InventorySystem.Core/        # Core models and services
├── InventorySystem.Test/        # Automated tests
├── .gitignore
├── .gitattributes
└── Inventory Mangement System.slnx


The core project separates important application logic from the user interface, while the test project provides a dedicated place for automated tests.

## Getting Started

### Prerequisites

- Windows development environment.
- Visual Studio with the .NET MAUI development workload installed.
- The .NET SDK version required by the solution.

### Installation

1. Clone the repository:


   git clone https://github.com/MarkTilt/Inventory-Mangement-System.git


2. Open the cloned project directory.

3. Open `Inventory Mangement System.slnx` in a compatible version of Visual Studio.

4. Restore the solution's NuGet dependencies.

5. Set the main application project as the startup project.

6. Build and run the application using a supported target.

### Running Tests

The solution includes a dedicated test project. To run its tests from the solution directory, use:


dotnet test


Ensure the required .NET SDKs and workloads are installed before building the application.

## Current Development

The project is actively being developed, with planned improvements including:

- Improving the layout for mobile screen sizes.
- Integrating Entity Framework Core for database persistence.
- Displaying product images.
- Allowing users to select images when creating products.
- Further improving the UI and user experience.

These improvements will be introduced as development progresses.

## Purpose

This project demonstrates practical application development with C#, including data modeling, service-based organization, user interface development, inventory-related business logic, and testing.

It is an ongoing portfolio project intended to grow as additional technologies and software engineering practices are learned and applied.

## Author

**MarkTilt**

GitHub: [@MarkTilt](https://github.com/MarkTilt)
