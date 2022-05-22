![logo](wwwroot/img/logo.svg)  
# AIT Business management system   


## Table of Contents
* [Introduction](#introduction)
* [Video Demo](#video-demo)
* [Technologies Used](#technologies-used)
* [Features](#features)
* [Deployment](#deployment)
* [How to install and run the project](#how-to-install-and-run-the-project)
* [Usage](#usage)
* [Acknowledgements](#acknowledgements)
* [AIT Team](#ait-team)
* [License](#license)

<!-- * [License](#license) -->



## Introduction
In each company, there is a separate way of doing business, but the operations are relatively similar (HR, product management, finance, engineering,...). These operations to do on paper are easy to lead to errors, data loss, which takes a lot of time as well as is not focused, easy to repeat many times. Understanding these requirements, AIT company wants to build a software to manage the company's business system to meet the basic tasks. \
This is the [Software Requirements Document](https://docs.google.com/document/d/1XslZNEgI-ZwAGA6tdiFFctRs1TUKHNEG/edit?usp=sharing&ouid=116992013396456829835&rtpof=true&sd=true)

![Dashboard](wwwroot/img/demoPage/dashboard.png)


## Video Demo
You can have a look with AIT Business management system through [the video](https://drive.google.com/file/d/13xiX5x_VPI3gQ6fpwlVZXEeGYY2wQXCk/view).


## Technologies Used
AIT Business management system should use the following technologies, frameworks and development techniques:

- ASP.NET MVC
- MS SQL Server as database back-end
- Entity Framework to access database
- Bootstrap SB Admin2 
- The standard ASP.NET Identity System for managing users and roles
- AJAX communication in some parts of web application
- Apply error handling and data validation to avoid crashes when invalid data is entered



## Features
A few of the things you can do with AIT Business management system:
- Register
- Login
- Manage account
- Manage employee
- Manage employee skills
- Manage employee positions
- Manage salary
- Manage customer
- Manage product
- Manage product type
- Manage supplier
- Manage billSetup

> EmployeeManagement Page
![EmployeeManagement](wwwroot/img/demoPage/employeeIndex.png)


> Profile Page
![Dashboard](wwwroot/img/demoPage/profile.png)


> Bill Creation Page
![Dashboard](wwwroot/img/demoPage/createBill.png)


## Deployment
Web is deploying on [AIT Bussiness Management System](http://lucasuit-001-site1.etempurl.com/) until Feb 13, 2022.\
After that date, this link will be disable.\
Please run it on localhost with below step!
## How to install and run the project
To clone and run this application, you'll need [.Net Framework](https://dotnet.microsoft.com/en-us/download/dotnet-framework), [Visual Studio](https://visualstudio.microsoft.com/) and [MS SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) installed on your computer. 

It requires .Net version 5.0.

You might want to look into `appsettings.json` to make change connection string: 
```
"AppMvcConnectionString" : "Data Source=[YourComputerName];Initial Catalog=[YourDatabaseName];User Id=[YourUserID];Password=[YourPassWord];"\
"AppMvcConnectionString" : "Data Source=localhost,1433; Initial Catalog=appmvc; User ID=SA;Password=Password123”
```

Set up
Go to https://localhost:5001/database-manage/Index → Seed data, to create admin account

After that your command line will be in terminal:

```
- Install dotnet
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator

// run docker for Azure database
docker-compose up -d

// build table for database
dotnet ef migrations add Init
dotnet ef database update
```

```
dotnet restore
dotnet watch run
```

## Usage
After you clone this repo to your desktop, go to its root directory and run `dotnet restore` to install its dependencies.\
Once the dependencies are installed, you can run `dotnet watch run` to start the application. You will then be able to access it at localhost:5000

To enter AIT Business management system and use as administrator, you should:
- Register new account
- After enter the system, go to Database Management, then click `Seed Data` to create administrator account
- To check administrator account, log out current account
- Log in again with administrator account.\
    UserName: `admin`.\
    Password: `admin123`. 

## Acknowledgements
- Thanks to [Microsoft](https://www.microsoft.com/vi-vn/) for supporting us with Asp.net documentation
- Thanks to [Stackoverflow](https://stackoverflow.com/) for supporting us 
- Thanks to [Github](https://github.com/) for supporting us control application's versions 
- Thanks to [SmarterASP.NET](https://www.smarterasp.net/) for supporting us hosing web application


## AIT Team
- [Tran Quoc Thang](https://github.com/LucasTran-tq)
- [Nguyen Huu Phat](https://github.com/nguyenhuuphat2001)
- [Bui Thanh Phu](https://github.com/phubuideptrai)


## 📝 License

Copyright © 2021. <br />
This project is [MIT](https://github.com/LucasTran-tq/Business-Management-AspNet/blob/main/LICENSE) licensed.
