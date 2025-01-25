# Recipe Sharing Application - WebAPI

![License](https://img.shields.io/badge/license-MIT-blue.svg)

A RESTful Web API for sharing and managing recipes. This application allows users to create, read, update, and delete recipes, as well as search for recipes based on various criteria. It is built with scalability and maintainability in mind, leveraging modern technologies and design patterns.

## Table of Contents

- [Technologies Used](#technologies-used)
- [Features](#features)
- [Design Patterns](#design-patterns)
- [Installation](#installation)
- [Usage](#usage)

---

## Technologies Used

- **Backend Framework**: ASP.NET Core
- **Database**: SQL Server (with Entity Framework Core for ORM)
- **Authentication**: JWT (JSON Web Tokens) - ASP.NET Identity
- **Dependency Injection**: Built-in ASP.NET Core DI Container
- **Caching**: In-Memory Caching
- **Testing**: xUnit (for unit testing)
- **Version Control**: Git
- **CI/CD**: GitHub Actions (optional)
- **FluentValidation**: Validation of incoming requests.

---

## Features

- **User Authentication**: Secure user registration and login using JWT.
- **Recipe Management**:
  - Create, read, update, and delete recipes.
  - Search recipes by name, ingredients, or tags.
- **Pagination**: Support for paginated responses for large datasets.
- **Validation**: Input validation for all API requests.
- **Real-Time Notifications**: Notify users in real time when their recipes are liked or when they are followed by other users.
- **Follow System**: Users can follow each other to stay updated on new recipes.
- ***Like System*: Users can like recipes to show appreciation.
- **Modular Architecture**: The application is divided into multiple projects (Web API, Data Access Library, Service Library) for better maintainability.
---

## Design Patterns

- **Repository Pattern**: Used to abstract data access logic and promote separation of concerns.
- **Dependency Injection**: Leveraged for loose coupling and testability.
- **DTOs (Data Transfer Objects)**: Used to transfer data between layers while reducing unnecessary data exposure.
- **Middleware**: Custom middleware for global error handling and logging.
---

## Installation

Follow these steps to set up the project locally:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/behradkamyab/RecipeSharingApplication-WebAPI.git
   cd RecipeSharingApplication-WebAPI
---
## API Endpoints
**Method** **Endpoint**	 **Description**
POST	/api/auth/register	Register a new user.
POST	/api/auth/login	Log in and receive a JWT token.
GET	/api/recipes	Get all recipes (paginated).
GET	/api/recipes/{id}	Get a recipe by ID.
POST	/api/recipes	Create a new recipe.
PUT	/api/recipes/{id}	Update an existing recipe.
DELETE	/api/recipes/{id}	Delete a recipe.
GET	/api/recipes/search	Search recipes by name or ingredients.
