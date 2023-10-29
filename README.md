# LibraryManagementAPI

The LibraryManagementAPI project is a .NET web application that uses a SQL Server database for managing a library.

## Table of Contents
- [LibraryManagementAPI](#library-management-api)
  - [Table of Contents](#table-of-contents)
  - [Requirements](#requirements)
  - [Setup](#setup)
      - [Database Setup](#database-setup)
      - [Running the Application](#running-the-application)
      - [Accessing the API](#accessing-the-api)
  - [API Endpoints](#api-endpoints)
  - [Authorization](#authorization)
      - [Login](#login)
      - [Register](#register)

## Requirements
Before running the LibraryManagementAPI project, ensure you have the following prerequisites installed:
- [.NET SDK](https://dotnet.microsoft.com/download) (need .NET 6)
- [SQL Server for Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Setup
1. Clone the project repository from GitHub.
2. Navigate to the `API` directory.

#### Database Setup
3. Open `appsettings.Development.json` in the `LibraryManagementAPI` project and update the `ConnectionStrings` section with your SQL Server credentials.

#### Running the Application
4. Use Visual Studio to run LibraryManagementAPI project

#### Accessing the API

Once the LibraryManagementAPI is running, you can access it through the following URL:

- Swagger Documentation: https://localhost:7049/swagger

## API Endpoints

- **GET /api/account**: Get a list of all users.
- **GET /api/account/{id}**: Get details of a user by its Id.
- **DELETE /api/account/{id}**: Delete a user by providing the user Id (requires authorization).
- **PUT /api/account/{id}**: Update an existing user's details (requires authorization).

- **GET /api/books**: Get a list of all books in the library.
- **GET /api/books/{id}**: Get details of a book by its Id
- **GET /api/books/isbn/{isbn}**: Get details of a book by its ISBN.
- **POST /api/books**: Add a new book to the library (requires authorization).
- **PUT /api/books/{id}**: Update an existing book's details (requires authorization).
- **DELETE /api/books/{id}**: Remove a book from the library (requires authorization).

## Authorization
The LibraryManagementAPI project uses JWT (JSON Web Tokens) for authorization. To perform authorized actions, you must obtain an access token by following the steps below:

#### Login
To obtain an access token for an existing user, use the following endpoint:

- **POST /api/account/login**: Provide your login and password in the request body to receive an access token.

#### Register
If you are a new user, you can register and receive an access token using the following endpoint:

- **POST /api/account/register**: Register as a new user to obtain an access token.

With the access token obtained from either the login or register endpoint, include it in the Authorization header of subsequent requests as follows: `Authorization: Bearer <your_access_token>`
