# FurnitureWebApp - personal project using ASP.NET Core 3.1

## Technologies
- ASP.NET Core 3.1
- Entity Framework Core 5.0.17
## Install Tools
- .NET Core SDK 3.1
- Git client
- Visual Studio 2022
- SQL Server 2019
## How to configure and run
- Clone code from Github: git clone https://github.com/NMS1010/FurnitureWebApp
- Open solution FurnitureWebApp.sln in Visual Studio 2022
- Install full required packages 
- Set startup project is FurnitureWebApp.Domain
- Change connection string in appsetting.json in FurnitureWebApp.Data project
- Open Tools --> Nuget Package Manager -->  Package Manager Console in Visual Studio 2022
- Run Update-database and Enter to update database for your local machine
- Change database connection in appsettings.Development.json and appsettings.json in FurnitureWebApp.WebApp. FurnitureWebApp.AdminWebApp, FurnitureWebApp.BackendWebAPI project.
- Replace BaseAddress in appsettings.json and appsettings.Development.json of FurnitureWebApp.WebApp, FurnitureWebApp.AdminWebApp by address of FurnitureWebApp.BackendWebAPI when running
- You need to change 3 projects to self-host profile.
- Set multiple run project: Right click to Solution, choose Properties and set Multiple Project, choose Start for 3 Projects: BackendWebAPI, WebApp and AdminWebApp.
- Choose profile to run or press F5
## Template
- Admin page: Ekka - Admin Dashboard eCommerce
- Website: Furea - Furniture Shop
## Deploy website to Somee
- Admin dashboard: http://admin-fursshop.somee.com/admin/home
- Website: http://www.fursshop.somee.com/home