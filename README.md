# Books Management System

## Overview

Books Management System is an ASP.NET Core MVC application for managing books in a library. It includes features such as listing, creating, editing, and deleting books and borrowings.

## Features

- List all books with pagination
- Search for books by title or author
- Sort books by title, author, or publication year
- Create, edit, and delete book records
- List all members with pagination
- Search for members by name or author
- Create, edit, and delete member records
- Manage borrowings with borrowing and return dates

## Technologies Used

- ASP.NET Core MVC
- The code does not use stored procedures or EF Core. Instead, it directly executes SQL commands within the application code using inline SQL queries.
- SQL Server
- X.PagedList

## Setup Instructions

1. Clone the repository:
    ```sh
    git clone https://github.com/WallStreetWo/BooksManagementSystem.git
    cd repository-name
    ```

2. Update the connection string in `appsettings.json` to point to your SQL Server database.

3. Apply the migrations and seed the database:
    ```sh
    dotnet ef database update
    ```

4. Run the application:
    ```sh
    dotnet run
    ```

## Usage

Navigate to `https://localhost:5100`


