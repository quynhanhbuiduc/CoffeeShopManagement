# Café Pour La Vie - Coffee Shop Management System
Website quản lý bán hàng và tồn kho cho cửa hàng cà phê, hỗ trợ quản lý sản phẩm, nhân viên, đơn hàng, kho nguyên liệu và báo cáo doanh thu.


## Overview
Café Pour La Vie là hệ thống quản lý cửa hàng cà phê được xây dựng nhằm số hóa các nghiệp vụ bán hàng và quản lý kho.

Hệ thống hỗ trợ nhân viên thực hiện bán hàng tại quầy (POS), quản lý sản phẩm, theo dõi tồn kho nguyên liệu và cung cấp các báo cáo phục vụ việc quản lý kinh doanh.


## Objectives
- Xây dựng hệ thống bán hàng trực tuyến/quản lý tại quầy cho cửa hàng cà phê.
- Hỗ trợ quản lý sản phẩm và nguyên liệu.
- Tự động cập nhật tồn kho khi phát sinh đơn hàng.
- Quản lý thông tin nhân viên và tài khoản.
- Cung cấp báo cáo doanh thu.


## Features

### Authentication
- Đăng nhập hệ thống.
- Phân quyền Admin và Employee.

### Product Management
- Thêm, sửa, xóa sản phẩm.
- Quản lý danh mục sản phẩm.

### Sales Management
- Tạo đơn hàng.
- Thêm sản phẩm vào giỏ hàng.
- Thanh toán.
- Xuất hóa đơn.

### Inventory Management
- Quản lý nguyên liệu.
- Theo dõi số lượng tồn kho.
- Cảnh báo tồn kho thấp.

### Employee Management
- Quản lý nhân viên.
- Quản lý tài khoản.


## Technologies

### Backend
- ASP.NET Core MVC
- Entity Framework Core
- LINQ

### Database
- SQL Server

### Frontend
- HTML
- CSS
- JavaScript
- Bootstrap

### Tools
- Visual Studio
- Git/GitHub


## System Architecture
The project follows MVC architecture:

Controller:
- Handles user requests.
- Communicates with Services.

Service:
- Contains business logic.

Repository/Data Access:
- Handles database operations.

Database:
- SQL Server managed by Entity Framework Core.


## Database Design
Main entities:
- Account
- Employee
- Product
- Category
- Order
- OrderDetail
- Inventory
- ImportReceipt

Database relationship diagram:
<img width="1612" height="515" alt="erd" src="https://github.com/user-attachments/assets/723683c9-0e03-4d52-84b9-eca6ad949971" />

## System Use Cases
Main actors:
- Admin
- Employee

Main use cases:
- Login
- Manage products
- Manage employees
- Create order
- Manage inventory
- View reports


## Installation

### Requirements
- .NET 8 SDK
- SQL Server
- Visual Studio 2022

### Steps
1. Clone repository: git clone https://github.com/username/project.git

2. Update database connection string: appsettings.json

3. Run migration: Update-Database

4. Run project: dotnet run


## Contributors
Quỳnh Anh Bùi Đức
