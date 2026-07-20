# MatchDetailsApp

## Overview

**MatchDetailsApp** is a full-stack web application built with **.NET 8**, **ASP.NET Core Web API**, and **Angular 18**. The application allows users to register, authenticate, upload XML match data, and explore football match information through an intuitive user interface.

The project follows a modern client-server architecture with a secure REST API, SQL Server database, and a responsive Angular frontend.

---

# ✨ New Feature: MCP Server Integration

This project now includes **Model Context Protocol (MCP) Server** integration to provide AI-assisted development and maintenance capabilities.

The MCP Server enables AI tools (such as **GitHub Copilot**, **Claude Desktop**, **VS Code AI extensions**, or other compatible MCP clients) to interact directly with the project, providing richer context and improving developer productivity.

## 🚀 Benefits of MCP Integration

- AI understands the entire project structure
- Faster debugging and code analysis
- Context-aware code generation
- Intelligent API and database assistance
- Improved developer productivity
- Easier maintenance of large codebases
- Better understanding of business logic across backend and frontend

---

# 🏗️ Architecture

```text
                Angular 18 Frontend
                        │
                        ▼
            ASP.NET Core REST API
                        │
                        ▼
                Business Logic Layer
                        │
                        ▼
             Entity Framework Core
                        │
                        ▼
               Microsoft SQL Server
```

The MCP Server provides AI with contextual access to the project, making development, debugging, and maintenance more efficient.

---

# ✨ Features

## 🔹 Backend (.NET 8)

- ASP.NET Core RESTful APIs
- Entity Framework Core
- Microsoft SQL Server
- JWT Authentication
- Role-Based Authorization
- XML File Processing
- Business Logic Implementation
- Dependency Injection
- Repository Pattern
- Error Handling & Validation

---

## 🔹 Frontend (Angular 18)

- Angular 18
- Angular Material
- Angular Flex Layout
- Responsive UI
- Route Guards (AuthGuard)
- Reactive Forms
- API Services
- Authentication
- User-Friendly Dashboard
- Dynamic Filtering & Navigation

---

## 🤖 AI Development Support

- MCP Server Integration
- AI-Assisted Code Navigation
- Context-Aware Project Understanding
- Intelligent Debugging Support
- Faster Feature Implementation
- Improved Developer Workflow

---

# 🔄 Application Workflow

### 1️⃣ Registration

Create a new user account.

### 2️⃣ Login

Authenticate using your credentials.

### 3️⃣ Upload XML

Upload an XML file containing football match data.

The backend parses the XML and stores the data in SQL Server.

### 4️⃣ Success Notification

A confirmation message is displayed after a successful upload.

### 5️⃣ Match Day Selection

Select a match day to display all available matches.

### 6️⃣ Match Date Selection

Filter matches by a selected date.

### 7️⃣ Team Details

Click on a team to view detailed team information and navigate back to continue browsing.

---

# 🛠️ Technology Stack

## Backend

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Microsoft SQL Server
- JWT Authentication

## Frontend

- Angular 18
- TypeScript
- Angular Material
- Angular Flex Layout
- HTML5
- CSS3

## AI & Development Tools

- MCP Server
- GitHub
- Git
- Visual Studio
- Visual Studio Code

---

# Project Objectives

- Build a secure and scalable web application
- Provide a responsive and user-friendly interface
- Process XML data efficiently
- Maintain clean and modular architecture
- Improve maintainability through AI-assisted development using MCP
- Ensure fault tolerance and robust error handling

---

# Prerequisites

Before running the project, install:

- .NET 8 SDK
- Node.js
- Angular CLI
- Microsoft SQL Server
- Git

---

# ⚙️ Configuration

Update the SQL Server connection string in **appsettings.json** before running the application.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MatchDetailsDB;Trusted_Connection=True;"
}
```

After updating the connection string, run the Entity Framework Core migrations.

---

# 🔒 Security

- JWT Authentication
- Route Protection (AuthGuard)
- Secure REST APIs
- Input Validation
- Exception Handling
- Role-Based Authorization

---

# 🚀 Future Improvements

- Docker Support
- CI/CD Pipeline
- Azure Cloud Deployment
- Unit & Integration Testing
- Real-Time Notifications
- AI-Powered Developer Workflows using MCP
- Analytics Dashboard

---
