# Cooks Corner E-Commerce Web Application

An ASP.NET Core web application for managing an e-commerce web app with features like user authentication, product management, and cart functionality. 
The application is deployed on **Azure App Service** and uses **Azure SQL Database** for data storage.

Live application: [Cooks Corner](https://coookingcorner-hgdtfjepg4anfpfh.canadacentral-01.azurewebsites.net/)

## Features

- **User Authentication**:
  - Admin users can manage products (CRUD operations).
  - Customers can browse products, add/delete/update items in their cart, and view their cart total.

- **Product Management**:
  - Admins can add, edit, delete, and view products.

- **Shopping Cart**:
  - Customers can add products to their cart and view the total price.

- **Responsive UI**:
  - Optimized for desktop and mobile devices.

- **Deployed to Azure**:
  - Hosted on Azure App Service.
  - Azure SQL Database as the backend.

---

## Prerequisites

- **.NET SDK 8.0** or higher.
- **Azure CLI** (if deploying manually).
- **SQL Server** (local development or Azure SQL Database for production).

---

## Installation

### 1. Clone the Repository
```bash
git clone https://github.com/sp-ho/eCommerceCookingCorner.git
cd shoppingcart
```

### 2. Set Up the Database
Ensure you have SQL Server running locally or use Azure SQL Database.
Update the connection string in appsettings.json

```bash
"ConnectionStrings": {
    "ShoppingCartContext": "YourConnectionStringHere"
}
```

### 3. Apply Migrations
Run the following commands to create the database schema:

Command prompt:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Package Manager Console:
```bash
Add-Migration InitialCreate
Update-Database
```

### 4. Run the Application
Start the app locally. The app will be hosted at `http://localhost:5000`.

---

## Deployment

### Deploy to Azure App Service
1. Create an Azure App Service and Azure SQL Database.
2. Update the connection string in Azure Configuration Settings.  
3. Deploy using GitHub Actions:
    - Add the Azure publish profile to your repository secrets.
    - Use the provided .github/workflows/deploy.yml for CI/CD.

### Environment Variables
Configure the following environment variables for secure deployment:

AZURE_SQL_CONNECTION_STRING: Connection string for the Azure SQL Database
AZURE_WEBAPP_PUBLISH_PROFILE: Publish profile for Azure App Service

---

## Technologies Used

- ASP.NET Core (8.0): Web application framework.
- Entity Framework Core: Database access and migrations.
- Azure App Service: Hosting the web application.
- Azure SQL Database: Data storage.
- GitHub Actions: CI/CD pipeline.
- Bootstrap: Responsive UI design.