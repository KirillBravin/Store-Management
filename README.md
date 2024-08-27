# Store Management App

Welcome to the Store Management App – a powerful solution designed to help you efficiently manage your store’s products, users, and orders. This application provides a full suite of features for creating, modifying, deleting, and viewing all the essential data needed to run your store smoothly.

## Features

 * Product Management: Create, update, delete, and view products in your store.
  
 * User Management: Handle user data, including creating, modifying, and removing users.
  
 * Order Management: Manage orders, including creation, modification, and deletion.


## Getting Started
### Prerequisites

Ensure you have the following installed on your machine:

 * .NET SDK
 * Entity Framework Core
 * A database setup (SQL Server or MongoDB)

## Installation

  **Run Database Migrations**
     
  Open your API terminal and execute the following commands to set up the database:
  ```
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

  Technologies Used

## This app is built using a range of modern technologies and best practices:

  * **C#**: The primary programming language used for the application.
  
  * **Entity Framework Core**: For interacting with the SQL database.
  
  * **ASP.NET Core**: To create RESTful APIs.
  
  * **MongoDB Driver**: To interact with MongoDB for NoSQL data handling.
  
  * **Moq**: For mocking dependencies in unit tests.
  
  * **Serilog**: For structured logging and monitoring.

## Architectural Principles

  **Object-Oriented Programming (OOP)**: The application follows OOP principles to promote reusability, scalability, and maintainability.
  ### Separation of Concerns:
  
  * **Repositories**: Handle data access logic, segregating it from business logic.
      
  * **Services**: Manage the core business logic of the application.
      
  * **Controllers**: Serve as the bridge between the client requests and the server, exposing RESTful APIs.


## Data Handling

This application supports both SQL and NoSQL databases, ensuring flexibility in data storage. Entity Framework is used for SQL operations, while the MongoDB driver is utilized for NoSQL data.

## Logging

All key operations and errors are logged using Serilog, providing detailed insights into the application’s behavior and performance.

## Visuals

While the original graph showcasing data insights was lost, you can find screenshots of similar graphs in the repository. These images give a general idea of how data visualization is handled within the app.
