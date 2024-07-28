# matchDetailsApp

This project is a combination of .NET Core backend and Angular frontend.

## Description

This repository contains the source code for a web application built with .NET Core for the backend and Angular for the frontend. The backend is responsible for handling data storage, business logic, and API endpoints, while the frontend provides a user-friendly interface for interacting with the application.

## Features

- **.NET Core Backend:**
  - RESTful API endpoints
  - Data storage with MS SQL Server
  - Entity Framework Core for data access

- **Angular Frontend:**
  - Angular Versio 18
  - Single-page application (SPA) architecture
  - Responsive design
  - Authguard to prevent unauthorized access
  - Service for managing API
  - Angular material
  - Angular flex
  - Angular Forms

## Application Process

The Application process is divided into the following steps:

1. **Registration process:** Navigate to the registration section of the application to sign up as a new user.
   
2. **Login process:** Enter your login credentials to access further functionalities of the application
   
3. **Upload process:** Choose and upload an XML file to import data into the system.

4. **Show the successful message to the user:** Upon successful upload, a confirmation message will be displayed to the user.
  
5. **Match Day Selection:** Use the dropdown list to select a specific match day and view available matches.
   
6. **Match Date Selection:** Use the dropdown list to select a specific match date and view the match schedule for that date.
    
7. **Team details:** Click on a team name to view detailed information about the team. This feature is available on both the match by day and match by date pages. Users can navigate back and select another team to view its details.


## Project Objectives

This project aims to achieve the following objectives:

- **Fault Tolerance:** Implement measures to handle errors gracefully and ensure the application remains operational even in the face of failures.

- **Security:** Employ best practices for data security to protect user data and prevent unauthorized access.

- **Usability:** Design an intuitive user interface and user experience to make the registration process smooth and easy to understand.

- **Maintainability:** Write clean, modular, and well-documented code to facilitate future maintenance and updates.

## Prerequisites

Before running the application, ensure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Important Step

1. **Change the server name in appsettings.json to access the database:** Before using the database and migration, update the server name in the `appsettings.json` file to match your MS SQL Server instance.
